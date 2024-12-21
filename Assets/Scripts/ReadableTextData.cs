using UnityEngine;

[CreateAssetMenu(fileName = "ReadableTextData", menuName = "ScriptableObjects/ReadableTextData", order = 1)]
public class ReadableTextData : ScriptableObject
{
    [Header("Text to Display")]
    public string headerText;
    [TextArea] public string mainText;
    public string signatureText;
}