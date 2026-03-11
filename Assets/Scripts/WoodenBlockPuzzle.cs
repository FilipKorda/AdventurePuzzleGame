using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WoodenBlockPuzzle : MonoBehaviour
{
    [SerializeField] private InputActionReference woodenPuzzleLeaveInput;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Canvas blurCanvas;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private Transform point;
    [SerializeField] private BoxCollider woodenCreate;

    [SerializeField] private InteractableItem[] woodenBlocks;
    [SerializeField] private Color baseColor;
    [SerializeField] private Animator animatorGateLeft;
    [SerializeField] private Animator animatorGateRight;

    private void OnEnable()
    {
        if (woodenPuzzleLeaveInput != null)
        {
            woodenPuzzleLeaveInput.action.Enable();
            woodenPuzzleLeaveInput.action.performed += OnWoodenPuzzlePerformed;
        }
        StartCoroutine(DisablePlayerLook());
    }

    private void OnDisable()
    {
        if (woodenPuzzleLeaveInput != null)
        {
            woodenPuzzleLeaveInput.action.performed -= OnWoodenPuzzlePerformed;
            woodenPuzzleLeaveInput.action.Disable();
        }
    }

    private void OnWoodenPuzzlePerformed(InputAction.CallbackContext context)
    {
        ExitWoodenPuzzleMode();
    }

    public void EnterWoodenPuzzleMode()
    {
        MovePlayerToPosition();
        SpawnThisObjectInFronOfPlayer();
        blurCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);
        woodenCreate.enabled = false;
        playerBehaviour.disableOnlyMovement = true;
        UIManager.Instance.EnableWoodenPuzzlePanel();
    }

    public void ExitWoodenPuzzleMode()
    {
        foreach (var block in woodenBlocks)
        {
            block.ResetblockInstant(baseColor);
        }

        PuzzleBlockManager.Instance.ResetClickedBlock();
        blurCanvas.gameObject.SetActive(false);
        woodenCreate.enabled = true;
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableWoodenPuzzlePanel();
    }

    public void PuzzleWon()
    {
        foreach (var block in woodenBlocks)
        {
            block.GetComponent<BoxCollider>().enabled = false;
        }
        woodenCreate.enabled = false;

        animatorGateLeft.SetTrigger("Open");    
        animatorGateRight.SetTrigger("Open");    

        ExitWoodenPuzzleMode();
    }

    private void SpawnThisObjectInFronOfPlayer()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        var position = player.position
                     + player.forward * distanceFromCamera
                     + player.up * verticalOffset;

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

}
