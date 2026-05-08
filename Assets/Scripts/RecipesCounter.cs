using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipesCounter : MonoBehaviour
{
    [SerializeField] private GameObject thisGameObject;
    [SerializeField] private TextMeshProUGUI recipeAmountText;
    [SerializeField] private int recipeAmount;

    private readonly List<int> recipeIds = new() { 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 };

    private Coroutine animateRoutine;

    private void Start()
    {
        DisableThisGameObject();
    }

    public void UpdateRecipeCount()
    {
        if (Inventory.Instance == null)
        {
            recipeAmount = 0;
        }
        else
        {
            recipeAmount = Inventory.Instance.CountItemsWithIds(recipeIds);
        }

        recipeAmountText.text = recipeAmount.ToString();
        if (thisGameObject.activeInHierarchy)
            PlayCollectRecipeAnimation();
    }

    public void DisableThisGameObject()
    {
        Debug.Log("DisableThisGameObject: " + thisGameObject.name);
        thisGameObject.SetActive(false);
    }


    public void ActiveThisGameObject()
    {
        if (recipeAmount < 1) return;
        if (thisGameObject.activeSelf) return;

        thisGameObject.SetActive(true);
        PlayCollectRecipeAnimation();
    }


    public void PlayCollectRecipeAnimation()
    {
        if (animateRoutine != null)
            StopCoroutine(animateRoutine);

        animateRoutine = StartCoroutine(CoinGainAnimation());
    }

    private IEnumerator CoinGainAnimation()
    {
        RectTransform rectTransform = recipeAmountText.rectTransform;

        Vector3 startScale = Vector3.one;
        Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.2f);

        Color startColor = Color.white;
        Color targetColor = Color.green;

        float duration = 1f;
        float halfDuration = duration * 0.2f;

        float time = 0f;

        while (time < halfDuration)
        {
            time += Time.deltaTime;
            float t = time / halfDuration;

            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            recipeAmountText.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        rectTransform.localScale = targetScale;
        recipeAmountText.color = targetColor;

        time = 0f;

        while (time < halfDuration)
        {
            time += Time.deltaTime;
            float t = time / halfDuration;

            rectTransform.localScale = Vector3.Lerp(targetScale, startScale, t);
            recipeAmountText.color = Color.Lerp(targetColor, startColor, t);

            yield return null;
        }

        rectTransform.localScale = startScale;
        recipeAmountText.color = startColor;

        animateRoutine = null;
    }

}
