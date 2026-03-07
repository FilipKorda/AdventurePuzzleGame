using UnityEngine;
using UnityEngine.InputSystem;

public class GearLockMode : MonoBehaviour
{
    [SerializeField] private InputActionReference gearModeInput;
    [SerializeField] private BoxCollider doorBoxCollider;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private BlurController blurController;
    [SerializeField] private GameObject[] gears;

    private bool riddleIsSolved = false;

    private void OnEnable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.Enable();
            gearModeInput.action.performed += OnGearModePerformed;
        }
    }


    private void OnDisable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.performed -= OnGearModePerformed;
            gearModeInput.action.Disable();
        }
    }

    private void OnGearModePerformed(InputAction.CallbackContext context)
    {
        ExitGearLockMode();
    }

    public void EnterGearLockMode()
    {
        blurController.ToggleBlurEffect();
        gameObject.SetActive(true);
        doorBoxCollider.enabled = false;
        playerBehaviour.disableOnlyMovement = true;
        UIManager.Instance.EnableGearModePanel();
    }

    public void ExitGearLockMode()
    {
        if (!riddleIsSolved)
        {
            doorBoxCollider.enabled = true;      
        }

        blurController.ToggleBlurEffect();
        ResetGears();
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableGearModePanel();
        Debug.Log("Wyszedłeś z trybu blokady zębatek!");
    }

    private void ResetGears()
    {
        foreach (GameObject gear in gears)
        {
            gear.transform.localRotation = Quaternion.identity;
        }

    }

    public void PuzzleSolved()
    {
        riddleIsSolved = true;

        ExitGearLockMode();
    }

}
