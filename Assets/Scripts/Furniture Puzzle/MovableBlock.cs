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
    [SerializeField] private FurniturePuzzle furniturePuzzle;

    [SerializeField] private BoxCollider itemInteractableCollider;
    [SerializeField] private BoxCollider EndCollider;


    public BlockType blockType;
    public BlockDirection direction;

    private float gridSize = 1f;
    public LayerMask obstacleLayer;

    private bool canMove;
    private bool isMoving = false;

    private Coroutine moveBlockRoutine;

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

        PlayerControlManager.Instance.LockPlayer();
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

        PlayerControlManager.Instance.UnlockPlayer();
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
            Vector3 toCamera = PlayerControlManager.Instance.GetPlayerCamera().transform.position - transform.position;

            toCamera.y = 0f;
            toCamera.Normalize();

            Vector3 blockForward = transform.forward;
            blockForward.y = 0f;
            blockForward.Normalize();

            Vector3 blockRight = transform.right;
            blockRight.y = 0f;
            blockRight.Normalize();

            float forwardDot = Vector3.Dot(toCamera, blockForward);
            float rightDot = Vector3.Dot(toCamera, blockRight);

            bool lookingMoreFromFrontOrBack = Mathf.Abs(forwardDot) >= Mathf.Abs(rightDot);

            if (lookingMoreFromFrontOrBack)
            {
                bool cameraIsBehindBlock = forwardDot > 0f;

                if (input.y > 0.5f)
                    move = cameraIsBehindBlock ? Vector3.back : Vector3.forward;
                else if (input.y < -0.5f)
                    move = cameraIsBehindBlock ? Vector3.forward : Vector3.back;
            }
            else
            {
                bool cameraIsOnRightSide = rightDot < 0f;

                if (input.x > 0.5f)
                    move = cameraIsOnRightSide ? Vector3.back : Vector3.forward;
                else if (input.x < -0.5f)
                    move = cameraIsOnRightSide ? Vector3.forward : Vector3.back;
            }
        }

        else if (direction == BlockDirection.Horizontal)
        {
            Vector3 toCamera = PlayerControlManager.Instance.GetPlayerCamera().transform.position - transform.position;
            toCamera.y = 0f;
            toCamera.Normalize();

            Vector3 blockForward = transform.forward;
            blockForward.y = 0f;
            blockForward.Normalize();

            Vector3 blockRight = transform.right;
            blockRight.y = 0f;
            blockRight.Normalize();

            float forwardDot = Vector3.Dot(toCamera, blockForward);
            float rightDot = Vector3.Dot(toCamera, blockRight);

            bool lookingMoreFromFrontOrBack = Mathf.Abs(forwardDot) >= Mathf.Abs(rightDot);

            if (lookingMoreFromFrontOrBack)
            {
                bool cameraIsBehindBlock = forwardDot > 0f;

                if (input.x > 0.5f)
                    move = cameraIsBehindBlock ? Vector3.left : Vector3.right;
                else if (input.x < -0.5f)
                    move = cameraIsBehindBlock ? Vector3.right : Vector3.left;
            }
            else
            {
                bool cameraIsOnRightSide = rightDot > 0f;

                if (input.y > 0.5f)
                    move = cameraIsOnRightSide ? Vector3.left : Vector3.right;
                else if (input.y < -0.5f)
                    move = cameraIsOnRightSide ? Vector3.right : Vector3.left;
            }
        }


        if (move != Vector3.zero && !IsObstacleInDirection(move))
        {
            Services.Audio.PlaySFX("FurnitureMove");
            moveBlockRoutine = StartCoroutine(MoveBlockCoroutine(move * gridSize, 2.9f));

        }
    }
    private bool reachedEnd;
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
        if (other != EndCollider) return;
        if (reachedEnd) return;
        reachedEnd = true;

        if (moveBlockRoutine != null)
        {
            StopCoroutine(moveBlockRoutine);
            moveBlockRoutine = null;
        }

        isMoving = false;
        LeaveControlOfThisBlock();

        StartCoroutine(MoveToTarget(new Vector3(0f, 0f, -3f), Quaternion.Euler(0f, 0f, 0f), 2f));
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
            Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
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