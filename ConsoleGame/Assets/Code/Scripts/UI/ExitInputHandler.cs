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
    void Update()
    {
        // Check for key presses
        if (playerControls.triangle || Input.GetKeyDown(KeyCode.Y))
        {
            sounds.PlayOnClick();
            Application.Quit();
        }

        if (playerControls.cross || Input.GetKeyDown(KeyCode.N))
        {
            // Make the canvas inactive when 'Space' is pressed
            sounds.PlayOnClick();
            exitWindow.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }
}
