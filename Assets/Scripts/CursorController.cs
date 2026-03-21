using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private InputActionReference cursorInput;
    private Vector2 mouseDelta;

    [Header("Center Of Screen Dot")]
    [SerializeField] private GameObject centerOfScreen;
    [SerializeField] private RectTransform centerOfScreenImage;
    [SerializeField] private RectTransform dot;

    [Header("Raycast")]
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private LayerMask interactableLayer;
    private RaycastHit currentHit;
    private bool hasHit;

    private Camera currentCamera;

    private Vector2 centerOfScreenTargetSize = new(10f, 10f);
    private float centerOfScreenScaleSpeed = 5f;

    private bool cursorEnabled;

    private Transform selectedObject;
    private Vector3 offset;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        centerOfScreenImage.gameObject.SetActive(false);
    }

    public void EnableCursor(Camera cam)
    {
        currentCamera = cam;
        cursorEnabled = true;

        OnEnableInput();
        centerOfScreen.SetActive(false);
        centerOfScreenImage.gameObject.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void DisableCursor()
    {
        cursorEnabled = false;
        currentCamera = null;

        OnDisableInput();
        centerOfScreen.SetActive(true);
        centerOfScreenImage.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region Click And Drag Input Region

    private void OnEnableInput()
    {
        cursorInput.action.Enable();
        cursorInput.action.started += OnInputStarted;
        cursorInput.action.performed += OnInputPerformed;
        cursorInput.action.canceled += OnInputCanceled;
    }

    private void OnDisableInput()
    {
        cursorInput.action.started -= OnInputStarted;
        cursorInput.action.performed -= OnInputPerformed;
        cursorInput.action.canceled -= OnInputCanceled;
        cursorInput.action.Disable();
    }

    private void OnInputStarted(InputAction.CallbackContext context)
    {
        if (!cursorEnabled) return;

        if (context.action.activeControl.path.Contains("leftButton"))
        {
            if (hasHit)
            {
                selectedObject = currentHit.transform;
                offset = selectedObject.position - currentHit.point;
            }
        }
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (!cursorEnabled) return;

        if (context.action.activeControl.path.Contains("delta"))
        {
            mouseDelta = context.ReadValue<Vector2>();

            if (selectedObject != null && Mouse.current.leftButton.isPressed)
            {
                Plane movePlane = new Plane(Vector3.up, new Vector3(0, selectedObject.position.y, 0));
                Ray ray = currentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (movePlane.Raycast(ray, out float distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    selectedObject.position = new Vector3(hitPoint.x + offset.x, selectedObject.position.y, hitPoint.z + offset.z);
                }
            }
        }

        if (context.action.activeControl.path.Contains("leftButton") && !Mouse.current.leftButton.isPressed)
        {
            selectedObject = null;
        }
    }

    private void OnInputCanceled(InputAction.CallbackContext context)
    {
        if (!cursorEnabled) return;

        if (context.action.activeControl.path.Contains("leftButton"))
        {
            selectedObject = null;
        }
    }

    #endregion

    #region Raycast
    void Update()
    {
        if (!cursorEnabled || currentCamera == null) return;

        HandleRaycast();
        centerOfScreenImage.position = Input.mousePosition;
        LerpCenterOfScreenSize();
    }

    private void HandleRaycast()
    {
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        hasHit = Physics.Raycast(ray, out currentHit, rayDistance, interactableLayer);

        UpdateDotVisibility(!hasHit);
        centerOfScreenTargetSize = hasHit ? new Vector2(20f, 20f) : new Vector2(10f, 10f);
    }

    private void UpdateDotVisibility(bool isVisible)
    {
        dot.gameObject.SetActive(isVisible);
    }

    private void LerpCenterOfScreenSize()
    {
        centerOfScreenImage.sizeDelta = Vector2.Lerp(centerOfScreenImage.sizeDelta, centerOfScreenTargetSize, centerOfScreenScaleSpeed * Time.deltaTime);
    }
    #endregion
}