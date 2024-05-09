using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class endingLevelDetection : MonoBehaviour
{
    
    public endingLevelCamera endinglevelcamera;
    public ASyncLoader loader;
    public GameObject helicopter;
    public float speed = 5.0f;
    bool helicopterMoving = false;
    public string sceneName;

    public GameObject player;

    private FMOD.Studio.EventInstance helicoptersound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endinglevelcamera.switchCamera();
            //helicopter.transform.Translate(Vector3.up * speed * Time.deltaTime);
            helicopterMoving = true;
            player.SetActive(false);
            PlayHelicopterSound();
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
        var masterBus = RuntimeManager.GetBus("bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        loader.LoadLevelButton(sceneName);
    }  

    public void PlayHelicopterSound()
    {
        helicoptersound = FMODUnity.RuntimeManager.CreateInstance("event:/Kieron/helicopterSound");
        helicoptersound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        helicoptersound.start();
        helicoptersound.release();
    }

}
