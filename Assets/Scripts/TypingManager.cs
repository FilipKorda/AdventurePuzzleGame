using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    [System.Serializable]
    private class TypingTarget
    {
        public TMP_Text textMeshPro;
        public LocalizeStringEvent localizeStringEvent;

        [HideInInspector] public string latestFullText;
        [HideInInspector] public Coroutine typingCoroutine;
    }

    [SerializeField] private TypingTarget[] targets;
    [SerializeField] private float letterDelay = 0.05f;
    [SerializeField] private float delayBetweenTargets = 2f;
    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private Color hiddenTextColor = Color.black;
    [SerializeField] private Color visibleTextColor = Color.white;
    [SerializeField] private PlayerBehaviour playerBehaviour;

    [SerializeField] private CanvasGroup textCanvasGroup;
    [SerializeField] private CanvasGroup controlsCanvasGroup;
    [SerializeField] private Image circleBackgroundAnim;
    [SerializeField] private float defaultTimerDuration = 5f;

    private Coroutine timerCoroutine;

    [SerializeField] private TextMeshProUGUI textTimer;

    private void Awake()
    {
        ApplyColorToAllTargets(hiddenTextColor);
        textCanvasGroup.alpha = 1f;
        controlsCanvasGroup.alpha = 0f;
        textTimer.text = string.Empty;
    }

    private Coroutine sequenceCoroutine;
    private Coroutine initializeCoroutine;

    private void OnEnable()
    {
#if UNITY_EDITOR
        gameObject.SetActive(true);
#else
        gameObject.SetActive(true);
#endif

        LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
        ApplyColorToAllTargets(hiddenTextColor);

        SetPlayerDisabled(true);

        if (playOnEnable)
        {
            StartSequenceFromSources();
        }
        else
        {
            PrepareTargetsWithoutPlaying();
        }
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;

        if (initializeCoroutine != null)
        {
            StopCoroutine(initializeCoroutine);
            initializeCoroutine = null;
        }

        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        for (int i = 0; i < targets.Length; i++)
        {
            TypingTarget target = targets[i];

            if (target == null)
            {
                continue;
            }

            if (target.typingCoroutine != null)
            {
                StopCoroutine(target.typingCoroutine);
                target.typingCoroutine = null;
            }
        }
    }

    public void StartTyping(int index)
    {
        if (index < 0 || index >= targets.Length)
        {
            return;
        }

        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        TypingTarget target = targets[index];

        if (target == null || target.textMeshPro == null)
        {
            return;
        }

        if (target.typingCoroutine != null)
        {
            StopCoroutine(target.typingCoroutine);
        }

        target.textMeshPro.color = visibleTextColor;
        target.typingCoroutine = StartCoroutine(TypeText(target, target.latestFullText ?? string.Empty));
    }

    private void OnSelectedLocaleChanged(UnityEngine.Localization.Locale _)
    {
        if (playOnEnable)
        {
            StartSequenceFromSources();
        }
        else
        {
            PrepareTargetsWithoutPlaying();
        }
    }

    private void StartSequenceFromSources()
    {
        SetPlayerDisabled(true);

        if (initializeCoroutine != null)
        {
            StopCoroutine(initializeCoroutine);
        }

        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        initializeCoroutine = StartCoroutine(InitializeTargetsAndPlay());
    }

    private void PrepareTargetsWithoutPlaying()
    {
        SetPlayerDisabled(true);

        if (initializeCoroutine != null)
        {
            StopCoroutine(initializeCoroutine);
        }

        initializeCoroutine = StartCoroutine(InitializeTargetsOnly());
    }

    private IEnumerator InitializeTargetsOnly()
    {
        yield return InitializeTargets();
        initializeCoroutine = null;
    }

    private IEnumerator InitializeTargetsAndPlay()
    {
        yield return InitializeTargets();
        sequenceCoroutine = StartCoroutine(TypeTargetsSequentially());
        initializeCoroutine = null;
    }

    private IEnumerator InitializeTargets()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            TypingTarget target = targets[i];

            if (target == null || target.textMeshPro == null)
            {
                continue;
            }

            if (target.typingCoroutine != null)
            {
                StopCoroutine(target.typingCoroutine);
                target.typingCoroutine = null;
            }

            target.textMeshPro.color = hiddenTextColor;

            if (target.localizeStringEvent != null)
            {
                target.textMeshPro.text = string.Empty;
                target.localizeStringEvent.enabled = false;

                AsyncOperationHandle<string> handle = target.localizeStringEvent.StringReference.GetLocalizedStringAsync();
                yield return handle;

                target.latestFullText = handle.Status == AsyncOperationStatus.Succeeded
                    ? handle.Result ?? string.Empty
                    : string.Empty;
            }
            else
            {
                target.latestFullText = target.textMeshPro.text;
                target.textMeshPro.text = string.Empty;
            }
        }
    }

    private IEnumerator TypeTargetsSequentially()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            TypingTarget target = targets[i];

            if (target == null || target.textMeshPro == null)
            {
                continue;
            }

            target.typingCoroutine = StartCoroutine(TypeText(target, target.latestFullText ?? string.Empty));
            yield return target.typingCoroutine;

            if (i < targets.Length - 1)
            {
                yield return new WaitForSeconds(delayBetweenTargets);
            }
        }

        sequenceCoroutine = null;

        timerCoroutine = StartCoroutine(StartTimer(5f));
        yield return new WaitForSeconds(5f);
        StartCoroutine(ChangeAlfaCanvasGroup(0f, 1f, 1f));
    }

    private void DoAfterEndOfTyping()
    {
        Services.Audio.PlaySFX("StartGameSound");
        SetPlayerDisabled(false);
        gameObject.SetActive(false);
    }

    private IEnumerator ChangeAlfaCanvasGroup(float targetTextAlpha, float targetControlsAlpha, float duration = 1f)
    {
        if (textCanvasGroup != null)
        {
            float elapsed = 0f;
            float startTextAlpha = textCanvasGroup.alpha;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                textCanvasGroup.alpha = Mathf.Lerp(startTextAlpha, targetTextAlpha, t);
                yield return null;
            }

            textCanvasGroup.alpha = targetTextAlpha;
        }

        if (controlsCanvasGroup != null)
        {
            float elapsed2 = 0f;
            float startControlsAlpha = controlsCanvasGroup.alpha;

            while (elapsed2 < duration)
            {
                elapsed2 += Time.deltaTime;
                float t2 = Mathf.Clamp01(elapsed2 / duration);
                controlsCanvasGroup.alpha = Mathf.Lerp(startControlsAlpha, targetControlsAlpha, t2);
                yield return null;
            }

            controlsCanvasGroup.alpha = targetControlsAlpha;
        }

        timerCoroutine = StartCoroutine(StartTimer(5f));
        yield return new WaitForSeconds(5f);
        DoAfterEndOfTyping();
    }

    private IEnumerator StartTimer(float duration)
    {
        if (circleBackgroundAnim != null)
        {
            circleBackgroundAnim.fillClockwise = true;
            circleBackgroundAnim.fillAmount = 0f;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            if (circleBackgroundAnim != null)
            {
                circleBackgroundAnim.fillAmount = progress;
            }

            float remaining = Mathf.Max(0f, duration - elapsed);
            if (textTimer != null)
            {
                textTimer.text = Mathf.CeilToInt(remaining).ToString();
            }

            yield return null;
        }

        yield return null;
        yield return null;

        if (circleBackgroundAnim != null)
        {
            circleBackgroundAnim.fillAmount = 0f;
        }

        if (textTimer != null)
        {
            textTimer.text = string.Empty;
        }

        timerCoroutine = null;
    }

    private IEnumerator TypeText(TypingTarget target, string fullText)
    {
        target.textMeshPro.color = visibleTextColor;
        target.textMeshPro.text = string.Empty;

        for (int i = 0; i < fullText.Length; i++)
        {
            target.textMeshPro.text += fullText[i];
            Services.Audio.PlaySFX("softkyboardtyping");
            yield return new WaitForSeconds(letterDelay);
        }

        target.typingCoroutine = null;
    }

    private void ApplyColorToAllTargets(Color color)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            TypingTarget target = targets[i];

            if (target != null && target.textMeshPro != null)
            {
                target.textMeshPro.color = color;
            }
        }
    }

    private void SetPlayerDisabled(bool isDisabled)
    {
        if (playerBehaviour != null)
        {
            playerBehaviour.disablePlayer = isDisabled;
        }
    }
}
