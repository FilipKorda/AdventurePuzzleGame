using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ReadableTextData", menuName = "ScriptableObjects/ReadableTextData", order = 1)]
public class ReadableTextData : ScriptableObject
{
    [Header("Text to Display")]
    public string headerText;
    [TextArea] public string mainText;
    public string signatureText;

    public LocalizedString localizeHeader;
    public LocalizedString localizeMainText;
    public LocalizedString localizeSignature;
}