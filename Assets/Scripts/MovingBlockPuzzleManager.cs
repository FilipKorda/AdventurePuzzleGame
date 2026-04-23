using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBlockPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject objectA;
    [SerializeField] private GameObject objectB;
    [SerializeField] private Vector3 correctPosition1;
    [SerializeField] private Vector3 correctPosition2;
    [SerializeField] private BoxCollider[] arrows;
    [SerializeField] private BoxCollider puzzlepartCollider;
    private float positionTolerance = 0.01f;


    [SerializeField] private InputActionReference gearModeInput;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private Canvas blurCanvas;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform point;

    public bool puzzleWin = false;
    [SerializeField] private Vector3 basePositionObject1;
    [SerializeField] private Vector3 basePositionObject2;


    [SerializeField] private GameObject objectAAtrapa;
    [SerializeField] private GameObject objectBAtrapa;
    [SerializeField] private Vector3 basePositionObject1Atrapa;
    [SerializeField] private Vector3 basePositionObject2Atrapa;
    [SerializeField] private PipePuzzleManager pipePuzzleManager;


    private void OnEnable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.Enable();
            gearModeInput.action.performed += OnMovingPuzzleModePerformed;
        }

        StartCoroutine(DisablePlayerLook());
    }

    private void OnDisable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.performed -= OnMovingPuzzleModePerformed;
            gearModeInput.action.Disable();
        }
    }


    public void EnterMovingBlockPuzzle()
    {
        MovePlayerToPosition();
        SpawnThisObjectInFronOfPlayer();
        blurCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);
        puzzlepartCollider.enabled = false;
        playerBehaviour.disableOnlyMovement = true;
        UIManager.Instance.EnableLpmToClickPanel();

    }

    public void ExitMovingPuzzleMode()
    {
        puzzleWin = false;
        puzzlepartCollider.enabled = true;
        blurCanvas.gameObject.SetActive(false);
        ResetObjectPosition();
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableSharedPanelText();
    }

    private void ResetObjectPosition()
    {
        objectA.transform.localPosition = basePositionObject1;
        objectB.transform.localPosition = basePositionObject2;
    }

    private void SetObjectAtrapa()
    {
        objectAAtrapa.transform.localPosition = basePositionObject1Atrapa;
        objectBAtrapa.transform.localPosition = basePositionObject2Atrapa;
    }

    private void OnMovingPuzzleModePerformed(InputAction.CallbackContext context)
    {
        ExitMovingPuzzleMode();
    }

    private void MovePlayerToPosition()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        if (playerBehaviour.TryGetComponent<CharacterController>(out var controller))
            controller.enabled = false;

        player.SetPositionAndRotation(point.position, point.rotation);

        if (controller != null)
            controller.enabled = true;
    }

    private void SpawnThisObjectInFronOfPlayer()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        var position = player.position
                     + player.forward * distanceFromCamera
                     + player.up * verticalOffset
                     + player.right * 0.26f;

        transform.SetPositionAndRotation(position, Quaternion.Euler(-90f, 0f, 0f));
    }

    private IEnumerator DisablePlayerLook()
    {
        yield return null;

        playerBehaviour.disablePlayer = true;

        playerBehaviour.ResetCameraRotation();

        yield return new WaitForSeconds(0.1f);

        playerBehaviour.disablePlayer = false;
    }

    public void CheckCorrectPositionOfAnObjects()
    {
        if (IsClose(objectA.transform.localPosition, correctPosition1) &&
            IsClose(objectB.transform.localPosition, correctPosition2))
        {
            WinPuzzle();
        }
    }

    private bool IsClose(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b) <= positionTolerance;
    }

    private void WinPuzzle()
    {
        foreach (var a in arrows)
        {
            a.enabled = false;
        }
        puzzleWin = true;
        ResetObjectPosition();
        SetObjectAtrapa();
        blurCanvas.gameObject.SetActive(false);
        ResetObjectPosition();
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableSharedPanelText();


        Services.Audio.PlaySFX("AfterGearPuzzleSolved");
        pipePuzzleManager.CheckWInBothPipePuzzle();

    }

}
