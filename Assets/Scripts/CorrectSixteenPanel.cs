using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CorrectSixteenPanel : MonoBehaviour
{
    [SerializeField] private Image firstImage;
    [SerializeField] private Image secondImage;
    [SerializeField] private Image thirdImage;
    [SerializeField] private Image fourthImage;
    [SerializeField] private float swapDuration = 0.25f;

    private RectTransform[] imageRects;
    private int currentVisibleImageIndex = 0;
    private bool isAnimating = false;
    private float imageWidth;

    public int CurrentVisibleImageIndex => currentVisibleImageIndex;

    [SerializeField] private CorrectSixteenPanelsManager panelsManager;


    private void Awake()
    {
        imageRects = new RectTransform[4]
        {
            firstImage.rectTransform,
            secondImage.rectTransform,
            thirdImage.rectTransform,
            fourthImage.rectTransform
        };

        imageWidth = firstImage.rectTransform.rect.width;
        SetupInitialState();
    }

    public void TrySwapPanel()
    {
        if (isAnimating) return;
        StartCoroutine(SwapRightCoroutine());
    }

    private void SetupInitialState()
    {
        for (int i = 0; i < imageRects.Length; i++)
        {
            imageRects[i].anchoredPosition = new Vector2(99999f, 0f);
        }

        imageRects[currentVisibleImageIndex].anchoredPosition = Vector2.zero;
    }

    private IEnumerator SwapRightCoroutine()
    {
        isAnimating = true;
        Services.Audio.PlaySFX("CardSwap");
        int nextIndex = currentVisibleImageIndex - 1;
        if (nextIndex < 0)
        {
            nextIndex = imageRects.Length - 1;
        }

        RectTransform currentRect = imageRects[currentVisibleImageIndex];
        RectTransform nextRect = imageRects[nextIndex];

        nextRect.anchoredPosition = new Vector2(-imageWidth, 0f);

        Vector2 currentStart = currentRect.anchoredPosition;
        Vector2 currentTarget = new Vector2(imageWidth, 0f);

        Vector2 nextStart = nextRect.anchoredPosition;
        Vector2 nextTarget = Vector2.zero;

        float time = 0f;

        while (time < swapDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / swapDuration);

            currentRect.anchoredPosition = Vector2.Lerp(currentStart, currentTarget, t);
            nextRect.anchoredPosition = Vector2.Lerp(nextStart, nextTarget, t);

            yield return null;
        }

        currentRect.anchoredPosition = new Vector2(imageWidth, 0f);
        nextRect.anchoredPosition = Vector2.zero;

        currentVisibleImageIndex = nextIndex;
        isAnimating = false;

        if (panelsManager != null)
        {
            panelsManager.CheckPuzzle();
        }

    }
}
