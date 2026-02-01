using UnityEngine;

public class Ailments : MonoBehaviour
{
    public static Ailments Instance { get; private set; }
    [SerializeField] private PlayerBehaviour playerBehaviour;

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
        playerBehaviour.NoEffect();
    }

    public void ApplyAcidBucketEffect()
    {
        playerBehaviour.ApplyAcidEffect(10f);
    }

    public void ApplyBloodBucketEffect()
    {
        playerBehaviour.NoEffect();
    }

    public void ApplyWineBucketEffect()
    {
        playerBehaviour.ApplyDrunkEffect(15f);
    }

    public void ApplyRawMeatEffect()
    {
        playerBehaviour.NoEffect();
    }

    public void ApplyNiceWaterEffect()
    {
        playerBehaviour.ApplyNiceWaterEffect(10);
    }

    public void ApplyMudWaterEffect()
    {
        playerBehaviour.MudWaterEffect(10);
    }

    public void ApplyLeafGoodsEffect()
    {
        playerBehaviour.LeafGoods(10);
    }

    public void ApplyAngryTimeEffect()
    {
        playerBehaviour.ApplyAngryTime(10);
    }

    public void ApplyBadMoodEffect()
    {
        playerBehaviour.BadMoodTeleport(10);
    }

    public void ApplyGoodSoupEffect()
    {
        Debug.Log("Efekt z Ailments: Zjedzono dobr¹ zupê. Ciep³o rozchodzi siê po ciele.");
        // W przysz³oœci: np. player.RestoreHealth(15); player.RestoreHunger(25);
    }

    public void ApplyHolyCowEffect()
    {
        Debug.Log("Efekt z Ailments: Zjedzono 'HolyCow'. Czujesz siê b³ogos³awiony.");
        playerBehaviour.OpenHiddenDoor(7);
    }
}