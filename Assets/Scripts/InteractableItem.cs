using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private int requiredItemId;
    [SerializeField] private Animator animator;
    [SerializeField] private ReadableTextData readableTextData;
    private bool isActualReading;
    private bool isActualOpen;
    private bool isActualPreesed;
    [SerializeField] private UnityEvent onAllWallButtonPressed;
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private BoxCollider boxCollider;

    public void OnPickUp()
    {
        if (interactableType == InteractableType.Pickupable)
        {
            DestroyInteractable();
            Inventory.Instance.AddItemToInventory(this);
        }
    }

    public string GetItemName() => itemName;

    public Sprite GetItemSprite() => itemSprite;

    public void DestroyInteractable() => Destroy(gameObject);

    public int GetItemId() => requiredItemId;


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

    public void OpenObject()
    {
        if (interactableType == InteractableType.Openable)
        {
            if (UIManager.Instance != null && UIManager.Instance.GetSelectedItemId() == requiredItemId)
            {
                Inventory.Instance.RemoveItemFromInventoryByID(requiredItemId);
                UIManager.Instance.RemoveItemFromUIByID(requiredItemId);
                animator.SetTrigger("Interact");
                isActualOpen = true;
            }
            else
            {
                Debug.Log($"Nie mo¿na otworzyæ drzwi {gameObject.name}. Brak odpowiedniego przedmiotu o ID: {requiredItemId}");
            }
        }

    }

    public bool IsOpen()
    {
        return isActualOpen;
    }

    public void CloseObject()
    {
        if (interactableType == InteractableType.Openable)
        {
            if (UIManager.Instance != null && UIManager.Instance.GetSelectedItemId() == requiredItemId)
            {
                isActualOpen = false;
                animator.SetTrigger("Close");
            }
            else
            {
                Debug.Log($"Nie mo¿na otworzyæ drzwi {gameObject.name}. Brak odpowiedniego przedmiotu o ID: {requiredItemId}");
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
            Debug.Log($"Read");
        }
    }

    public bool IsReading()
    {
        return isActualReading;
    }

    public void OnStopRead()
    {
        if (interactableType == InteractableType.Readable)
        {
            UIManager.Instance.readablePanel.HideReadablePanel();
            UIManager.Instance.readablePanel.headerTextUI.text = "";
            UIManager.Instance.readablePanel.mainTextUI.text = "";
            UIManager.Instance.readablePanel.signatureTextUI.text = "";
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

    public bool IsPressed()
    {
        return isActualPreesed;
    }

    public void CheckIfIsPressed()
    {
        IsPressed();
    }

    public void PlaceObject()
    {
        if(interactableType == InteractableType.Placeable)
        {
            if (UIManager.Instance != null && UIManager.Instance.GetSelectedItemId() == requiredItemId)
            {
                boxCollider.enabled = false;
                objectToPlace.SetActive(true);
                Inventory.Instance.RemoveItemFromInventoryByID(requiredItemId);
                UIManager.Instance.RemoveItemFromUIByID(requiredItemId);
            }
        }    
    }
}
