using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PipeGearPuzzle : MonoBehaviour
{
    [SerializeField] private InputActionReference gearModeInput;
    [SerializeField] private Canvas blurCanvas;
    [SerializeField] private GameObject[] gears;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private GearCorrectIndexes[] correctGear90Indexes;
    //[SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform point;
    [SerializeField] private BoxCollider pipeGearPuzzleCollider;
    [SerializeField] private PipePuzzleManager pipePuzzleManager;
    public bool puzzleWin = false;

    private void OnEnable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.Enable();
            gearModeInput.action.performed += OnGearModePerformed;
        }

        PlayerControlManager.Instance.DisablePlayerLookTemporarily();
    }

    private void OnDisable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.performed -= OnGearModePerformed;
            gearModeInput.action.Disable();
        }
    }

    public void EnterGearLockMode()
    {
        MovePlayerToPosition();
        SpawnThisObjectInFronOfPlayer();
        blurCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);
        pipeGearPuzzleCollider.enabled = false;
        PlayerControlManager.Instance.LockMovementOnly();
        UIManager.Instance.EnableLpmToRotatePanel();
    }

    public void ExitGearLockMode()
    {
        pipeGearPuzzleCollider.enabled = true;

        blurCanvas.gameObject.SetActive(false);

        foreach (GameObject gear in gears)
        {
            if (gear.TryGetComponent<InteractableItem>(out var item))
            {
                item.ResetCurrentGear90Index();
            }
        }

        ResetGears();
        gameObject.SetActive(false);
        PlayerControlManager.Instance.UnlockMovementOnly();
        UIManager.Instance.DisableSharedPanelText();
    }

    private void OnGearModePerformed(InputAction.CallbackContext context)
    {
        ExitGearLockMode();
    }


    private void MovePlayerToPosition()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        PlayerControlManager.Instance.SetCharacterControllerEnabled(false);


        player.SetPositionAndRotation(point.position, point.rotation);

        PlayerControlManager.Instance.SetCharacterControllerEnabled(true);

    }

    private void SpawnThisObjectInFronOfPlayer()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        var position = player.position
                     + player.forward * distanceFromCamera
                     + player.up * verticalOffset
                     + player.right * -0.1f;

        transform.SetPositionAndRotation(position, Quaternion.Euler(-90f, 0f, 0f));
    }

    private void ResetGears()
    {
        foreach (GameObject gear in gears)
        {
            gear.transform.localRotation = Quaternion.identity;
        }

        puzzleWin = false;

    }

    public void CheckIfPuzzleSolved()
    {
        for (int i = 0; i < gears.Length; i++)
        {
            int current = gears[i]
                .GetComponent<InteractableItem>()
                .CurrentGear90Index;

            bool valid = false;

            for (int j = 0; j < correctGear90Indexes[i].indexes.Length; j++)
            {
                if (current == correctGear90Indexes[i].indexes[j])
                {
                    valid = true;
                    break;
                }
            }

            if (!valid)
                return;
        }

        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        StartCoroutine(CourutineSolvePuzzle());
    }

    private IEnumerator CourutineSolvePuzzle()
    {
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");

        puzzleWin = true;
        yield return new WaitForSeconds(0.5f);
        pipePuzzleManager.CheckWInBothPipePuzzle();
        blurCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
        PlayerControlManager.Instance.UnlockMovementOnly();
        UIManager.Instance.DisableSharedPanelText();
    }

}
[System.Serializable]
public class GearCorrectIndexes
{
    public int[] indexes;
}