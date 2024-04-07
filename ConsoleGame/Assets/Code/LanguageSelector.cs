using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    private void Start()
    {
        int ID = PlayerPrefs.GetInt("LocalKey", 0);
        ChangeLocale(ID);
    }

    private bool active = false;
    public void ChangeLocale(int localID)
    {
        if (active)
            return;
        StartCoroutine(setLocale(localID));
    }

    IEnumerator setLocale(int _localID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localID];
        PlayerPrefs.SetInt("LocalKey", _localID);
        active = false;
    }
}
