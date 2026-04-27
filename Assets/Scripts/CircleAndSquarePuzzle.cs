using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleAndSquarePuzzle : MonoBehaviour
{
    [SerializeField] private InputActionReference thisModeInput;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private Canvas blurCanvas;
    [SerializeField] private PlayerBehaviour playerBehaviour;
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

        StartCoroutine(DisablePlayerLook());
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

        playerBehaviour.disableOnlyMovement = true;
        playerBehaviour.gamepadSensitivity = 25;
        UIManager.Instance.EnableCircleAndSquarePuzzleHintPanel();

    }

    public void ExitPuzzleMode()
    {
        winPuzzle = false;
        boxCollider.enabled = true;
        blurCanvas.gameObject.SetActive(false);
        ResetPuzzle();
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        playerBehaviour.gamepadSensitivity = 100;
        UIManager.Instance.DisableSharedPanelText();
    }

    public void ExitAfterWin()
    { 
        winPuzzle = true;
        blurCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        playerBehaviour.gamepadSensitivity = 100;
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
                     + player.right * -0.122f;

        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, 0f));
    }

    private IEnumerator DisablePlayerLook()
    {
        yield return null;

        playerBehaviour.disablePlayer = true;

        playerBehaviour.ResetCameraRotation();

        yield return new WaitForSeconds(0.1f);

        playerBehaviour.disablePlayer = false;
    }
}
