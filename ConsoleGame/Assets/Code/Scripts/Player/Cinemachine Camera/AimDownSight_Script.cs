using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight_Script : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject aimCamera;
    public Transform stockMainCamera;
    public bool Hidden=false;

    public bool AimDownSight = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Hidden)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                AimDownSight = !AimDownSight;
            }

            if (AimDownSight && !aimCamera.activeInHierarchy)
            {
                ZoomIn();


            }
            else if (!AimDownSight && !mainCamera.activeInHierarchy)
            {
                ZoomOut();

            }
        }
    }

    public void ZoomIn()
    {
        mainCamera.SetActive(false);
        aimCamera.SetActive(true);
    }
    public void ZoomOut()
    {
        mainCamera.SetActive(true);
        aimCamera.SetActive(false);
    }
    public void ToggleHideCamera()
    {
        Hidden=!Hidden;
        Debug.Log("HIDING");
        mainCamera.SetActive(false);
        aimCamera.SetActive(false);
    }
}
