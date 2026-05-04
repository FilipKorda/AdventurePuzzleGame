using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public void AnimationFinished()
    {
        PlayerControlManager.Instance.ActiveIsAnimating();
    }
}
