using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurController : MonoBehaviour
{
    public Volume blurVolume;
    private DepthOfField dof;

    private bool toggleDof;
    public float focusDistance;


    public void ToggleBlurEffect()
    {
        toggleDof = !toggleDof;
        if (toggleDof)
        {
            if (blurVolume.profile.TryGet<DepthOfField>(out dof))
            {
                dof.active = true;
                dof.focusDistance.value = focusDistance;
            }
        }
        else
        {
            if (dof != null)
            {
                dof.active = false;
            }
        }
    }
}
