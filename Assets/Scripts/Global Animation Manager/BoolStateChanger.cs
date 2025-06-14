using UnityEngine;

public class BoolStateChanger : MonoBehaviour
{
    [Tooltip("Zmienna typu Bool (ScriptableObject), której wartoœæ bêdziemy zmieniaæ.")]
    public BoolVariable boolVariable;

    public void SetStateTrue()
    {
        if (boolVariable != null)
        {
            boolVariable.Value = true;
            Debug.Log($"Zmieniono stan '{boolVariable.name}' na True");
        }
    }

    public void SetStateFalse()
    {
        if (boolVariable != null)
        {
            boolVariable.Value = false;
            Debug.Log($"Zmieniono stan '{boolVariable.name}' na False");
        }
    }
}