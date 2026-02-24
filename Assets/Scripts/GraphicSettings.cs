using UnityEngine;

public class GraphicSettings : MonoBehaviour
{
    private Vector2Int resolution1 = new Vector2Int(1280, 720);
    private Vector2Int resolution2 = new Vector2Int(1920, 1080);
    private Vector2Int resolution3 = new Vector2Int(2560, 1440);

    private const string PrefWidthKey = "Graphics_Width";
    private const string PrefHeightKey = "Graphics_Height";
    private const string PrefFullscreenKey = "Graphics_Fullscreen";

    private bool currentFullscreen;

    private void Start()
    {
        int width = PlayerPrefs.GetInt(PrefWidthKey, resolution3.x);
        int height = PlayerPrefs.GetInt(PrefHeightKey, resolution3.y);
        currentFullscreen = PlayerPrefs.GetInt(PrefFullscreenKey, 1) == 1;

        ApplyResolution(width, height, currentFullscreen);
    }

    public void SetResolution1()
    {
        ApplyAndSave(resolution1.x, resolution1.y, currentFullscreen);
    }

    public void SetResolution2()
    {
        ApplyAndSave(resolution2.x, resolution2.y, currentFullscreen);
    }

    public void SetResolution3()
    {
        ApplyAndSave(resolution3.x, resolution3.y, currentFullscreen);
    }

    public void SetFullscreen(bool fullscreen)
    {
        currentFullscreen = fullscreen;
        ApplyAndSave(Screen.width, Screen.height, currentFullscreen);
    }

    private void ApplyAndSave(int width, int height, bool fullscreen)
    {
        ApplyResolution(width, height, fullscreen);
        SaveResolution(width, height, fullscreen);
    }

    private void ApplyResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
    }

    private void SaveResolution(int width, int height, bool fullscreen)
    {
        PlayerPrefs.SetInt(PrefWidthKey, width);
        PlayerPrefs.SetInt(PrefHeightKey, height);
        PlayerPrefs.SetInt(PrefFullscreenKey, fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
