using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Outline outlinedObject = null;

    GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        Ray ray = new Ray(player.transform.position + new Vector3(0, 2f, 0), player.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if (objectHitByRaycast.GetComponent<Outline>())
            {
                outlinedObject = objectHitByRaycast.gameObject.GetComponent<Outline>();
                outlinedObject.GetComponent<Outline>().enabled = true;
            }
            else
            {
                if (outlinedObject)
                {
                    outlinedObject.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}