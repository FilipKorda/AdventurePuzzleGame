using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class ControlPanelsManager : MonoBehaviour
{
    [SerializeField] private GameObject keyboardControlPanels;
    [SerializeField] private GameObject controllerControlPanels;

    [SerializeField] private bool defaultToKeyboard = true;

    private bool usingController;

    private void OnEnable()
    {
        usingController = DetectCurrentDevice();
        ApplyPanels();

        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private bool DetectCurrentDevice()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.0001f)
                return true;

            if (Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.0001f)
                return true;

            if (Gamepad.current.dpad.ReadValue().sqrMagnitude > 0.0001f)
                return true;

            if (Gamepad.current.buttonSouth.isPressed ||
                Gamepad.current.buttonNorth.isPressed ||
                Gamepad.current.buttonEast.isPressed ||
                Gamepad.current.buttonWest.isPressed ||
                Gamepad.current.leftShoulder.isPressed ||
                Gamepad.current.rightShoulder.isPressed ||
                Gamepad.current.leftTrigger.ReadValue() > 0.1f ||
                Gamepad.current.rightTrigger.ReadValue() > 0.1f)
            {
                return true;
            }
        }

        if (Keyboard.current != null)
        {
            if (Keyboard.current.anyKey.isPressed)
                return false;
        }

        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.isPressed ||
                Mouse.current.rightButton.isPressed ||
                Mouse.current.middleButton.isPressed)
            {
                return false;
            }

            if (Mouse.current.delta.ReadValue().sqrMagnitude > 0.0001f)
                return false;
        }

        return !defaultToKeyboard;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            return;

        if (device is Gamepad)
        {
            if (HasMeaningfulControllerInput(eventPtr, device))
            {
                SetUsingController(true);
            }

            return;
        }

        if (device is Keyboard || device is Mouse)
        {
            if (HasMeaningfulKeyboardMouseInput(eventPtr, device))
            {
                SetUsingController(false);
            }
        }
    }

    private bool HasMeaningfulControllerInput(InputEventPtr eventPtr, InputDevice device)
    {
        foreach (InputControl control in eventPtr.EnumerateChangedControls(device))
        {
            if (control is StickControl stickControl && stickControl.ReadValueFromEvent(eventPtr).sqrMagnitude > 0.0001f)
                return true;

            if (control is DpadControl dpadControl && dpadControl.ReadValueFromEvent(eventPtr).sqrMagnitude > 0.0001f)
                return true;

            if (control is ButtonControl buttonControl && buttonControl.ReadValueFromEvent(eventPtr) > 0.1f)
                return true;

            if (control is AxisControl axisControl && Mathf.Abs(axisControl.ReadValueFromEvent(eventPtr)) > 0.1f)
                return true;
        }

        return false;
    }

    private bool HasMeaningfulKeyboardMouseInput(InputEventPtr eventPtr, InputDevice device)
    {
        foreach (InputControl control in eventPtr.EnumerateChangedControls(device))
        {
            if (control is DeltaControl deltaControl && deltaControl.ReadValueFromEvent(eventPtr).sqrMagnitude > 0.0001f)
                return true;

            if (control is Vector2Control vector2Control && vector2Control.ReadValueFromEvent(eventPtr).sqrMagnitude > 0.0001f)
                return true;

            if (control is ButtonControl buttonControl && buttonControl.ReadValueFromEvent(eventPtr) > 0.1f)
                return true;

            if (control is KeyControl keyControl && keyControl.ReadValueFromEvent(eventPtr) > 0.1f)
                return true;
        }

        return false;
    }

    private void SetUsingController(bool value)
    {
        if (usingController == value)
            return;

        usingController = value;
        ApplyPanels();
    }

    private void ApplyPanels()
    {
        if (keyboardControlPanels != null)
            keyboardControlPanels.SetActive(!usingController);

        if (controllerControlPanels != null)
            controllerControlPanels.SetActive(usingController);
    }
}
