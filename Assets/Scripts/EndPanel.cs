using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI firstText;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TextMeshProUGUI thierdText;
    [SerializeField] private TextMeshProUGUI fourthText;
    [SerializeField] private TextMeshProUGUI fifthText;
    [SerializeField] private TextMeshProUGUI sixText;

    [SerializeField] private Image sixButton;
    [SerializeField] private Button button;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float delayBetweenTexts = 0.5f;

    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private GameObject coinsGameObject;

    private void OnEnable()
    {
        coinsGameObject.SetActive(false);
        StartCoroutine(StartEndPanel());
    }

    private void OnDisable()
    {
        if (pausePanel != null)
            pausePanel.SetAllowPause(true);
    }

    private IEnumerator StartEndPanel()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (pausePanel != null)
            pausePanel.SetAllowPause(false);

        button.interactable = false;
        PlayerControlManager.Instance.LockPlayer();

        SetAlpha(0f, firstText, secondText, thierdText, fourthText, fifthText, sixText);

        yield return FadeText(firstText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeText(secondText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeText(thierdText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeText(fourthText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeText(fifthText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeButtonText(sixText, sixButton);      
    }

    private IEnumerator FadeText(TextMeshProUGUI text)
    {
        float time = 0f;
        Color color = text.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            text.color = color;
            yield return null;
        }

        color.a = 1f;
        text.color = color;
    }

    private IEnumerator FadeButtonText(TextMeshProUGUI text, Image buttonImage)
    {
        float time = 0f;
        Color color = text.color;
        Color colortwo = buttonImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            colortwo.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            text.color = color;
            buttonImage.color = colortwo;
            yield return null;
        }

        color.a = 1f;
        colortwo.a = 1f;
        text.color = color;
        buttonImage.color = colortwo;
        button.interactable = true;
        SelectEndButton();
    }

    private void SetAlpha(float value, params TextMeshProUGUI[] texts)
    {
        foreach (var text in texts)
        {
            Color color = text.color;
            color.a = value;
            text.color = color;
        }
    }

    public void SelectEndButton()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}
