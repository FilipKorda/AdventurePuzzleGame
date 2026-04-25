using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance { get; private set; }

    [SerializeField] private InputActionReference cursorDragInput;
    [SerializeField] private InputActionReference cursorClickButtonInput;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private PausePanel pausePanel;

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
    }

    void Update()
    {
        if (!cursorEnabled) return;

        UpdateRay();
        currentMode?.Tick();

        centerOfScreenImage.position = Mouse.current.position.ReadValue();
        LerpCenterOfScreenSize();
    }

    private void UpdateRay()
    {
        CurrentRay = CurrentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
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

    public void EnableCursor(Camera cam)
    {
        CurrentCamera = cam;

        cursorEnabled = true;

        cursorClickButtonInput.action.Enable();
        cursorClickButtonInput.action.started += OnClickButtonInput;

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

        cursorClickButtonInput.action.Enable();
        cursorClickButtonInput.action.started += OnClickButtonInput;

        cursorDragInput.action.Enable();
        cursorDragInput.action.performed += OnDragInputStarted;
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
        cursorClickButtonInput.action.Disable();

        cursorDragInput.action.performed -= OnDragInputStarted;
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