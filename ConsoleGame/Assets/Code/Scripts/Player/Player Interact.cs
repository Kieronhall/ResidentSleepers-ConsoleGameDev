using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ThirdPerson
{
    public class PlayerInteract : MonoBehaviour
    {
        // Layer mask for raycasting
        [SerializeField] private LayerMask mask;
        // Reference to player UI
        private PlayerUI playerUI;
        // Reference to player controls
        private PlayerControls playerControls;
        // Timer for interaction progress
        private float timer;
        // UI elements for interaction progress
        public GameObject playerSlider;
        public TextMeshProUGUI text;
        public TextMeshProUGUI progressText;

        void Start()
        {
            // Initialization
            playerUI = GetComponent<PlayerUI>();
            playerControls = GetComponent<PlayerControls>();

            // Set initial state of interaction slider
            playerSlider.GetComponent<Slider>().value = 0;
            playerSlider.SetActive(false);
        }

        void Update()
        {
            // Clamp timer value between 0 and 10
            timer = Mathf.Clamp(timer, 0, 10);
            // Update slider value and progress text
            playerSlider.GetComponent<Slider>().value = timer;
            progressText.text = playerSlider.GetComponent<Slider>().value.ToString() + ("0%");

            // Activate slider if timer is greater than 1, otherwise deactivate
            if (timer > 1)
            {
                playerSlider.SetActive(true);
            }
            else if (timer < 1)
            {
                playerSlider.SetActive(false);
            }

            // Update player UI text
            playerUI.UpdateText(string.Empty);

            // Create ray for raycasting
            Ray ray = new Ray(transform.position + new Vector3(0, 1.5f, 0), transform.forward);
            RaycastHit hitInfo;

            // Perform raycast
            if (Physics.Raycast(ray, out hitInfo, 1, mask))
            {
                // Check if the object hit is interactable
                if (hitInfo.collider.GetComponent<Interactable>() != null)
                {
                    Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                    // Update player UI text with prompt message from interactable object
                    playerUI.UpdateText(interactable.promptMessage);

                    // Handle specific interactions based on the interactable object
                    if (interactable.gameObject.name == "Terminal")
                    {
                        // Handle interaction with terminal
                        if (playerControls.interact)
                        {
                            // Progress timer towards completion
                            timer = Mathf.Lerp(timer, 10, 0.001f);
                            text.text = ("Extracting Drive:");
                            playerSlider.SetActive(true);
                            // Set alarm status in player preferences if it's not set
                            if (PlayerPrefs.GetInt("Alarm") == 0)
                            {
                                PlayerPrefs.SetInt("Alarm", 1);
                            }
                            // If interaction is complete, execute base interact method
                            if (playerSlider.GetComponent<Slider>().value == 10)
                            {
                                interactable.BaseInteract();
                            }
                        }
                        else
                        {
                            // If player stops interaction, reset timer and deactivate slider
                            if (timer > 0)
                            {
                                text.text = ("Repairing Drive:");
                                timer = 0;
                                playerSlider.SetActive(false);
                            }
                        }
                    }

                    // Handle interaction with power circuit
                    if (interactable.gameObject.name == "Power Circuit")
                    {
                        // Execute base interact method when interacting
                        if (playerControls.interact)
                        {
                            interactable.BaseInteract();
                        }
                    }
                }
            }
        }
    } 
}