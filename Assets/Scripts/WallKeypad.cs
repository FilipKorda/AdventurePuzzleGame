using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WallKeypad : MonoBehaviour, IPinNumber
{
    public static WallKeypad Instance { get; private set; }

    [SerializeField] private string correctCode = "3547";

    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider[] boxColliders;

    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;

    [SerializeField] private Sprite transparentEmptyNumber;
    [SerializeField] private Sprite number0;
    [SerializeField] private Sprite number1;
    [SerializeField] private Sprite number2;
    [SerializeField] private Sprite number3;
    [SerializeField] private Sprite number4;
    [SerializeField] private Sprite number5;
    [SerializeField] private Sprite number6;
    [SerializeField] private Sprite number7;
    [SerializeField] private Sprite number8;
    [SerializeField] private Sprite number9;

    [SerializeField] private Image numberSlot1;
    [SerializeField] private Image numberSlot2;
    [SerializeField] private Image numberSlot3;
    [SerializeField] private Image numberSlot4;

    private Image[] slots;
    private string currentCode = "";
    private PinNumber pinNumber;
    private Coroutine resetCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        slots = new[] { numberSlot1, numberSlot2, numberSlot3, numberSlot4 };
        ResetSlots();
    }

    public void OpenGate()
    {
        animator.SetTrigger("OPEN");
    }

    private void DisableColliders()
    {
        foreach (var collider in boxColliders)
            collider.enabled = false;
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
            slots[currentCode.Length - 1].sprite = GetNumberSprite(pinNumber);
        }
        else if (pinNumber == PinNumber.Clear)
        {
            ResetCode();
        }
        else if (pinNumber == PinNumber.Accept)
        {
            if (currentCode == correctCode)
            {
                SetSlotsColor(correctColor);
                OpenGate();
                DisableColliders();
                Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
            }
            else
            {
                SetSlotsColor(wrongColor);

                if (resetCoroutine != null)
                    StopCoroutine(resetCoroutine);

                resetCoroutine = StartCoroutine(ResetAfterDelay());
            }
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        ResetCode();
    }

    private void ResetCode()
    {
        currentCode = "";
        ResetSlots();
    }

    private void ResetSlots()
    {
        foreach (var slot in slots)
        {
            slot.sprite = transparentEmptyNumber;
            slot.color = defaultColor;
        }
    }

    private void SetSlotsColor(Color color)
    {
        foreach (var slot in slots)
            slot.color = color;
    }

    private Sprite GetNumberSprite(PinNumber number)
    {
        return number switch
        {
            PinNumber.None => number0,
            PinNumber.One => number1,
            PinNumber.Two => number2,
            PinNumber.Three => number3,
            PinNumber.Four => number4,
            PinNumber.Five => number5,
            PinNumber.Six => number6,
            PinNumber.Seven => number7,
            PinNumber.Eight => number8,
            PinNumber.Nine => number9,
            _ => transparentEmptyNumber
        };
    }


    public void HighlightButton()
    {
        //nic
    }

    public void ResetHighlightButton()
    {
        //nic
    }
}