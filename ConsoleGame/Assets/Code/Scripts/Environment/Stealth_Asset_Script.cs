using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineOrbitalTransposer;
using Cinemachine;

public class Stealth_Asset_Script : MonoBehaviour
{
    public GameObject childCamera;
    public GameObject mainCamera;
    public bool Hiding=false;
    public float duration = 1f;

    public enum Type
    {
        bin,
        manhole,
        cupboard
    }
    public Type stealthObjectType = Type.bin;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Close to Hiding Place
        if (Vector3.Distance(this.transform.position, player.transform.position) <= 2 && Hiding == false)
        {
            //Input
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Stop Repeat Inputs
                Hiding = true;
                //Method 
                HideInObject();
            }
        }
        else if(Vector3.Distance(this.transform.position, player.transform.position) <= 2 && Hiding == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Hiding = false;
                ExitHiding();
            }
        }
    }
    public void HideInObject()
    {
        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = this.transform.position;

        childCamera.GetComponent<CinemachineVirtualCamera>().Priority = 11;

        StartCoroutine(ScaleOverTime());

        player.GetComponent<CharacterController>().stepOffset = 0;

    }

    IEnumerator ScaleOverTime()
    {
        Vector3 originalScale = player.transform.localScale;
        Vector3 targetScale = originalScale * 0.01f;
        float timer = 0f;

        while (timer < duration)
        {
            float scaleFactor = timer / duration;
            player.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleFactor);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure we set the scale and position to the target values exactly
        player.transform.localScale = targetScale;
    }

    public void ExitHiding()
    {
        player.transform.position = this.transform.GetChild(2).transform.position /*new Vector3(17.9222775f, -1.29528141f, -6.40649414f)*/;

        player.GetComponent<CharacterController>().enabled = true;
        childCamera.GetComponent<CinemachineVirtualCamera>().Priority = 8;

        StartCoroutine(ScaleAndMoveOverTime());
    }

    IEnumerator ScaleAndMoveOverTime()
    {
        Vector3 originalScale = player.transform.localScale;
        Vector3 targetScale = Vector3.one; // Target scale is 1 for original size
        float timer = 0f;

        while (timer < duration)
        {
            float scaleFactor = timer / duration;
            player.transform.localScale = Vector3.Lerp(Vector3.one * 0.01f, Vector3.one, scaleFactor); // Scaling back to original size
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure we set the scale to the original value exactly
        player.transform.localScale = targetScale;
    }
}

