using TMPro;
using UnityEngine;

public class WallKeypad : MonoBehaviour, IPinNumber
{
    public static WallKeypad Instance { get; private set; }

    [SerializeField] private string correctCode = "3207";
    [SerializeField] private TextMeshProUGUI enterpinText;

    private string currentCode = "";
    private PinNumber pinNumber;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetPinNumber(PinNumber number)
    {
        pinNumber = number;
    }

    public void EnterPinNumber()
    {
        if (pinNumber >= PinNumber.None && pinNumber <= PinNumber.Nine)
        {
            if (currentCode.Length >= 4) return;

            currentCode += ((int)pinNumber).ToString();
            enterpinText.text = currentCode;
        }
        else if (pinNumber == PinNumber.Clear)
        {
            ResetCode();
        }
        else if (pinNumber == PinNumber.Accept)
        {
            if (currentCode == correctCode)
            {
                Debug.Log("OPEN");
            }
            else
            {
                ResetCode();
            }
        }
    }

    private void ResetCode()
    {
        currentCode = "";
        enterpinText.text = "";
    }
}