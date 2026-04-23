using System.Collections;
using UnityEngine;

public class CirclePuzzleWinManager : MonoBehaviour
{
    [System.Serializable]
    public class WinCondition
    {
        public CirclePuzzleSlot slot;
        public CirclePuzzleSymbol[] allowedSymbols;
    }

    [SerializeField] private WinCondition[] winConditions = new WinCondition[8];
    [SerializeField] private RotatingCirclePuzzle rotatingCirclePuzzle;
    [SerializeField] private BoxCollider[] boxColliders;
    [SerializeField] private ChestManager chestManager;
    [SerializeField] private float chestMoveDistanceX = 1f;
    [SerializeField] private float chestMoveDuration = 3f;

    [SerializeField] private RotatingCircleInPuzzle circleObject;
    [SerializeField] private OpenTrapDoorsManager openTrapDoorsManager;
    [SerializeField] private RotateAllGearsRoomTen rotateAllGearsRoomTen;


    private void Start()
    {
        foreach (BoxCollider boxCollider in boxColliders)
        {
            boxCollider.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (circleObject != null)
            circleObject.OnActiveStateChanged += HandleCirclePuzzleActiveChanged;

    }

    private void OnDisable()
    {
        if (circleObject != null)
            circleObject.OnActiveStateChanged -= HandleCirclePuzzleActiveChanged;

    }

    private void HandleCirclePuzzleActiveChanged(RotatingCircleInPuzzle circle, bool isActive)
    {
        CheckPlayersActiveState();
    }


    private void CheckPlayersActiveState()
    {
        if (circleObject == null) return;

        if (circleObject.IsActive)
        {
            ActiveateColliders();
        }
    }

    public void CheckWinPuzzle()
    {
        for (int i = 0; i < winConditions.Length; i++)
        {
            WinCondition condition = winConditions[i];

            if (condition == null) return;
            if (condition.slot == null) return;
            if (condition.allowedSymbols == null || condition.allowedSymbols.Length == 0) return;
            if (condition.slot.CurrentSymbol == null) return;

            if (!IsAllowedSymbol(condition.slot.CurrentSymbol, condition.allowedSymbols))
            {
                return;
            }
        }

        WinPuzzle();
    }

    private bool IsAllowedSymbol(CirclePuzzleSymbol currentSymbol, CirclePuzzleSymbol[] allowedSymbols)
    {
        for (int i = 0; i < allowedSymbols.Length; i++)
        {
            if (allowedSymbols[i] == currentSymbol)
            {
                return true;
            }
        }

        return false;
    }

    private void WinPuzzle()
    {
        rotatingCirclePuzzle.ExitAfterWin();
        foreach (BoxCollider boxCollider in boxColliders)
        {
            boxCollider.enabled = false;
        }
        StartChestAnimation();
        openTrapDoorsManager.CloseAnimation();
        rotateAllGearsRoomTen.RotateTenGears();
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private void StartChestAnimation()
    {
        StartCoroutine(ChestAnimation());
    }

    private IEnumerator ChestAnimation()
    {
        yield return new WaitForSeconds(1f);
        Services.Audio.PlaySFX("ChestMovingStone");
        Vector3 startPosition = chestManager.transform.position;
        Vector3 targetPosition = startPosition + Vector3.left * chestMoveDistanceX;

        float time = 0f;

        while (time < chestMoveDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / chestMoveDuration);

            chestManager.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        chestManager.transform.position = targetPosition;
        chestManager.OpenChest();
    }


    public void ActiveateColliders()
    {
        foreach (BoxCollider boxCollider in boxColliders)
        {
            boxCollider.enabled = true;
        }
    }

}
