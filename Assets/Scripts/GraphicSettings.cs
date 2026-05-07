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
    private const string PrefInitializedKey = "Graphics_Initialized";
    private const string PrefTargetFpsKey = "Graphics_TargetFPS";

    [SerializeField] private Toggle fps30Toggle;
    [SerializeField] private Toggle fps60Toggle;

    private void Start()
    {
        FirstInitialization();

        int width = PlayerPrefs.GetInt(PrefWidthKey, resolution2.x);
        int height = PlayerPrefs.GetInt(PrefHeightKey, resolution2.y);
        bool fullscreen = PlayerPrefs.GetInt(PrefFullscreenKey, 1) == 1;
        int targetFps = PlayerPrefs.GetInt(PrefTargetFpsKey, 30);

        Screen.SetResolution(width, height, fullscreen);
        ApplyTargetFrameRate(targetFps);
        RefreshFpsToggles(targetFps);
    }

    private void FirstInitialization()
    {
        bool isInitialized = PlayerPrefs.GetInt(PrefInitializedKey, 0) == 1;

        if (isInitialized)
            return;

        bool isFullscreen = true;

        Screen.SetResolution(resolution2.x, resolution2.y, isFullscreen);
        Application.targetFrameRate = 30;

        PlayerPrefs.SetInt(PrefWidthKey, resolution2.x);
        PlayerPrefs.SetInt(PrefHeightKey, resolution2.y);
        PlayerPrefs.SetInt(PrefFullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.SetInt(PrefTargetFpsKey, 30);
        PlayerPrefs.SetInt(PrefInitializedKey, 1);
        PlayerPrefs.Save();
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

    public void Set30Fps(bool isOn)
    {
        if (!isOn)
        {
            if (fps30Toggle != null)
                fps30Toggle.SetIsOnWithoutNotify(true);

            return;
        }

        SetTargetFps(30);
    }

    public void Set60Fps(bool isOn)
    {
        if (!isOn)
        {
            if (fps60Toggle != null)
                fps60Toggle.SetIsOnWithoutNotify(true);

            return;
        }

        SetTargetFps(60);
    }


    private void SetTargetFps(int fps)
    {
        ApplyTargetFrameRate(fps);
        PlayerPrefs.SetInt(PrefTargetFpsKey, fps);
        PlayerPrefs.Save();
        RefreshFpsToggles(fps);
    }

    private void ApplyTargetFrameRate(int fps)
    {
        Application.targetFrameRate = fps;
    }

    private void RefreshFpsToggles(int fps)
    {
        if (fps30Toggle != null)
            fps30Toggle.SetIsOnWithoutNotify(fps == 30);

        if (fps60Toggle != null)
            fps60Toggle.SetIsOnWithoutNotify(fps == 60);
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
