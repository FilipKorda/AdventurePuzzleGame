using UnityEngine;
using System; 

[CreateAssetMenu(fileName = "New Bool Variable", menuName = "Game Variables/Bool Variable")]
public class BoolVariable : ScriptableObject
{
    [SerializeField] private bool value;
    public event Action OnValueChanged;

    public bool Value
    {
        get { return value; }
        set
        {
            if (this.value != value)
            {
                this.value = value;
                OnValueChanged?.Invoke(); 
            }
        }
    }

}