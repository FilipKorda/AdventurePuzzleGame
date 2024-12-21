using UnityEngine;

public class SecretWallButtonsManager : MonoBehaviour
{
    [SerializeField] private InteractableItem[] wallButtons;
    [SerializeField] private Animator secretLeverGiverAnimator;

    public void ChechIfAllButtonsWasPressed()
    {
        bool allButtonsArePressed = true;

        foreach (var button in wallButtons)
        {
            if (!button.IsPressed())
            {
                allButtonsArePressed = false;
                break; 
            }
            else
            {
                Debug.Log($"Button {button.name} IsPressed: true");
            }
        }

        if (allButtonsArePressed)
        {
            Debug.Log($"All Button Are Preesed");

            if (secretLeverGiverAnimator.TryGetComponent<Animator>(out var animator))
            {
                animator.SetTrigger("Interact");
            }
            else
            {
                Debug.LogWarning("Animator not found in object secretLeverGiverAnimator.");
            }
        }
    }

}
