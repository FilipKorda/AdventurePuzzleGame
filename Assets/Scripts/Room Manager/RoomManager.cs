using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private RoomUI roomUI;

    private RoomTrigger currentRoom;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EnterRoom(RoomTrigger roomTrigger)
    {
        if (roomTrigger == null) return;

        if (currentRoom == roomTrigger)
            return;

        currentRoom = roomTrigger;

        if (roomUI != null)
        {
            roomUI.ShowRoomName(roomTrigger.LocalizeRoomName);
        }

    }
}
