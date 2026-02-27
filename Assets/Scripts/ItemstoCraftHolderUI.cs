using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemstoCraftHolderUI : MonoBehaviour
{
    [SerializeField] private RectTransform itemsToCraftBackground;
    [SerializeField] private Animator backgroundAnimator;
    [SerializeField] private float backgroundAnimationTime = 0.6f;
    [SerializeField] private float fadeTime = 0.25f;

    private bool backgroundPlayed;
    private int collectedCount;

    public void OnItemCollected(Image image)
    {
        collectedCount++;

        if (!backgroundPlayed)
        {
            backgroundPlayed = true;
            StartCoroutine(FirstItemFlow(image));
        }
        else
        {
            StartCoroutine(FadeImageFromZeroToOne(image));
        }
    }

    private IEnumerator FirstItemFlow(Image image)
    {
        backgroundAnimator.SetTrigger("Play");
        yield return new WaitForSeconds(backgroundAnimationTime);
        yield return FadeImageFromZeroToOne(image);
    }


    public void HideBackgroud()
    {
        StartCoroutine(HideBackgroudCorutine());
    }

    private IEnumerator HideBackgroudCorutine()
    {
        yield return new WaitForSeconds(1);
        backgroundAnimator.SetTrigger("Hide");
    }

    private IEnumerator FadeImageFromZeroToOne(Image image)
    {
        float t = 0f;
        Color c = image.color;
        c.a = 0f;
        image.color = c;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeTime);
            image.color = c;
            yield return null;
        }

        c.a = 1f;
        image.color = c;
    }
}