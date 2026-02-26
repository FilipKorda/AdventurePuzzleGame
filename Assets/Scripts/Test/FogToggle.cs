using UnityEngine;

public class FogToggle : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            RenderSettings.fog = !RenderSettings.fog;
    }
}