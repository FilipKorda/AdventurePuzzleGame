using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    private int itemId;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image imageSelector;

    private string itemName;

    private void Start()
    {
        SetHighlighted(false);
    }

    public void SetItem(string name, Sprite sprite, int id)
    {
        itemName = name;
        itemNameText.text = name;
        itemImage.sprite = sprite;
        itemId = id;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public int GetItemId()
    {
        return itemId;
    }

    public void ClearItemName()
    {
        itemNameText.text = "";
    }

    public void SetHighlighted(bool isHighlighted)
    {
        imageSelector.enabled = isHighlighted;
    }
}
