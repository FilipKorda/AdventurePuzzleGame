using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BriefcaseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference thisModeInput;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private float horizotalOffset = 0f;
    [SerializeField] private Canvas blurCanvas;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform point;
    [SerializeField] private BoxCollider boxCollider;

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
        UIManager.Instance.EnableGearModePanel();

    }

    public void ExitPuzzleMode()
    {
        winPuzzle = false;
        boxCollider.enabled = true;
        blurCanvas.gameObject.SetActive(false);
        ResetPuzzle();
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableGearModePanel();
    }

    public void ExitAfterWin()
    {
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
        winPuzzle = true;
        blurCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableGearModePanel();
        // pipePuzzleManager.CheckWInBothPipePuzzle();
    }

    private void OnPuzzleModePerformed(InputAction.CallbackContext context)
    {
        ExitPuzzleMode();
    }

    private void ResetPuzzle()
    {
        //playerPathMovement.ResetToCurrentPoint();
        winPuzzle = false;
        // wheelObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
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
                     + player.right * horizotalOffset;

        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 90f, 90f));
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
