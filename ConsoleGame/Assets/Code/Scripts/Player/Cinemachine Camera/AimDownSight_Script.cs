using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight_Script : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject aimCamera;

    public bool AimDownSight = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            AimDownSight = !AimDownSight;
        }

        if (AimDownSight && !aimCamera.activeInHierarchy)
        {
            mainCamera.SetActive(false);
            aimCamera.SetActive(true);
           
        }
        else if (!AimDownSight && !mainCamera.activeInHierarchy)
        {
            mainCamera.SetActive(true);
            aimCamera.SetActive(false);

        }
    }
}
