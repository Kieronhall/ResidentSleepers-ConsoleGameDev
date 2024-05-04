using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Shutters : MonoBehaviour
{
    public float lerpDistance = 1f; // Amount to move up
    public float lerpDuration = 1f; // Time taken to complete the movement
    public bool OpenShutter = false;

    private Vector3 startPos;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenShutter)
        {
            OpenShutters();
        }
    }
    public void OpenShutters()
    {
        targetPos = startPos + Vector3.up * lerpDistance;
        StartCoroutine(LerpPosition());
    }

    IEnumerator LerpPosition()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches exactly the target position
        transform.position = targetPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OpenShutter = true;
        }
    }
}
