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
    //[SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform point;
    [SerializeField] private BoxCollider boxCollider;

    [SerializeField] private SafeDial safeDial;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorOne;

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

        PlayerControlManager.Instance.SetCharacterControllerEnabled(false);


        player.SetPositionAndRotation(point.position, point.rotation);

        PlayerControlManager.Instance.SetCharacterControllerEnabled(true);

    }

    private IEnumerator DisablePlayerLook()
    {
        yield return null;

        PlayerControlManager.Instance.LockPlayer();

        PlayerControlManager.Instance.ResetCamera();


        yield return new WaitForSeconds(0.1f);
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
        safeDial.canRotateDial = true;
        MovePlayerToPosition();
        SpawnThisObjectInFronOfPlayer();
        blurCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);
        boxCollider.enabled = false;
        PlayerControlManager.Instance.LockMovementOnly();
        UIManager.Instance.EnableSafeRotateCodeTextPanel();
    }

    public void ExitPuzzleMode()
    {
        boxCollider.enabled = true;

        blurCanvas.gameObject.SetActive(false);
        ResetPuzzle();
        gameObject.SetActive(false);
        PlayerControlManager.Instance.UnlockMovementOnly();
        UIManager.Instance.DisableSharedPanelText();

        PlayerControlManager.Instance.UnlockPlayer();
    }

    private void ResetPuzzle()
    {
        safeDial.InstantResetDial();
    }

    public void WinPuzzle()
    {
        StartCoroutine(WinPuzzleCourutine());
    }

    private IEnumerator WinPuzzleCourutine()
    {
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");

        safeDial.canRotateDial = false;

        animator.SetTrigger("Open");

        yield return new WaitForSeconds(1.4f);

        blurCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
        PlayerControlManager.Instance.UnlockMovementOnly();
        UIManager.Instance.DisableSharedPanelText();

        PlayerControlManager.Instance.UnlockPlayer();

        animatorOne.SetTrigger("Open");
    }
}
