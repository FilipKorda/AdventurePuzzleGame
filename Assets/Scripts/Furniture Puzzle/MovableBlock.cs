using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BlockType
{
    Short = 2,
    Long = 3
}
public enum BlockDirection { Horizontal, Vertical }

public class MovableBlock : MonoBehaviour
{
    [SerializeField] private InputActionReference movableBlockLeaveInput;
    [SerializeField] private InputActionReference movableBlockMoveInput;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private FurniturePuzzle furniturePuzzle;

    [SerializeField] private BoxCollider itemInteractableCollider;
    [SerializeField] private BoxCollider EndCollider;


    public BlockType blockType;
    public BlockDirection direction;

    private float gridSize = 1f;
    public LayerMask obstacleLayer;

    private bool canMove;
    private bool isMoving = false;


    public void DisabelThisMovableBlock()
    {
        itemInteractableCollider.enabled = false;
        GetComponentInChildren<InteractableItem>().enabled = false;
    }

    public void TakeControlOfTheThiBlock()
    {
        if (movableBlockLeaveInput != null)
        {
            movableBlockLeaveInput.action.Enable();
            movableBlockLeaveInput.action.performed += OnMovableBlockPerformed;
        }

        if (movableBlockMoveInput != null)
        {
            movableBlockMoveInput.action.Enable();
            movableBlockMoveInput.action.performed += MoveInput;
        }

        playerBehaviour.disablePlayer = true;
        canMove = true;

        if (direction == BlockDirection.Vertical)
        {
            UIManager.Instance.EnableVerticalFurnitureModePanel();
        }
        else
        {
            UIManager.Instance.EnableHorizontalFurnitureModePanel();
        }
    }

    public void LeaveControlOfThisBlock()
    {
        if (movableBlockLeaveInput != null)
        {
            movableBlockLeaveInput.action.performed -= OnMovableBlockPerformed;
            movableBlockLeaveInput.action.Disable();
        }

        if (movableBlockMoveInput != null)
        {
            movableBlockMoveInput.action.performed -= MoveInput;
            movableBlockMoveInput.action.Disable();
        }

        playerBehaviour.disablePlayer = false;
        canMove = false;

        if (direction == BlockDirection.Vertical)
        {
            UIManager.Instance.DisableSharedPanelText();
        }
        else
        {
            UIManager.Instance.DisableSharedPanelText();
        }
    }

    private void OnMovableBlockPerformed(InputAction.CallbackContext context)
    {
        LeaveControlOfThisBlock();
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        if (!canMove || isMoving)
            return;

        Vector2 input = context.ReadValue<Vector2>();
        Vector3 move = Vector3.zero;

        if (direction == BlockDirection.Vertical)
        {
            if (input.y > 0.5f)
                move = Vector3.forward;
            else if (input.y < -0.5f)
                move = Vector3.back;
        }
        else if (direction == BlockDirection.Horizontal)
        {
            if (input.x > 0.5f)
                move = Vector3.right;
            else if (input.x < -0.5f)
                move = Vector3.left;
        }

        if (move != Vector3.zero && !IsObstacleInDirection(move))
        {
            Services.Audio.PlaySFX("FurnitureMove");
            StartCoroutine(MoveBlockCoroutine(move * gridSize, 2.9f));
        }
    }

    private IEnumerator MoveBlockCoroutine(Vector3 moveVector, float duration)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + moveVector;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EndCollider == null) return;
        if (other == EndCollider)
        {
            LeaveControlOfThisBlock();

            StartCoroutine(MoveToTarget(new Vector3(0f, 0f, -3f), Quaternion.Euler(0f, 0f, 0f), 0.6f));
        }
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            transform.localPosition = Vector3.Lerp(startPos, targetPosition, t);
            transform.localRotation = Quaternion.Slerp(startRot, targetRotation, t);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;

        if (furniturePuzzle != null)
        {
            furniturePuzzle.DisableAllMovableBlocks();
            furniturePuzzle.ActiveOpenShelf();
        }
    }

    bool IsObstacleInDirection(Vector3 direction)
    {
        float distance = gridSize;

        if (Physics.Raycast(transform.position, direction, distance, obstacleLayer))
            return true;

        if (blockType == BlockType.Long)
        {
            Vector3 secondCheckPos = transform.position + direction * distance;
            if (Physics.Raycast(secondCheckPos, direction, distance, obstacleLayer))
                return true;
        }

        return false;
    }
}