using System.Collections;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    public static PlayerControlManager Instance { get; private set; }

    [SerializeField] private PlayerBehaviour playerBehaviour;

    private int lockCount;
    private int movementLockCount;


    private void Awake()
    {
        Instance = this;
    }

    public void LockPlayer()
    {
        lockCount++;
        ApplyLockCountState();
    }

    public void UnlockPlayer()
    {
        lockCount = Mathf.Max(0, lockCount - 1);
        ApplyLockCountState();
    }

    public void LockMovementOnly()
    {
        movementLockCount++;
        ApplyMovementLockCountState();
    }

    public void UnlockMovementOnly()
    {
        movementLockCount = Mathf.Max(0, movementLockCount - 1);
        ApplyMovementLockCountState();
    }

    private void ApplyLockCountState()
    {
        if (playerBehaviour == null)
            return;

        playerBehaviour.disablePlayer = lockCount > 0;
    }

    private void ApplyMovementLockCountState()
    {
        if (playerBehaviour == null)
            return;

        playerBehaviour.disableOnlyMovement = movementLockCount > 0;
    }


    public void ActiveIsAnimating()
    {
        playerBehaviour.isAnimating = true;
    }

    public void EnableCamera()
    {
        playerBehaviour._playerCamera.enabled = true;
    }

    public Camera GetPlayerCamera()
    {
        if (playerBehaviour == null)
            return null;

        return playerBehaviour._playerCamera;
    }

    public void SetGamepadSens(float sens)
    {
        playerBehaviour.gamepadSensitivity = sens;
    }

    public void TogglePlayer(bool toggle)
    {
        playerBehaviour._playerCamera.enabled = toggle;
        playerBehaviour.GetComponent<MeshRenderer>().enabled = toggle;
        playerBehaviour.GetComponent<CapsuleCollider>().enabled = toggle;
        playerBehaviour.GetComponent<CharacterController>().enabled = toggle;
    }

    public void DisableLampOnTrigger()
    {
        playerBehaviour.DisableLampOnTrigger();
    }

    public void SetCharacterControllerEnabled(bool enabled)
    {
        if (playerBehaviour == null)
            return;

        if (playerBehaviour.TryGetComponent<CharacterController>(out var controller))
            controller.enabled = enabled;
    }


    public void ResetCamera()
    {
        playerBehaviour.ResetCameraRotation();
    }

    public void DisablePlayerLookTemporarily()
    {
        StartCoroutine(DisablePlayerLook());
    }

    private IEnumerator DisablePlayerLook()
    {
        yield return null;

        playerBehaviour.disablePlayer = true;

        playerBehaviour.ResetCameraRotation();

        yield return new WaitForSeconds(0.1f);

        playerBehaviour.disablePlayer = false;
    }

    #region AlimentsEffects
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
        playerBehaviour.NoEffect();
        // Debug.Log("Efekt z Ailments: Zjedzono dobrą zupę. Ciepło rozchodzi się po ciele.");
        // W przyszłości: np. player.RestoreHealth(15); player.RestoreHunger(25);
    }

    public void ApplyHolyCowEffect()
    {
        // Debug.Log("Efekt z Ailments: Zjedzono 'HolyCow'. Czujesz się błogosławiony.");
        playerBehaviour.OpenHiddenDoor(7);
    }

    #endregion
}
