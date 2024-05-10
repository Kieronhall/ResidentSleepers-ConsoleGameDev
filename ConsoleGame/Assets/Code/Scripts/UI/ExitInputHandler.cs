using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.UI;

public class ExitInputHandler : MonoBehaviour
{
    public PlayerControls playerControls;
    public PlayButtonSounds sounds;
    public GameObject exitWindow;
    public GameObject mainMenu;

    private void Start()
    {
        Time.timeScale = 0f;
    }
    void Update()
    {
        // Check for key presses
        if (playerControls.crouch || Input.GetKeyDown(KeyCode.Y))
        {
            sounds.PlayOnClick();
            Application.Quit();
        }
        else if (playerControls.cross || Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("pressing cross");            // Make the canvas inactive when 'Space' is pressed
            sounds.PlayOnClick();
            exitWindow.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }
}
