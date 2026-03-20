using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance { get; private set; }

    [Header("Center Of Screen Dot")]
    [SerializeField] private GameObject centerOfScreen;
    [SerializeField] private RectTransform centerOfScreenImage;
    [SerializeField] private RectTransform dot;
    [Header("Raycast")]
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private LayerMask interactableLayer;

    private Camera currentCamera;

    private Vector2 centerOfScreenTargetSize = new(10f, 10f);
    private float centerOfScreenScaleSpeed = 5f;

    private bool cursorEnabled;

    private IMovingBraiserCircle lastIMovingBraiserCircle;

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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && cursorEnabled)
        {
            lastIMovingBraiserCircle?.InteractWithRing();
        }
    }

    void Update()
    {
        if (!cursorEnabled || currentCamera == null) return;

        HandleRaycast();
        centerOfScreenImage.position = Input.mousePosition;
        LerpCenterOfScreenSize();
    }

    public void EnableCursor(Camera cam)
    {
        currentCamera = cam;
        cursorEnabled = true;

        centerOfScreen.SetActive(false);
        centerOfScreenImage.gameObject.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void DisableCursor()
    {
        cursorEnabled = false;
        currentCamera = null;

        centerOfScreen.SetActive(true);
        centerOfScreenImage.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HandleRaycast()
    {
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

        lastIMovingBraiserCircle = null;

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
        {
            if (hit.collider.TryGetComponent<InteractableItem>(out var interactableObject))
            {
                lastIMovingBraiserCircle = interactableObject;
            }



            UpdateDotVisibility(false);
            centerOfScreenTargetSize = new Vector2(20f, 20f);

        }
        else
        {
            UpdateDotVisibility(true);
            centerOfScreenTargetSize = new Vector2(10f, 10f);
        }
    }


    private void UpdateDotVisibility(bool isVisible)
    {
        dot.gameObject.SetActive(isVisible);
    }

    private void LerpCenterOfScreenSize()
    {
        centerOfScreenImage.sizeDelta = Vector2.Lerp(centerOfScreenImage.sizeDelta, centerOfScreenTargetSize, centerOfScreenScaleSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (currentCamera == null) return;

        Ray ray = currentCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * rayDistance);
    }
}