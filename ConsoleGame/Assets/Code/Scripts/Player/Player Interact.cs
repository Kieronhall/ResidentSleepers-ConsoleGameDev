using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float distance = 2f;
    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;
    private PlayerControls playerControls;

    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        playerControls = GetComponent<PlayerControls>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                if (playerControls.interact)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}