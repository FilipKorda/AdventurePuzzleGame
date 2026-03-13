using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectObjectRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;

    private bool isRotating;

    [SerializeField] private InputActionReference inspectObjectRotation;

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (inspectObjectRotation != null)
        {
            inspectObjectRotation.action.performed += InspectObejctInput;
        }
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (inspectObjectRotation != null)
        {
            inspectObjectRotation.action.performed -= InspectObejctInput;
        }
    }


    private void InspectObejctInput(InputAction.CallbackContext context)
    {
        if (!isRotating) return;

        Vector2 delta = context.ReadValue<Vector2>();

        float mouseX = delta.x;
        float mouseY = delta.y;

        transform.Rotate(
            Vector3.up,
            -mouseX * rotationSpeed * Time.deltaTime,
            Space.World
        );

        transform.Rotate(
            Vector3.right,
            mouseY * rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }
}