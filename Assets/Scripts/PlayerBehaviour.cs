using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sensitivity = 10f;
    private float gravity = -9.81f;
    [SerializeField] private float climbSpeed = 3f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float minLookAngle = -60f;
    [SerializeField] private float maxLookAngle = 60f;

    [Header("Raycast Detector")]
    [SerializeField] private float raycastRange = 5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Center Of Screen Dot")]
    [SerializeField] private GameObject centerOfScreen;
    [SerializeField] private GameObject dot;
    private Vector2 centerOfScreenTargetSize = new(10f, 10f);
    private float centerOfScreenScaleSpeed = 5f;

    private IPickupable lastIpickupable;
    private IBookThrowable lastIBookThrowable;
    private IOpenable lastIOpenable;
    private IReadable lastIReadable;
    private IPressable lastIPressable;
    private IPlaceable lastIPlaceable;
    private ILockPick lastILockPick;
    private IFillable lastIFillable;
    private IPickupARenewableItem lastIPickupARenewableItem;

    private CharacterController characterController;
    private Vector2 inputMovement;
    private Vector2 inputLook;
    private Vector3 velocity;

    private float cameraVerticalRotation = 0f;
    private Inventory playerInventory;
    private bool isClimbing = false; 

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned!");
        }
    }

    private void Start()
    {
        if (!TryGetComponent(out playerInventory))
        {
            playerInventory = gameObject.AddComponent<Inventory>();
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (lastILockPick != null && lastILockPick.IsLockPicking()) return;
        if (lastIReadable != null && lastIReadable.IsReading()) return;
        inputMovement = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (lastILockPick != null && lastILockPick.IsLockPicking()) return;
        if (lastIReadable != null && lastIReadable.IsReading()) return;
        inputLook = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lastIpickupable?.OnPickUp();
            lastIBookThrowable?.OnBookThrow();

            if (lastIOpenable != null && lastIOpenable.IsOpen())
            {
                lastIOpenable?.CloseObject();
            }
            else
            {
                lastIOpenable?.OpenObject();
            }

            if (lastIReadable != null && lastIReadable.IsReading())
            {
                lastIReadable?.OnStopRead();
            }
            else
            {
                lastIReadable?.OnRead();
            }

            if (lastILockPick != null && lastILockPick.IsLockPicking())
            {
                lastILockPick?.StopLockPicking();
            }
            else
            {
                lastILockPick?.StartLockPick();
            }

            lastIPressable?.OnPress();
            lastIPlaceable?.PlaceObject();
            lastIFillable?.OnFill();
            lastIPickupARenewableItem?.OnPickupARenewableItem();
        }
    }

    private void Update()
    {
        HandleRaycast();
        HandleMovement();
        HandleLook();
        ApplyGravity();
        HandleClimbing(); 
    }

    private void HandleMovement()
    {
        if (isClimbing) { return; } 
        Vector3 moveDirection = transform.right * inputMovement.x + transform.forward * inputMovement.y;
        characterController.Move(moveSpeed * Time.deltaTime * moveDirection);
    }

    private void HandleLook()
    {
        float mouseX = inputLook.x * sensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = inputLook.y * sensitivity * Time.deltaTime;
        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, minLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }

    private void ApplyGravity()
    {
        if (isClimbing) { velocity.y = 0; return; } 

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleRaycast()
    {
        Ray ray = new(cameraTransform.position, cameraTransform.forward);

        // Resetowanie referencji
        lastIpickupable = null;
        lastIBookThrowable = null;
        lastIOpenable = null;
        lastIReadable = null;
        lastIPressable = null;
        lastIPlaceable = null;
        lastILockPick = null;
        lastIFillable = null;
        lastIPickupARenewableItem = null;

        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent<InteractableItem>(out var interactableObject))
            {
                switch (interactableObject.interactableType)
                {
                    case InteractableItem.InteractableType.Pickupable:
                        lastIpickupable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Throwable:
                        lastIBookThrowable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Openable:
                        lastIOpenable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Readable:
                        lastIReadable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Pressable:
                        lastIPressable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Placeable:
                        lastIPlaceable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.LockPick:
                        lastILockPick = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Fillable:
                        lastIFillable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.PickupARenewableItem:
                        lastIPickupARenewableItem = interactableObject;
                        break;
                }
            }

            UpdateDotVisibility(false);
            centerOfScreenTargetSize = new Vector2(20f, 20f);
        }
        else
        {
            UpdateDotVisibility(true);
            centerOfScreenTargetSize = new Vector2(10f, 10f);
        }

        LerpCenterOfScreenSize();
    }

    private void HandleClimbing()
    {
        if (!isClimbing) { return; }

        if (characterController.isGrounded && inputMovement.y < -0.1f)
        {
            isClimbing = false;
            return; 
        }
       

        Vector3 climbDirection = new Vector3(0, inputMovement.y, 0);
        characterController.Move(climbSpeed * Time.deltaTime * climbDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }

    private void UpdateDotVisibility(bool isVisible)
    {
        dot.SetActive(isVisible);
    }

    private void LerpCenterOfScreenSize()
    {
        RectTransform rectTransform = centerOfScreen.GetComponent<RectTransform>();
        rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, centerOfScreenTargetSize, centerOfScreenScaleSpeed * Time.deltaTime);
    }
}