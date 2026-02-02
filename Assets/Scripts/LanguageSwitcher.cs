using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageSwitcher : MonoBehaviour
{
    public void SetPolish()
    {
        StartCoroutine(SetLocale("pl-PL"));
    }

    public void SetEnglish()
    {
        StartCoroutine(SetLocale("en-US"));
    }

    IEnumerator SetLocale(string code)
    {
        yield return LocalizationSettings.InitializationOperation;

        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == code)
            {
                LocalizationSettings.SelectedLocale = locale;
                yield break;
            }
        }
    }
}
