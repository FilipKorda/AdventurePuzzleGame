using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class InteractableItem : MonoBehaviour, IPickupable, IBookThrowable, IOpenable, IReadable, IPressable, IPlaceable, ILockPick, IFillable
{
    public enum InteractableType
    {
        Pickupable,
        Throwable,
        Openable,
        Readable,
        Pressable,
        Placeable,
        LockPick,
        Fillable
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
    public bool canOpenWithNoSelectedItem = false;

    private bool isActualReading;
    private bool isActualOpen;
    private bool isActualPreesed;
    private bool isLockPicking;
    [SerializeField] private int requiredUses = 3;

    [Header("Fillable Source Settings (if Fillable)")]
    [Tooltip("Typ p³ynu, który dostarcza to Ÿród³o.")]
    [SerializeField] private LiquidType providedLiquidType = LiquidType.None;

    [Tooltip("Lista mapowañ, które definiuj¹, jaki pusty pojemnik zamienia siê w jaki nape³niony.")]
    [SerializeField] private FillMapping fillMapping;

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


    public void OnFill()
    {
        if (interactableType != InteractableType.Fillable) return;

        int selectedItemId = UIManager.Instance.GetSelectedItemId();
        if (selectedItemId == -1)
        {
            Debug.Log("Musisz wybraæ pusty pojemnik, aby go nape³niæ.");
            return;
        }

        if (fillMapping == null || fillMapping.requiredEmptyItem == null)
        {
            Debug.LogError($"Brak zdefiniowanego mapowania 'fillMapping' na obiekcie {gameObject.name}");
            return;
        }

        if (fillMapping.requiredEmptyItem.GetItemId() == selectedItemId)
        {
            if (fillMapping.resultingFilledItem == null)
            {
                Debug.LogError($"Brak przypisanego przedmiotu 'resultingFilledItem' na obiekcie {gameObject.name}");
                return;
            }

            Inventory.Instance.RemoveItemFromInventoryByID(selectedItemId);
            UIManager.Instance.RemoveItemFromUIByID(selectedItemId);
            Inventory.Instance.AddItemToInventory(fillMapping.resultingFilledItem);

            Debug.Log($"Nape³niono '{fillMapping.requiredEmptyItem.GetItemName()}' p³ynem typu '{providedLiquidType}'. Otrzymano: {fillMapping.resultingFilledItem.GetItemName()}");
        }
    }

    public void OpenObject()
    {
        if (interactableType == InteractableType.Openable)
        {
            if(canOpenWithNoSelectedItem)
            {
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
            else
            {
                Debug.Log($"Nie mo¿na otworzyæ {gameObject.name}. Brak odpowiedniego przedmiotu.");
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
            if (UIManager.Instance != null && requiredItemIds.Contains(UIManager.Instance.GetSelectedItemId()))
            {
                UIManager.Instance.lockPickPanel.ShowLockPickPanel();
                isLockPicking = true;
            }
            else
            {
                Debug.Log($"Nie mo¿na otworzyæ {gameObject.name}. Brak odpowiedniego przedmiotu.");
            }
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

    [System.Serializable] 
    public class FillMapping
    {
        [Tooltip("Pusty przedmiot, który gracz musi trzymaæ (np. prefab Pustego Wiadra).")]
        public InteractableItem requiredEmptyItem;

        [Tooltip("Przedmiot, który gracz otrzyma po nape³nieniu (np. prefab Wiadra z Wod¹).")]
        public InteractableItem resultingFilledItem;
    }
}