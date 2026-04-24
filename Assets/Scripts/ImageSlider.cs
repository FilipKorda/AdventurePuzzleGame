using UnityEngine;
using System.Collections;

public class ImageSlider : MonoBehaviour
{
    [SerializeField] private RectTransform[] images;
    [SerializeField] private float spacing = 600f;
    [SerializeField] private float animationTime = 0.4f;
    [SerializeField] private float yPos = -19.5f;
    [SerializeField] private float centerXPos = 0f;
    [SerializeField] private bool isAnimating;
    private int currentIndex;
    private Coroutine coroutineAnimation;

    public int CurrentIndex => currentIndex;
    [SerializeField] private MovingWallwithPaintingManager movingWallwithPaintingManager;

    void Start()
    {
        SetInstant();
    }

    public void Next()
    {
        if (isAnimating)
            return;

        currentIndex = (currentIndex + 1) % images.Length;
        coroutineAnimation = StartCoroutine(Animate());

    }

    private IEnumerator Animate()
    {
        isAnimating = true;
        Services.Audio.PlaySFX("CardSwap");
        float elapsed = 0f;

        Vector2[] start = new Vector2[images.Length];
        Vector2[] target = new Vector2[images.Length];

        for (int i = 0; i < images.Length; i++)
        {
            start[i] = images[i].anchoredPosition;
            target[i] = new Vector2(centerXPos + (i - currentIndex) * spacing, yPos);
        }

        while (elapsed < animationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationTime;

            for (int i = 0; i < images.Length; i++)
                images[i].anchoredPosition = Vector2.Lerp(start[i], target[i], t);

            yield return null;
        }

        for (int i = 0; i < images.Length; i++)
            images[i].anchoredPosition = target[i];

        movingWallwithPaintingManager.CheckWinPuzzle();
        isAnimating = false;
    }

    private void SetInstant()
    {
        for (int i = 0; i < images.Length; i++)
            images[i].anchoredPosition = new Vector2(centerXPos + (i - currentIndex) * spacing, yPos);
    }
}