using UnityEngine;
using UnityEngine.Events; 

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event, na który ten komponent ma nas³uchiwaæ.")]
    public GameEvent Event;

    [Tooltip("Akcje, które maj¹ zostaæ wykonane, gdy event zostanie wywo³any.")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}