using UnityEngine;

public class ObjectDisabler : MonoBehaviour
{
    public void Start()
    {
#if UNITY_EDITOR
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
}
