using UnityEngine;

public class GlassVisionManager : MonoBehaviour
{
    private void OnDisable()
    {
        Shader.SetGlobalFloat("_SpiritVision", 0f);
    }
}
