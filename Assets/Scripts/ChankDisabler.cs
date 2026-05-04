using UnityEngine;

public class ChunkDisabler : MonoBehaviour
{
    [SerializeField] GameObject[] allRooms;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        for (int i = 0; i < allRooms.Length; i++)
            allRooms[i].SetActive(true);
    }


    [ContextMenu("RUN")]
    public void Run()
    {
        for (int i = 0; i < allRooms.Length; i++)
            allRooms[i].SetActive(true);
    }
}