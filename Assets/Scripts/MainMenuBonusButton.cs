using UnityEngine;

public class MainMenuBonusButton : MonoBehaviour
{
    [SerializeField] private GameObject bonusButton;

    private void Start()
    {
        if (bonusButton == null)
            return;

        bonusButton.SetActive(BonusUnlockProgress.HasPlayedGame());
    }


    [ContextMenu("Reset Progress")]
    private void ResetProgress()
    {
        BonusUnlockProgress.ResetProgress();
        if (bonusButton != null)
        {
            bonusButton.SetActive(false);
        }
    }
}
