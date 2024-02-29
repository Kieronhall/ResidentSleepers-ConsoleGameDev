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

    private LineRenderer lineRenderer;

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
    }

    void Update()
    {
        CheckTripwire();   
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPoint.position, 0.1f);
        Gizmos.DrawWireSphere(endPoint.position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
