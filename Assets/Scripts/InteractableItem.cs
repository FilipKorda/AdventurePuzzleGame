using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class InteractableItem : MonoBehaviour, IPickupable, IBookThrowable, IOpenable, IReadable, IPressable, IPlaceable, ILockPick
{
    public enum InteractableType
    {
        Pickupable,
        Throwable,
        Openable,
        Readable,
        Pressable,
        Placeable,
        LockPick
    }

    public InteractableType interactableType;
    [SerializeField] private string itemName = "Przedmiot";
    [SerializeField] private Sprite itemSprite;

    [Header("ID (if Pickupable)")]
    [Tooltip("ID tego przedmiotu, jeœli mo¿na go podnieœæ i umieœciæ w ekwipunku.")]
    [SerializeField] private int itemId;

    [Header("Requirements (if Openable, Placeable, etc.)")]
    [Tooltip("Lista ID przedmiotów z ekwipunku, które s¹ wymagane do tej interakcji.")]
    [SerializeField] private int[] requiredItemIds;

    [Header("Components & Events")]
    [SerializeField] private Animator animator;
    [SerializeField] private ReadableTextData readableTextData;
    [SerializeField] private UnityEvent onAllWallButtonPressed;
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private BoxCollider boxCollider;

    private bool isActualReading;
    private bool isActualOpen;
    private bool isActualPreesed;
    private bool isLockPicking;
    [SerializeField] private int requiredUses = 3;

    public void OnPickUp()
    {
        if (interactableType == InteractableType.Pickupable)
        {
            DestroyInteractable();
            Inventory.Instance.AddItemToInventory(this);
        }
    }

    public int GetItemId() => itemId;

    public void DestroyInteractable() => Destroy(gameObject);

    public string GetItemName() => itemName;

    public Sprite GetItemSprite() => itemSprite;


    public void OpenObject()
    {
        if (interactableType == InteractableType.Openable)
        {
            if (UIManager.Instance != null && requiredItemIds.Contains(UIManager.Instance.GetSelectedItemId()))
            {
                int selectedId = UIManager.Instance.GetSelectedItemId();
                Inventory.Instance.RemoveItemFromInventoryByID(selectedId);
                UIManager.Instance.RemoveItemFromUIByID(selectedId);
                if (animator != null)
                {
                    animator.SetTrigger("Interact");
                }
                else
                {
                    Debug.Log("Nie masz animatora");
                }

                isActualOpen = true;
            }
            else
            {
                Debug.Log($"Nie mo¿na otworzyæ {gameObject.name}. Brak odpowiedniego przedmiotu.");
            }
        }
    }

    public void PlaceObject()
    {
        if (interactableType == InteractableType.Placeable)
        {
            if (UIManager.Instance != null && requiredItemIds.Contains(UIManager.Instance.GetSelectedItemId()))
            {
                boxCollider.enabled = false;
                objectToPlace.SetActive(true);
                int selectedId = UIManager.Instance.GetSelectedItemId();
                Inventory.Instance.RemoveItemFromInventoryByID(selectedId);
                UIManager.Instance.RemoveItemFromUIByID(selectedId);
            }
        }
    }

    public void OnBookThrow()
    {
        if (interactableType == InteractableType.Throwable)
        {
            if (TryGetComponent<Rigidbody>(out var rb))
            {
                Vector3 throwDirection = transform.forward;
                float throwForce = 5f;
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
                Debug.Log($"Throwing {gameObject.name} with force {throwForce} in direction {throwDirection}!");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} does not have a Rigidbody. Cannot apply force.");
            }
        }
    }

    public bool IsOpen() => isActualOpen;

    public void CloseObject()
    {
        if (interactableType == InteractableType.Openable)
        {
            if (isActualOpen)
            {
                isActualOpen = false;
                if (animator != null)
                {
                    animator.SetTrigger("Close");
                }
                else
                {
                    Debug.Log("Nie masz animatora");
                }
            }
        }
    }

    public void OnRead()
    {
        if (interactableType == InteractableType.Readable)
        {
            UIManager.Instance.readablePanel.ShowReadablePanel();
            UIManager.Instance.readablePanel.headerTextUI.text = readableTextData.headerText;
            UIManager.Instance.readablePanel.mainTextUI.text = readableTextData.mainText;
            UIManager.Instance.readablePanel.signatureTextUI.text = readableTextData.signatureText;
            isActualReading = true;
        }
    }

    public bool IsReading() => isActualReading;

    public void OnStopRead()
    {
        if (interactableType == InteractableType.Readable)
        {
            UIManager.Instance.readablePanel.HideReadablePanel();
            isActualReading = false;
        }
    }

    public void OnPress()
    {
        if (interactableType == InteractableType.Pressable)
        {
            isActualPreesed = true;
            if (animator != null)
            {
                animator.SetTrigger("Interact");
            }
            else
            {
                Debug.Log("Nie masz animatora");
            }
            onAllWallButtonPressed?.Invoke();
        }
    }

    public bool IsPressed() => isActualPreesed;

    public void StartLockPick()
    {
        if (interactableType == InteractableType.LockPick)
        {
            UIManager.Instance.lockPickPanel.ShowLockPickPanel();
            isLockPicking = true;
        }
    }

    public bool IsLockPicking() => isLockPicking;

    public void StopLockPicking()
    {
        if (interactableType == InteractableType.LockPick)
        {
            UIManager.Instance.lockPickPanel.HideLockPickPanel();
            isLockPicking = false;
        }
    }
}