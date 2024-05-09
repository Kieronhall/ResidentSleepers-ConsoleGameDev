
using ThirdPerson;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using FMODUnity;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuCanvas;
    [SerializeField] private GameObject _deathMenuCanvas;
    [SerializeField] private GameObject _optionMenuCanvas;
    [SerializeField] private GameObject _endMenuCanvas;

    public PlayerControls _input;
    public ThirdPersonAim _controlinput;

    [SerializeField] private GameObject _pauseMenuFB;
    [SerializeField] private GameObject _deathMenuFB;
    [SerializeField] private GameObject _optionMenuFB;
    [SerializeField] private GameObject _endMenuFB;

    private bool isPaused;
    private void Start()
    {
        _pauseMenuCanvas.SetActive(false);
        _deathMenuCanvas.SetActive(false);
        _optionMenuCanvas.SetActive(false);
        _endMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.pause)
        {
            if (!isPaused)
            {
                Pause();
                _input.pause = false;
            }
            else
            {
                UnPause();
                _input.pause = false;
            }
        }
    }
    public void Pause()
    {
        isPaused = true;
        _controlinput.enabled = false;
        Time.timeScale = 0f;
        OpenPauseMenu();
    }
    private void UnPause()
    {
        isPaused = false;
        _controlinput.enabled =true;
        Time.timeScale = 1f;
        CloseAllMenus();
    }

    public void DeathScreen()
    {
        OpenDeathMenu();
        Time.timeScale = 0f;
    }

    public void EndGameScreen()
    {
        OpenEndGameMenu();
        Time.timeScale = 0f;
    }

    private void CloseAllMenus()
    {
        _pauseMenuCanvas.SetActive(false);
        _deathMenuCanvas.SetActive(false);
        _optionMenuCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

    }
    private void OpenPauseMenu()
    {
        _pauseMenuCanvas.SetActive(true);
        _deathMenuCanvas.SetActive(false);
        _optionMenuCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_pauseMenuFB);
    }

    private void OpenDeathMenu()
    {
        _pauseMenuCanvas.SetActive(false);
        _deathMenuCanvas.SetActive(true);
        _optionMenuCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_deathMenuFB);
    }

    public void OpenOptionMenu()
    {
        _pauseMenuCanvas.SetActive(false);
        _deathMenuCanvas.SetActive(false);
        _optionMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_optionMenuFB);
    }

    public void OpenEndGameMenu()
    {
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
        Pause();
        _optionMenuCanvas.SetActive(false);
    }
    public void ResumeButton()
    {
        UnPause();
    }

    public void RestartButton(string leveltoLoad)
    {
        //UnPause();
        SceneManager.LoadScene(leveltoLoad);
        Time.timeScale = 1f;

        var masterBus = RuntimeManager.GetBus("bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    #endregion
}
