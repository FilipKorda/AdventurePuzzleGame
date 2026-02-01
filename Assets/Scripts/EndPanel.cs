using TMPro;
using UnityEngine;
using System.Collections;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI firstText;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TextMeshProUGUI thierdText;
    [SerializeField] private TextMeshProUGUI fourthText;
    [SerializeField] private TextMeshProUGUI fifthText;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float delayBetweenTexts = 0.5f;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    private void OnEnable()
    {
        StartCoroutine(StartEndPanel());
    }

    private IEnumerator StartEndPanel()
    {
        playerBehaviour.disablePlayer = true;

        SetAlpha(0f, firstText, secondText, thierdText, fourthText, fifthText);

        yield return FadeText(firstText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeText(secondText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeText(thierdText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeText(fourthText);
        yield return new WaitForSeconds(delayBetweenTexts);

        yield return FadeText(fifthText);
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

    private void SetAlpha(float value, params TextMeshProUGUI[] texts)
    {
        foreach (var text in texts)
        {
            Color color = text.color;
            color.a = value;
            text.color = color;
        }
    }
}
