using UnityEngine;

public class LibraryButtonsManager : MonoBehaviour
{
    [SerializeField] private BoxCollider[] buttonsColliders;
    [SerializeField] private Animator animator;

    private int[] requiredPresses = { 2, 4, 3, 1 };
    private int currentButtonIndex = 0;
    private int currentPressCount = 0;

    public void PressButton(int buttonIndex)
    {
        if (buttonIndex != currentButtonIndex)
        {
            ResetSequence();
            return;
        }

        currentPressCount++;

        if (currentPressCount > requiredPresses[currentButtonIndex])
        {
            ResetSequence();
            return;
        }

        if (currentPressCount == requiredPresses[currentButtonIndex])
        {
            currentButtonIndex++;
            currentPressCount = 0;

            if (currentButtonIndex >= requiredPresses.Length)
            {
                PuzzleWin();
                ResetSequence();
            }
        }
    }

    private void ResetSequence()
    {
        currentButtonIndex = 0;
        currentPressCount = 0;
    }

    public void PuzzleWin()
    {
        foreach (var button in buttonsColliders)
        {
            button.enabled = false;
        }
        animator.SetTrigger("Open");
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }
}