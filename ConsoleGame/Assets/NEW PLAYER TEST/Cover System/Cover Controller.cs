using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoverController : MonoBehaviour
{
    [SerializeField] Vector3 RayOffset = new Vector3(0, 0.8f, 0);
    [SerializeField] float maxDistanceFromCover;
    [SerializeField] LayerMask coverLayerMask;
    [SerializeField] Transform highCoverDetectionTransform;

    bool highCover;

    KeyCode coverKey = KeyCode.JoystickButton9;

    private void Update()
    {
        TakeCover();
        CoverTypeDetecter();

        Debug.DrawRay(transform.position + RayOffset, transform.forward * maxDistanceFromCover, 
            (IsNearCover()) ? Color.green : Color.white);
        Debug.DrawRay(highCoverDetectionTransform.position, highCoverDetectionTransform.forward * maxDistanceFromCover, 
            (highCover) ? Color.blue : Color.white);
    }

    private bool IsNearCover()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position + RayOffset, transform.forward, out hitInfo, maxDistanceFromCover, coverLayerMask))
        {
            return true;
        } 
        else
        {
            return false;
        };
    }

    private void TakeCover()
    {
        if (Input.GetKeyDown(coverKey))
        {
            if (IsNearCover() && highCover)
            {
                
            }
            else if (IsNearCover() && !highCover)
            {
                
            }
        }
    }

    private void CoverTypeDetecter()
    {
        if (Physics.Raycast(highCoverDetectionTransform.position, highCoverDetectionTransform.forward, maxDistanceFromCover, 
            coverLayerMask))
        {
            highCover = true;
        } 
        else
        {
            highCover = false;
        }
    }
}
