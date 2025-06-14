using UnityEngine;

public class LockPickPanel : MonoBehaviour
{
    public void ShowLockPickPanel()
    {
        StartLockPickMode();
    }

    public void HideLockPickPanel()
    {
        StopLockPickMode();
    }


    private void StartLockPickMode()
    {
        gameObject.SetActive(true);
    }

    private void StopLockPickMode()
    {
        gameObject.SetActive(false);
    }
}
