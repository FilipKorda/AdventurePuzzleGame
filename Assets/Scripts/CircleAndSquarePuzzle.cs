using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleAndSquarePuzzle : MonoBehaviour
{
    [SerializeField] private InputActionReference thisModeInput;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private Canvas blurCanvas;
   // [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform point;
    [SerializeField] private BoxCollider boxCollider;

    [SerializeField] private GameObject wheelObject;
    [SerializeField] private PlayerPathMovement playerPathMovement;
    [SerializeField] private PipePuzzleManager pipePuzzleManager;
    public bool winPuzzle = false;

    private void OnEnable()
    {
        if (thisModeInput != null)
        {
            thisModeInput.action.Enable();
            thisModeInput.action.performed += OnPuzzleModePerformed;
        }

        PlayerControlManager.Instance.DisablePlayerLookTemporarily();
    }

    private void OnDisable()
    {
        if (thisModeInput != null)
        {
            thisModeInput.action.performed -= OnPuzzleModePerformed;
            thisModeInput.action.Disable();
        }
    }

    public void EnterPuzzle()
    {
        boxCollider.enabled = false;
        MovePlayerToPosition();
        SpawnThisObjectInFronOfPlayer();
        blurCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);

        PlayerControlManager.Instance.LockMovementOnly();
        PlayerControlManager.Instance.SetGamepadSens(25);
        UIManager.Instance.EnableCircleAndSquarePuzzleHintPanel();

    }

    public void ExitPuzzleMode()
    {
        winPuzzle = false;
        boxCollider.enabled = true;
        blurCanvas.gameObject.SetActive(false);
        ResetPuzzle();
        gameObject.SetActive(false);
        PlayerControlManager.Instance.UnlockMovementOnly();
        PlayerControlManager.Instance.SetGamepadSens(100);
        UIManager.Instance.DisableSharedPanelText();
    }

    public void ExitAfterWin()
    { 
        winPuzzle = true;
        blurCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
        PlayerControlManager.Instance.UnlockMovementOnly();
        PlayerControlManager.Instance.SetGamepadSens(100);
        UIManager.Instance.DisableSharedPanelText();
        pipePuzzleManager.CheckWInBothPipePuzzle();
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private void OnPuzzleModePerformed(InputAction.CallbackContext context)
    {
        ExitPuzzleMode();
    }

    private void ResetPuzzle()
    {
        playerPathMovement.ResetToCurrentPoint();
        winPuzzle = false;
        wheelObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
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
                     + player.right * -0.122f;

        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, 0f));
    }
}
