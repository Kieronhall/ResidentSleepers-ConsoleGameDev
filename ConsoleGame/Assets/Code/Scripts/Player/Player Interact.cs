using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.UI;
using TMPro;    

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float distance = 2f;
    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;
    private PlayerControls playerControls;
    private float timer;
    public GameObject playerSlider;
    public TextMeshProUGUI text;
    public TextMeshProUGUI progressText;

    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        playerControls = GetComponent<PlayerControls>();
        playerSlider.GetComponent<Slider>().value = 0;
        playerSlider.SetActive(false);
    }

    void Update()
    {
        timer = Mathf.Clamp(timer, 0, 10);
        playerSlider.GetComponent<Slider>().value = timer;
        progressText.text = playerSlider.GetComponent<Slider>().value.ToString() + ("0%");

        if (timer > 1)
        {
            playerSlider.SetActive(true);
        }
        else if (timer < 1)
        {
            playerSlider.SetActive(false);
        }

        playerUI.UpdateText(string.Empty);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                if (interactable.gameObject.name == "Terminal")
                {
                    if (playerControls.interact)
                    {
                        timer = Mathf.Lerp(timer, 10, 0.001f);
                        text.text = ("Extracting Drive:");
                        if (playerSlider.GetComponent<Slider>().value == 10)
                        {
                            interactable.BaseInteract();
                        }
                    }
                    else
                    {
                        if (timer > 0)
                        {
                            text.text = ("Repairing Drive:");
                            timer = Mathf.Lerp(timer, 0, 0.001f);
                        }
                    }
                }
            }
        }
    }
}