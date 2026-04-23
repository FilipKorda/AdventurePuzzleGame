using UnityEngine;

public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private PlayerBehaviour playerBehaviour;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerBehaviour.DisableLampOnTrigger();

            boxCollider.enabled = false;
        }
    }
}