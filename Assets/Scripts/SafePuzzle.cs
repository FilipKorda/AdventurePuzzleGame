using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SafePuzzle : MonoBehaviour
{
    [SerializeField] private InputActionReference thisModeInput;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private float horizontalOffset = 0f;
    [SerializeField] private Canvas blurCanvas;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform point;
    [SerializeField] private BoxCollider boxCollider;

    private void OnEnable()
    {
        if (thisModeInput != null)
        {
            thisModeInput.action.Enable();
            thisModeInput.action.performed += OnGearModePerformed;
        }

        StartCoroutine(DisablePlayerLook());
    }

    private void OnDisable()
    {
        if (thisModeInput != null)
        {
            thisModeInput.action.performed -= OnGearModePerformed;
            thisModeInput.action.Disable();
        }
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

    private IEnumerator DisablePlayerLook()
    {
        yield return null;

        playerBehaviour.disablePlayer = true;

        playerBehaviour.ResetCameraRotation();

        yield return new WaitForSeconds(0.1f);

        playerBehaviour.disablePlayer = false;
    }

    private void SpawnThisObjectInFronOfPlayer()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        var position = player.position
                     + player.forward * distanceFromCamera
                     + player.up * verticalOffset
                      + player.right * horizontalOffset;

        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 90f, 0f));
    }

    private void OnGearModePerformed(InputAction.CallbackContext context)
    {
        ExitPuzzleMode();
    }

    public void EnterPuzzleMode()
    {
        MovePlayerToPosition();
        SpawnThisObjectInFronOfPlayer();
        blurCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);
        boxCollider.enabled = false;
        playerBehaviour.disableOnlyMovement = true;
        UIManager.Instance.EnableGearModePanel();
    }

    public void ExitPuzzleMode()
    {
        boxCollider.enabled = true;

        blurCanvas.gameObject.SetActive(false);
        ResetPuzzle();
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableGearModePanel();
    }

    private void ResetPuzzle()
    {


    }
}
