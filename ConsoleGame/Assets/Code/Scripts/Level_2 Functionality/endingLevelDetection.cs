using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingLevelDetection : MonoBehaviour
{
    
    public endingLevelCamera endinglevelcamera;
    public ASyncLoader loaderScene;
    public GameObject helicopter;
    public float speed = 5.0f;
    bool helicopterMoving = false;

    [Header("Next Level Name")]
    public string NameLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endinglevelcamera.switchCamera();
            //helicopter.transform.Translate(Vector3.up * speed * Time.deltaTime);
            helicopterMoving = true;
            StartCoroutine(DelayedSceneLoad(8));
        }
    }

    void Update()
    {

        if (helicopterMoving)
        {
            helicopter.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
       
    }

    IEnumerator DelayedSceneLoad(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        loaderScene.LoadLevelButton(NameLevel);
        Debug.Log("NEXT SCENE LOADING");

    }

}
