using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    private void Awake()
    {
        ApplyColorToAllTargets(hiddenTextColor);
    }

    private Coroutine sequenceCoroutine;
    private Coroutine initializeCoroutine;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
        ApplyColorToAllTargets(hiddenTextColor);

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

    public void StartTypingAll()
    {
        SetPlayerDisabled(true);
        StartSequenceFromSources();
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
            SetPlayerDisabled(true);
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

        yield return new WaitForSeconds(5f);
        DoAfterEndOfTyping();
    }

    private void DoAfterEndOfTyping()
    {
        Services.Audio.PlaySFX("StartGameSound");     
        SetPlayerDisabled(false);
        gameObject.SetActive(false);
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
