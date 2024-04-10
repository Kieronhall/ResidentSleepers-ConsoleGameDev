using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class LanguageSelector : MonoBehaviour
{
    private static LanguageSelector instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
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
