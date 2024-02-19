using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineOrbitalTransposer;

public class Stealth_Asset_Script : MonoBehaviour
{
    public GameObject childCamera;
    public GameObject mainCamera;
    public bool Hiding=false;

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
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
       // player = GameObject.FindGameObjectWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(this.transform.position, player.transform.position) <= 2 && Hiding==false)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Hiding = true;
                HideInObject();
            }
        }
    }
    public void HideInObject()
    {
        switch (stealthObjectType)
        {
            case Type.bin:
                player.SetActive(false);
                childCamera.SetActive(true);
                mainCamera.GetComponent<AimDownSight_Script>().ToggleHideCamera();
                Debug.Log("Switch Camera to Hide");
                break;
            case Type.manhole:
               
                break;
            case Type.cupboard:
                break;
        }

    }

}
