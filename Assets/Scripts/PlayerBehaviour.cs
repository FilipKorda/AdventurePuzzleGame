using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

[RequireComponent(typeof(CharacterController))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sensitivity = 10f;
    private float gravity = -9.81f;

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

    private CharacterController characterController;
    private Vector2 inputMovement;
    private Vector2 inputLook;
    private Vector3 velocity;

    private float cameraVerticalRotation = 0f;
    private Inventory playerInventory;

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
        if (lastIReadable != null && lastIReadable.IsReading()) return;
        inputMovement = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (lastIReadable != null && lastIReadable.IsReading()) return;
        inputLook = context.ReadValue<Vector2>();
    }

    //podczepione w inspektorze Playera
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

            lastIPressable?.OnPress();
            lastIPlaceable?.PlaceObject();
         
        }
    }

    private void Update()
    {
        HandleRaycast();
        HandleMovement();
        HandleLook();
        ApplyGravity();
    }

    private void HandleMovement()
    {
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

        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent<InteractableItem>(out var interactableObject))
            {
                // Rozró¿niamy typ na podstawie wartoœci interactableType
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
