using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NinePadPanelManager : MonoBehaviour
{
    [SerializeField] private InputActionReference thisModeInput;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private float horizontalOffset = 0f;
    [SerializeField] private Canvas blurCanvas;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform point;
    [SerializeField] private BoxCollider boxCollider;

    [SerializeField] private BlockPanel blockPanel;
    [SerializeField] private BlockPanel blockPanel1;
    [SerializeField] private BlockPanel blockPanel2;
    [SerializeField] private BlockPanel blockPanel3;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject blockObject;


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
        UIManager.Instance.EnableLpmToClickPanel();

        Debug.Log("Wszedłeś w część Puzzle Pipe!");
    }

    public void ExitMovingPuzzleMode()
    {    
        boxCollider.enabled = true;
        blurCanvas.gameObject.SetActive(false);
        ResetPuzzle();
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableSharedPanelText();
    }

    private void OnPuzzleModePerformed(InputAction.CallbackContext context)
    {
        ExitMovingPuzzleMode();
    }

    private void ResetPuzzle()
    {
        blockPanel.ResetBlockPanel();
        blockPanel1.ResetBlockPanel();
        blockPanel2.ResetBlockPanel();
        blockPanel3.ResetBlockPanel();

        blockPanel.panelWin = false;
        blockPanel1.panelWin = false;
        blockPanel2.panelWin = false;
        blockPanel3.panelWin = false;  
    }

    public void CheckIfAllButtonsArePressed()
    {
        if (blockPanel.panelWin && blockPanel1.panelWin && blockPanel2.panelWin && blockPanel3.panelWin)
        {
            blurCanvas.gameObject.SetActive(false);
            ResetPuzzle();
            gameObject.SetActive(false);
            playerBehaviour.disableOnlyMovement = false;
            UIManager.Instance.DisableSharedPanelText();
            animator.SetTrigger("Interact");
            blockObject.SetActive(false);
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

    private void SpawnThisObjectInFronOfPlayer()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        var position = player.position
                     + player.forward * distanceFromCamera
                     + player.up * verticalOffset
                     + player.right * horizontalOffset;

        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, 90f));
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
