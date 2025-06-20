using UnityEngine;

public interface IPickupable
{
    void OnPickUp();
    int GetItemId();
    void DestroyInteractable();
    string GetItemName();
    Sprite GetItemSprite();
    GameObject GetItemPrefab();
}