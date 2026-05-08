using UnityEngine;

public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControlManager.Instance.DisableLampOnTrigger();

            boxCollider.enabled = false;
        }
    }
}