using UnityEngine;
using UnityEngine.InputSystem;

public class Mirror : MonoBehaviour
{
    [Header("Mirror Settings")]
    [SerializeField] private PlayerBehaviour playerBehaviour;

    [SerializeField] private Transform mirrorRoot;
    [SerializeField] private Transform mirrorPivot;
    [SerializeField] private Camera mirrorCamera;

    [SerializeField] private float sensitivity = 100f;
    private float verticalRotation;
    private float horizontalRotation;

    [Header("Input")]
    [SerializeField] private InputActionReference rotationAction;
    [SerializeField] private InputActionReference confirmAndExit;


    [SerializeField] private float horizontalLeft = -20;
    [SerializeField] private float horizontalRight = 20;

    [SerializeField] private float verticalDown = -20;
    [SerializeField] private float verticalUp = 20;





    private void Start()
    {
        DisableControl();
    }

    public void EnableControl()
    {
        mirrorCamera.enabled = true;
        enabled = true;

        playerBehaviour.disablePlayer = true;
        TogglePlayer(false);

        UIManager.Instance.EnableMirrorInputPanel();

        if (rotationAction != null)
        {
            rotationAction.action.Enable();
            rotationAction.action.performed += OnMirrorRotation;
        }

        if (confirmAndExit != null)
        {
            confirmAndExit.action.Enable();
            confirmAndExit.action.performed += OnMirorConfirmAndExit;
        }
    }

    private void TogglePlayer(bool toggle)
    {
        playerBehaviour._playerCamera.enabled = toggle;
        playerBehaviour.GetComponent<MeshRenderer>().enabled = toggle;
        playerBehaviour.GetComponent<CapsuleCollider>().enabled = toggle;
        playerBehaviour.GetComponent<CharacterController>().enabled = toggle;
    }

    public void DisableControl()
    {
        mirrorCamera.enabled = false;
        enabled = false;

        playerBehaviour.disablePlayer = false;
        TogglePlayer(true);


        UIManager.Instance.DisableSharedPanelText();

        if (rotationAction != null)
        {
            rotationAction.action.performed -= OnMirrorRotation;
            rotationAction.action.Disable();
        }

        if (confirmAndExit != null)
        {
            confirmAndExit.action.Disable();
            confirmAndExit.action.performed -= OnMirorConfirmAndExit;
        }
    }


    private void OnMirorConfirmAndExit(InputAction.CallbackContext context)
    {
        DisableControl();
    }

    private void OnMirrorRotation(InputAction.CallbackContext context)
    {
        float mouseX = rotationAction.action.ReadValue<Vector2>().x * sensitivity * Time.deltaTime;
        float mouseY = rotationAction.action.ReadValue<Vector2>().y * sensitivity * Time.deltaTime;

        horizontalRotation += mouseX;
        horizontalRotation = Mathf.Clamp(horizontalRotation, horizontalLeft, horizontalRight);
        mirrorRoot.localRotation = Quaternion.Euler(0, horizontalRotation, 0);

        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalDown, verticalUp);
        mirrorPivot.localRotation = Quaternion.Euler(0, 0, verticalRotation);
    }
}