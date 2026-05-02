using UnityEngine;

public class Ailments : MonoBehaviour
{
    public static Ailments Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyWaterBucketEffect()
    {
        PlayerControlManager.Instance.ApplyWaterBucketEffect();
    }

    public void ApplyAcidBucketEffect()
    {
        PlayerControlManager.Instance.ApplyAcidBucketEffect();
    }

    public void ApplyBloodBucketEffect()
    {
        PlayerControlManager.Instance.ApplyBloodBucketEffect();
    }

    public void ApplyWineBucketEffect()
    {
        PlayerControlManager.Instance.ApplyWineBucketEffect();
    }

    public void ApplyRawMeatEffect()
    {
        PlayerControlManager.Instance.ApplyRawMeatEffect();
    }

    public void ApplyNiceWaterEffect()
    {
        PlayerControlManager.Instance.ApplyNiceWaterEffect();
    }

    public void ApplyMudWaterEffect()
    {
        PlayerControlManager.Instance.ApplyMudWaterEffect();
    }

    public void ApplyLeafGoodsEffect()
    {
        PlayerControlManager.Instance.ApplyLeafGoodsEffect();
    }

    public void ApplyAngryTimeEffect()
    {
        PlayerControlManager.Instance.ApplyAngryTimeEffect();
    }

    public void ApplyBadMoodEffect()
    {
        PlayerControlManager.Instance.ApplyBadMoodEffect();
    }

    public void ApplyGoodSoupEffect()
    {
        PlayerControlManager.Instance.ApplyGoodSoupEffect();
    }

    public void ApplyHolyCowEffect()
    {
        PlayerControlManager.Instance.ApplyHolyCowEffect();
    }
}