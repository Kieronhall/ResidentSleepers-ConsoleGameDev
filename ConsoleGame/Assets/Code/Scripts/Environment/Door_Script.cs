using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door_Script : MonoBehaviour
{
    private GameObject _player;
    private PlayerInput _playerInput;
    private PlayerControls _playerControls;
    public int doorNumber;
    public bool doorLocked=false;

    public enum Type
    {
        Single_Slide,
        Double_Slide,
        Single_Swing,
        Double_Swing
    }
    public Type doorType = Type.Single_Slide;

    public Transform Door_Right;
    public Transform Door_Left;
    public float doorSpeed;
    public bool doorOpen=false;


    //Light
    //public Material lightMat;
    public Light lightColour;
    public Light lightColour2;


    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerControls = _player.GetComponent<PlayerControls>();

        if (doorLocked)
        {
            //lightMat.color = new Color(256f, 0f, 0f);
            lightColour.color = new Color(256f, 0f, 0f);
            lightColour2.color = new Color(256f, 0f, 0f);
        }
        else
        {
            //lightMat.color = new Color(0f, 256f, 0f);
            lightColour.color = new Color(0f, 256f, 0f);
            lightColour2.color = new Color(0f, 256f, 0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (doorLocked)
        {
            //lightMat.color = new Color(256f, 0f, 0f);
            lightColour.color = new Color(256f, 0f, 0f);
            lightColour2.color = new Color(256f, 0f, 0f);
        }
        else
        {
            //lightMat.color = new Color(0f, 256f, 0f);
            lightColour.color = new Color(0f, 256f, 0f);
            lightColour2.color = new Color(0f, 256f, 0f);
        }

        if (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 5f)
        {
            Debug.Log("PLAYER IN CLOSE PROXIMITY OF Door:" + doorNumber);
        }
        //if(doorOpen == true && Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 5f)
        //{
        //    Debug.Log("CloseDoors!");
        //    StartCoroutine(CloseDoors());
        //}

        //if (Input.GetKeyDown(KeyCode.Space) && doorOpen == false && Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)<=2f) // Change KeyCode.Space to the desired key
        //{
        //    Debug.Log("OpenDoor!");
        //    StartCoroutine(OpenDoors());
        //}
        //else if(Input.GetKeyDown(KeyCode.Space) && doorOpen == true) // Change KeyCode.Space to the desired key
        //{
        //    Debug.Log("CloseDoors!");
        //    StartCoroutine(CloseDoors());
        //}
        if (!doorLocked)
        {
            if (doorOpen == true && Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 5f)
            {
                Debug.Log("CloseDoors!");
                StartCoroutine(CloseDoors());
            }

            if (_playerControls.interact /*&& Input.GetKeyDown(KeyCode.Space)*/ && doorOpen == false && Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 2f) // Change KeyCode.Space to the desired key
            {
                Debug.Log("OpenDoor!");
                _playerControls.interact = false;
                StartCoroutine(OpenDoors());
            }
            else if (_playerControls.interact /*&& Input.GetKeyDown(KeyCode.Space)*/ && doorOpen == true) // Change KeyCode.Space to the desired key
            {
                Debug.Log("CloseDoors!");
                _playerControls.interact = false;
                StartCoroutine(CloseDoors());
            }
    }
}
    private IEnumerator OpenDoors()
    {   
        //Right Door
        float initialX_1 = Door_Right.localPosition.x;
        //Left Door
        float initialX_2 = Door_Left.localPosition.x;

        switch (doorType)
        {
            case Type.Single_Slide:
                float elapsedTime = 0f;
                while (elapsedTime < 1f)
                {
                    //Door Right
                    float newX_1 = Mathf.Lerp(initialX_1, -0.66f, elapsedTime);
                    Door_Right.localPosition = new Vector3(newX_1, Door_Right.localPosition.y, Door_Right.localPosition.z);
                    //Door Left
                    float newX_2 = Mathf.Lerp(initialX_2, 0.66f, elapsedTime);
                    Door_Left.localPosition = new Vector3(newX_2, Door_Left.localPosition.y, Door_Left.localPosition.z);
                    elapsedTime += Time.deltaTime * doorSpeed;
                    yield return null;
                }
                break;
            case Type.Double_Slide:              
                elapsedTime = 0f;
                while (elapsedTime < 1f)
                {
                    //Door 1
                    float newX_1 = Mathf.Lerp(initialX_1, -0.9f, elapsedTime);
                    Door_Right.localPosition = new Vector3(newX_1, Door_Right.localPosition.y, Door_Right.localPosition.z);
                    //Door 2
                    float newX_2 = Mathf.Lerp(initialX_2, 0.9f, elapsedTime);
                    Door_Left.localPosition = new Vector3(newX_2, Door_Left.localPosition.y, Door_Left.localPosition.z);
                    elapsedTime += Time.deltaTime * doorSpeed;
                    yield return null;
                }
                break;
            case Type.Single_Swing:
                break;
            case Type.Double_Swing:
                break;
        }
        
        doorOpen = true;

    }
    private IEnumerator CloseDoors()
    {
        //Right Door
        float initialX_1 = Door_Right.localPosition.x;
        //Left Door
        float initialX_2 = Door_Left.localPosition.x;

        switch (doorType)
        {
            case Type.Single_Slide:
                float elapsedTime = 0f;
                while (elapsedTime < 1f)
                {
                    //Door Right
                    float newX_1 = Mathf.Lerp(initialX_1, 0f, elapsedTime);
                    Door_Right.localPosition = new Vector3(newX_1, Door_Right.localPosition.y, Door_Right.localPosition.z);
                    //Door Left
                    float newX_2 = Mathf.Lerp(initialX_2, 0f, elapsedTime);
                    Door_Left.localPosition = new Vector3(newX_2, Door_Left.localPosition.y, Door_Left.localPosition.z);
                    elapsedTime += Time.deltaTime * doorSpeed;
                    yield return null;
                }
                break;
            case Type.Double_Slide:
                elapsedTime = 0f;
                while (elapsedTime < 1f)
                {
                    //Door 1
                    float newX_1 = Mathf.Lerp(initialX_1, 0f, elapsedTime);
                    Door_Right.localPosition = new Vector3(newX_1, Door_Right.localPosition.y, Door_Right.localPosition.z);
                    //Door 2
                    float newX_2 = Mathf.Lerp(initialX_2, 0f, elapsedTime);
                    Door_Left.localPosition = new Vector3(newX_2, Door_Left.localPosition.y, Door_Left.localPosition.z);
                    elapsedTime += Time.deltaTime * doorSpeed;
                    yield return null;
                }
                break;
            case Type.Single_Swing:
                break;
            case Type.Double_Swing:
                break;
        }

        doorOpen = false;

    }
    public void LockDoor(int _doorNumber)
    {
        //if(_doorNumber == doorNumber)
        //{
        //    doorLocked = !doorLocked;
        //}
        if (doorLocked)
        {
            doorLocked = false;
            Debug.Log("Door Unlocked!");
        }
    }
}
