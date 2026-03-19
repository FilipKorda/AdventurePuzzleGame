using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    public void AnimationFinished()
    {
        playerBehaviour.isAnimating = false;
    }
}
