using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ThirdPerson;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuCanvas;
    [SerializeField] private GameObject _deathMenuCanvas;

    public PlayerControls _input;

    private bool isPaused;
    private void Start()
    {
        _pauseMenuCanvas.SetActive(false);
        _deathMenuCanvas.SetActive(false);
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
        //if (InputManager.instance._pauseMenuInput)
        //{
        //    if (!isPaused)
        //    {
        //        Pause();
        //    }
        //    else
        //    {
        //        UnPause();
        //    }
        //}
    }
    private void Pause()
    {
            isPaused = true;
            Time.timeScale = 0f;
            OpenPauseMenu();
    }
    private void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        CloseAllMenus();
    }

    private void CloseAllMenus()
    {
        _pauseMenuCanvas.SetActive(false);
        _deathMenuCanvas.SetActive(false);
    }
    private void OpenPauseMenu()
    {
        _pauseMenuCanvas.SetActive(true);
        _deathMenuCanvas.SetActive(false);
    }
}
