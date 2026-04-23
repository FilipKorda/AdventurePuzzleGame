using UnityEngine;
using UnityEngine.UI;

public class GraphicSettings : MonoBehaviour
{
    private Vector2Int resolution1 = new Vector2Int(1280, 720);
    private Vector2Int resolution2 = new Vector2Int(1920, 1080);
    private Vector2Int resolution3 = new Vector2Int(2560, 1440);

    private const string PrefWidthKey = "Graphics_Width";
    private const string PrefHeightKey = "Graphics_Height";
    private const string PrefFullscreenKey = "Graphics_Fullscreen"; 

    private void Start()
    {
       
        int width = PlayerPrefs.GetInt(PrefWidthKey, resolution3.x);
        int height = PlayerPrefs.GetInt(PrefHeightKey, resolution3.y);
        bool isFullscreen = PlayerPrefs.GetInt(PrefFullscreenKey, 1) == 1;

        Screen.SetResolution(width, height, isFullscreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        PlayerPrefs.SetInt(PrefFullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetResolution1()
    {
        ApplyAndSaveResolution(resolution1.x, resolution1.y);
    }

    public void SetResolution2()
    {
        ApplyAndSaveResolution(resolution2.x, resolution2.y);
    }

    public void SetResolution3()
    {
        ApplyAndSaveResolution(resolution3.x, resolution3.y);
    }

    private void ApplyAndSaveResolution(int width, int height)
    {
        bool currentFullscreen = Screen.fullScreen;

        Screen.SetResolution(width, height, currentFullscreen);

        PlayerPrefs.SetInt(PrefWidthKey, width);
        PlayerPrefs.SetInt(PrefHeightKey, height);
        PlayerPrefs.Save();
    }
}
