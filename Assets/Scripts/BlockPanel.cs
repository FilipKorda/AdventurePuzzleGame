using UnityEngine;

public class BlockPanel : MonoBehaviour
{
    [SerializeField] private InteractableItem[] interactableButton;
    [SerializeField] private int[] buttonsToCheck;
    public bool panelWin = false;
    [SerializeField] private Color baseColor;
    public void ResetBlockPanel()
    {
        foreach (var interactableButton in interactableButton)
        {
            interactableButton.ResetColorblockInstant(baseColor);
        }

    }

    public void CheckIfAllButtonsArePreesed()
    {
        for (int i = 0; i < interactableButton.Length; i++)
        {
            bool shouldBePressed = false;

            foreach (int index in buttonsToCheck)
            {
                if (i == index)
                {
                    shouldBePressed = true;
                    break;
                }
            }

            if (interactableButton[i].buttonIsPressed != shouldBePressed)
            {
                return;
            }
        }

        panelWin = true;
    }


}