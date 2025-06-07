using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class InteractableItem : MonoBehaviour, IPickupable, IBookThrowable, IOpenable, IReadable, IPressable, IPlaceable
{
    public enum InteractableType
    {
        Pickupable,
        Throwable,
        Openable,
        Readable,
        Pressable,
        Placeable
    }

    public InteractableType interactableType;
    [SerializeField] private string itemName = "Przedmiot";
    [SerializeField] private Sprite itemSprite;

    [Header("ID (if Pickupable)")]
    [Tooltip("ID tego przedmiotu, jeœli mo¿na go podnieœæ i umieœciæ w ekwipunku.")]
    [SerializeField] private int itemId; // ID samego przedmiotu

    [Header("Requirements (if Openable, Placeable, etc.)")]
    [Tooltip("Lista ID przedmiotów z ekwipunku, które s¹ wymagane do tej interakcji.")]
    [SerializeField] private int[] requiredItemIds; // Lista ID wymaganych przedmiotów

    [Header("Components & Events")]
    [SerializeField] private Animator animator;
    [SerializeField] private ReadableTextData readableTextData;
    [SerializeField] private UnityEvent onAllWallButtonPressed;
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private BoxCollider boxCollider;

    private bool isActualReading;
    private bool isActualOpen;
    private bool isActualPreesed;
    [SerializeField] private int requiredUses = 3;
    private int toolUsageCount = 0;

    // --- Metody interfejsu IPickupable ---

    public void OnPickUp()
    {
        if (interactableType == InteractableType.Pickupable)
        {
            DestroyInteractable();
            Inventory.Instance.AddItemToInventory(this);
        }
    }

    // ZWRACA ID SAMEGO PRZEDMIOTU (dla ekwipunku)
    public int GetItemId() => itemId;

    public void DestroyInteractable() => Destroy(gameObject);

    public string GetItemName() => itemName;

    public Sprite GetItemSprite() => itemSprite;

    // --- Pozosta³e metody interakcji ---

    public void OpenObject()
    {
        if (interactableType == InteractableType.Openable)
        {
            // Sprawdzamy, czy wybrany przedmiot ma ID z listy WYMAGANYCH ID
            if (UIManager.Instance != null && requiredItemIds.Contains(UIManager.Instance.GetSelectedItemId()))
            {
                int selectedId = UIManager.Instance.GetSelectedItemId();
                Inventory.Instance.RemoveItemFromInventoryByID(selectedId);
                UIManager.Instance.RemoveItemFromUIByID(selectedId);
                animator.SetTrigger("Interact");
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
            // Sprawdzamy, czy wybrany przedmiot ma ID z listy WYMAGANYCH ID
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

    // ... reszta metod (OnRead, OnPress, etc.) pozostaje bez zmian ...

    #region Pozosta³e metody bez zmian
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
            if (isActualOpen) // Za³ó¿my, ¿e zamykanie nie wymaga klucza
            {
                isActualOpen = false;
                animator.SetTrigger("Close");
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
            animator.SetTrigger("Interact");
            onAllWallButtonPressed?.Invoke();
        }
    }

    public bool IsPressed() => isActualPreesed;

    #endregion
}