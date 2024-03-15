using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public bool activateElevator = false; 
    public float height = 10.0f; // height the elevator to move
    public float speed = 2.0f; // Speed of the elevator movement

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool moving = false; 

    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
    }

    void Update()
    {
        if (activateElevator && !moving)
        {
            StartCoroutine(MoveElevator());
        }
    }

    System.Collections.IEnumerator MoveElevator()
    {
        moving = true; 

        // Move up
        while (transform.position.y < endPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Wait at the top for a moment 
        yield return new WaitForSeconds(5.0f);

        // Move down
        while (transform.position.y > startPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            yield return null;
        }

        moving = false; // Indicate that the elevator has stopped moving
        activateElevator = false; // Reset the elevator activation so it can be triggered again
    }
}
