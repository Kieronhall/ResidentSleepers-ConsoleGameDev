
using ThirdPerson;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using FMODUnity;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuCanvas;
    [SerializeField] private GameObject _deathMenuCanvas;
    [SerializeField] private GameObject _optionMenuCanvas;
    [SerializeField] private GameObject _endMenuCanvas;
    [SerializeField] private GameObject _exitMenuCanvas;

    public PlayerControls _input;
    public ThirdPersonAim _controlinput;

    [SerializeField] private GameObject _pauseMenuFB;
    [SerializeField] private GameObject _deathMenuFB;
    [SerializeField] private GameObject _optionMenuFB;
    [SerializeField] private GameObject _endMenuFB;

    [SerializeField] private bool isPaused;
    private void Start()
    {
        _pauseMenuCanvas.SetActive(false);
        _deathMenuCanvas.SetActive(false);
        _optionMenuCanvas.SetActive(false);
        _endMenuCanvas.SetActive(false);
        _exitMenuCanvas.SetActive(false);

        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.pause && !isPaused)
        {
            isPaused = true;
            _pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            var masterBus = RuntimeManager.GetBus("bus:/");
            masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            EventSystem.current.SetSelectedGameObject(_pauseMenuFB);
        }
    }
    public void DeathScreen()
    {
        _deathMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        var masterBus = RuntimeManager.GetBus("bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        EventSystem.current.SetSelectedGameObject(_deathMenuFB);
    }
    public void EndGameScreen()
    {
        OpenEndGameMenu();
        Time.timeScale = 0f;
    }
    public void OpenOptionMenu()
    {
        _pauseMenuCanvas.SetActive(false);
        _optionMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_optionMenuFB);
    }
    public void OpenEndGameMenu()
    {
        _input.aim = false;
        _input.shoot = false;
        _pauseMenuCanvas.SetActive(false);
        _deathMenuCanvas.SetActive(false);
        _optionMenuCanvas.SetActive(false);
        _endMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_endMenuFB);
    }
    public void OnOptionPress()
    {
        OpenOptionMenuHandle();
    }

    private void OpenOptionMenuHandle()
    {
        _pauseMenuCanvas.SetActive(true);
        _deathMenuCanvas.SetActive(false);
        _optionMenuCanvas.SetActive(false);
    }

    #region Button logic

    public void BackButton()
    {
        _pauseMenuCanvas.SetActive(true);
        _optionMenuCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_pauseMenuFB);
    }
    public void ResumeButton()
    {
        isPaused = false;
        _input.pause = false;
        _pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RestartButton(string leveltoLoad)
    {
        SceneManager.LoadScene(leveltoLoad);
        Time.timeScale = 1f;

        var masterBus = RuntimeManager.GetBus("bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void ExitButton()
    {
        _exitMenuCanvas.SetActive(true);
        _pauseMenuCanvas.SetActive(false);
    }
    #endregion
}
