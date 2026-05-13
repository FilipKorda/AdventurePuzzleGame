using UnityEngine;

public static class BonusUnlockProgress
{
    private const string HasPlayedGameKey = "HasPlayedGame";

    public static void MarkGameAsPlayed()
    {
        PlayerPrefs.SetInt(HasPlayedGameKey, 1);
        PlayerPrefs.Save();
    }

    public static bool HasPlayedGame()
    {
        return PlayerPrefs.GetInt(HasPlayedGameKey, 0) == 1;
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(HasPlayedGameKey);
        PlayerPrefs.Save();
    }
}
