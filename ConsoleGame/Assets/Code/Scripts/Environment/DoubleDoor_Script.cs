using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor_Script : MonoBehaviour
{
    public Transform Door_1;
    public Transform Door_2;
    public float doorSpeed;
    public bool doorOpen = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && doorOpen == false) // Change KeyCode.Space to the desired key
        {
            Debug.Log("OpenDoor!");
            StartCoroutine(OpenDoors());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && doorOpen == true) // Change KeyCode.Space to the desired key
        {
            Debug.Log("OpenDoor!");
            StartCoroutine(CloseDoors());
        }
    }
    private IEnumerator OpenDoors()
    {
        float initialX_1 = Door_1.localPosition.x;
        float initialX_2 = Door_2.localPosition.x;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            //Door 1
            float newX_1 = Mathf.Lerp(initialX_1, -0.9f, elapsedTime);
            Door_1.localPosition = new Vector3(newX_1, Door_1.localPosition.y, Door_1.localPosition.z);
            //Door 2
            float newX_2 = Mathf.Lerp(initialX_2, 0.9f, elapsedTime);
            Door_2.localPosition = new Vector3(newX_2, Door_2.localPosition.y, Door_2.localPosition.z);

            elapsedTime += Time.deltaTime * doorSpeed;

            yield return null;
        }
        doorOpen = true;
    }
    private IEnumerator CloseDoors()
    {
        float initialX_1 = Door_1.localPosition.x;
        float initialX_2 = Door_2.localPosition.x;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            //Door 1
            float newX_1 = Mathf.Lerp(initialX_1, 0f, elapsedTime);
            Door_1.localPosition = new Vector3(newX_1, Door_1.localPosition.y, Door_1.localPosition.z);
            //Door 2
            float newX_2 = Mathf.Lerp(initialX_2, 0f, elapsedTime);
            Door_2.localPosition = new Vector3(newX_2, Door_2.localPosition.y, Door_2.localPosition.z);

            elapsedTime += Time.deltaTime * doorSpeed;

            yield return null;
        }
        doorOpen = false;
    }
}
