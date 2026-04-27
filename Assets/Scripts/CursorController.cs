using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance { get; private set; }

    [SerializeField] private InputActionReference cursorDragInput;
    [SerializeField] private InputActionReference cursorClickButtonInput;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private PausePanel pausePanel;

    [SerializeField] private float gamepadCursorSpeed = 1200f;
    [SerializeField] private float gamepadDeadzone = 0.15f;

    public LayerMask InteractableLayer => interactableLayer;
    public Camera CurrentCamera { get; private set; }

    public ICursorGameMode currentMode;
    private bool cursorEnabled;

    [SerializeField] private GameObject centerOfScreen;
    [SerializeField] private RectTransform centerOfScreenImage;
    [SerializeField] private RectTransform dot;

    private float rayDistance = 2f;
    private Vector2 centerOfScreenTargetSize = new(10f, 10f);
    private float centerOfScreenScaleSpeed = 5f;

    public Ray CurrentRay { get; private set; }
    public RaycastHit CurrentHit { get; private set; }
    public bool HasHit { get; set; }

    private Vector2 virtualCursorPosition;
    private Vector2 lastMousePosition;
    private bool usingGamepadCursor;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        centerOfScreenImage.gameObject.SetActive(false);

        Vector2 startPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        virtualCursorPosition = startPos;
        lastMousePosition = Mouse.current != null ? Mouse.current.position.ReadValue() : startPos;
    }

    void Update()
    {
        if (!cursorEnabled) return;
        if (CurrentCamera == null) return;

        UpdateVirtualCursorPosition();
        UpdateRay();
        currentMode?.Tick();

        centerOfScreenImage.position = virtualCursorPosition;
        LerpCenterOfScreenSize();
    }

    private void UpdateVirtualCursorPosition()
    {
        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            if ((mousePos - lastMousePosition).sqrMagnitude > 0.0001f)
            {
                virtualCursorPosition = mousePos;
                usingGamepadCursor = false;
            }

            lastMousePosition = mousePos;
        }

        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue();

            if (stick.sqrMagnitude > gamepadDeadzone * gamepadDeadzone)
            {
                usingGamepadCursor = true;
                virtualCursorPosition += new Vector2(
                    stick.x * gamepadCursorSpeed * Time.deltaTime,
                    stick.y * gamepadCursorSpeed * Time.deltaTime
                );
            }
        }

        virtualCursorPosition.x = Mathf.Clamp(virtualCursorPosition.x, 0f, Screen.width);
        virtualCursorPosition.y = Mathf.Clamp(virtualCursorPosition.y, 0f, Screen.height);
    }

    private void UpdateRay()
    {
        CurrentRay = CurrentCamera.ScreenPointToRay(virtualCursorPosition);
        HasHit = Physics.Raycast(CurrentRay, out RaycastHit hit, rayDistance, interactableLayer);

        CurrentHit = hit;

        dot.gameObject.SetActive(!HasHit);
        centerOfScreenTargetSize = HasHit ? new Vector2(20f, 20f) : new Vector2(10f, 10f);
    }

    private void LerpCenterOfScreenSize()
    {
        centerOfScreenImage.sizeDelta =
            Vector2.Lerp(centerOfScreenImage.sizeDelta, centerOfScreenTargetSize, centerOfScreenScaleSpeed * Time.deltaTime);
    }

    public Ray GetRayFromScreenPoint(Vector2 screenPos)
    {
        return CurrentCamera.ScreenPointToRay(screenPos);
    }

    public Vector2 GetCursorScreenPosition()
    {
        if (usingGamepadCursor)
            return virtualCursorPosition;

        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        return virtualCursorPosition;
    }


    public void EnableCursor(Camera cam)
    {
        CurrentCamera = cam;
        cursorEnabled = true;

        virtualCursorPosition = Mouse.current != null
            ? Mouse.current.position.ReadValue()
            : new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        cursorClickButtonInput.action.Enable();
        cursorClickButtonInput.action.started += OnClickButtonInput;
        cursorClickButtonInput.action.performed += OnClickButtonInput;
        cursorClickButtonInput.action.canceled += OnClickButtonInput;

        cursorDragInput.action.Enable();
        cursorDragInput.action.started += OnDragInputStarted;
        cursorDragInput.action.canceled += OnDragInputCanceled;

        centerOfScreen.SetActive(false);
        centerOfScreenImage.gameObject.SetActive(true);
        pausePanel.SetAllowPause(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void EnableBraiserCursor(Camera cam)
    {
        CurrentCamera = cam;
        cursorEnabled = true;

        virtualCursorPosition = Mouse.current != null
            ? Mouse.current.position.ReadValue()
            : new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        cursorClickButtonInput.action.Enable();
        cursorClickButtonInput.action.started += OnClickButtonInput;
        cursorClickButtonInput.action.performed += OnClickButtonInput;
        cursorClickButtonInput.action.canceled += OnClickButtonInput;

        cursorDragInput.action.Enable();
        cursorDragInput.action.started += OnDragInputStarted;
        cursorDragInput.action.canceled += OnDragInputCanceled;

        centerOfScreen.SetActive(false);
        centerOfScreenImage.gameObject.SetActive(true);
        pausePanel.SetAllowPause(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void DisableCursor()
    {
        cursorEnabled = false;

        currentMode?.Exit();
        currentMode = null;

        cursorClickButtonInput.action.started -= OnClickButtonInput;
        cursorClickButtonInput.action.performed -= OnClickButtonInput;
        cursorClickButtonInput.action.canceled -= OnClickButtonInput;
        cursorClickButtonInput.action.Disable();


        cursorDragInput.action.started -= OnDragInputStarted;
        cursorDragInput.action.canceled -= OnDragInputCanceled;
        cursorDragInput.action.Disable();

        CurrentCamera = null;

        centerOfScreen.SetActive(true);
        centerOfScreenImage.gameObject.SetActive(false);
        pausePanel.SetAllowPause(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        HasHit = false;
    }

    public void DisableBraiserCursor()
    {
        cursorEnabled = false;

        currentMode?.Exit();
        currentMode = null;

        cursorClickButtonInput.action.started -= OnClickButtonInput;
        cursorClickButtonInput.action.performed -= OnClickButtonInput;
        cursorClickButtonInput.action.canceled -= OnClickButtonInput;
        cursorClickButtonInput.action.Disable();


        cursorDragInput.action.started -= OnDragInputStarted;
        cursorDragInput.action.canceled -= OnDragInputCanceled;
        cursorDragInput.action.Disable();

        CurrentCamera = null;

        centerOfScreen.SetActive(true);
        centerOfScreenImage.gameObject.SetActive(false);
        pausePanel.SetAllowPause(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        HasHit = false;
    }

    public void SetGameMode(ICursorGameMode mode)
    {
        currentMode = mode;
        currentMode?.Enter(this);
    }

    private void OnDragInputStarted(InputAction.CallbackContext context)
    {
        currentMode?.OnDragInputStarted(context);
    }

    private void OnDragInputCanceled(InputAction.CallbackContext context)
    {
        currentMode?.OnDragInputCanceled(context);
    }

    private void OnClickButtonInput(InputAction.CallbackContext context)
    {
        currentMode?.OnClickInput(context);
    }
}
