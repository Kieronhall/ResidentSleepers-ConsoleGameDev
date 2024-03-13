using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripwire_Script : MonoBehaviour
{
    public Transform startPoint; // The starting point of the tripwire
    public Transform endPoint; // The ending point of the tripwire
    public float lineWidth = 0.1f; // Width of the line
    public Color lineColor = Color.red; // Color of the line
    public LayerMask detectionLayer; // Layer mask to detect objects

    [Tooltip("Choose which type of Laser")]
    public enum Type
    {      
        Static_Laser, 
        Moving_Laser,      
        Random_Timer_Laser,   
        Cycle_Laser
    }
    [Tooltip("Choose which type of Laser")]
    public Type LaserType = Type.Static_Laser;


    private LineRenderer lineRenderer;




    //MOVING LASER VARIABLES
    [Header("Moving Laser Variables")]
    public float moveDistance = 2f;
    public float moveSpeed = 1f;
    public bool moveUp = true;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        if (startPoint != null && endPoint != null)
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }
        switch (LaserType)
        {
            case Type.Static_Laser:
                break;
            case Type.Moving_Laser:
                StartCoroutine(MoveUpDown());
                break;
            case Type.Random_Timer_Laser:
                break;
            case Type.Cycle_Laser:
                break;

        }
    }

    void Update()
    {
        CheckTripwire();

        if (startPoint != null && endPoint != null)
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }
    }


    void CheckTripwire()
    {
        if (Physics.Linecast(startPoint.position, endPoint.position, out RaycastHit hit, detectionLayer))
        {
            Debug.Log("Tripwire triggered by: " + hit.collider.gameObject.name);
            // Do something when the tripwire is triggered, for example:
            //hit.collider.gameObject.SetActive(false);

            PlayerPrefs.SetInt("Alarm", 1);
            PlayerPrefs.Save();

        }
    }

    //MOVING LASER FUNCTIONS
    IEnumerator MoveUpDown()
    {
        while (true)
        {
            //orginal transform
            Vector3 Laser_1_StartPosition = startPoint.localPosition;
            Vector3 Laser_2_StartPosition = endPoint.localPosition;
            //create target variables
            Vector3 Laser_1_TargetPosition;
            Vector3 Laser_2_TargetPosition;

            if (moveUp)
            {
                //final position for movement
                Laser_1_TargetPosition = Laser_1_StartPosition + Vector3.up * moveDistance;
                //final position for movement
                Laser_2_TargetPosition = Laser_2_StartPosition + Vector3.up * moveDistance;
            }
            else
            {
                //final position for movement
                Laser_1_TargetPosition = Laser_1_StartPosition - Vector3.up * moveDistance;
                //final position for movement
                Laser_2_TargetPosition = Laser_2_StartPosition - Vector3.up * moveDistance;
            }

            //Debug.Log(isMovingUp.ToString() + Laser_1_TargetPosition + Laser_2_TargetPosition);
            while (Vector3.Distance(startPoint.localPosition, Laser_1_TargetPosition) > 0.01f && Vector3.Distance(endPoint.localPosition, Laser_2_TargetPosition) > 0.01f)
            {
                startPoint.localPosition = Vector3.MoveTowards(startPoint.localPosition, Laser_1_TargetPosition, moveSpeed * Time.deltaTime);
                endPoint.localPosition = Vector3.MoveTowards(endPoint.localPosition, Laser_2_TargetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            moveUp = !moveUp;

            yield return new WaitForSeconds(1f); // Pause at the top/bottom position before moving back
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPoint.position, 0.1f);
        Gizmos.DrawWireSphere(endPoint.position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
