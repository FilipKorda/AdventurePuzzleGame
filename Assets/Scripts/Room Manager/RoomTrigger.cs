using UnityEngine;
using UnityEngine.Localization;

[RequireComponent(typeof(Collider))]
public class RoomTrigger : MonoBehaviour
{
    [SerializeField]  private LocalizedString localizeRoomName; 

    public string LocalizeRoomName => localizeRoomName.GetLocalizedString();

    private bool doItOnce = false;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!doItOnce)
        {
            if (!other.CompareTag("Player")) return;
            if (RoomManager.Instance == null) return;

            RoomManager.Instance.EnterRoom(this);
            doItOnce = true;

        }
      
    }
}
