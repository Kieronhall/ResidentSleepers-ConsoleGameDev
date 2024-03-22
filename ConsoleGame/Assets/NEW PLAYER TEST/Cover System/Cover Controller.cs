using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class CoverController : MonoBehaviour
{
    [SerializeField] Vector3 RayOffset = new Vector3(0, 0.8f, 0);
    [SerializeField] float maxDistanceFromCover;
    [SerializeField] LayerMask coverLayerMask;
    [SerializeField] Transform highCoverDetectionTransform;

    Animator animator;

    PlayerController playerController;

    bool inCover;
    bool highCover;
    bool inLowCover;
    bool inHighCover;

    Vector3 lowCoverPos;
    Vector3 highCoverPos;

    KeyCode coverKey = KeyCode.JoystickButton9;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        TakeCover();
        LeaveCover();
        CoverTypeDetecter();

        Debug.DrawRay(transform.position + RayOffset, transform.forward * maxDistanceFromCover, 
            (IsNearCover()) ? Color.green : Color.white);
        Debug.DrawRay(highCoverDetectionTransform.position, highCoverDetectionTransform.forward * maxDistanceFromCover, 
            (highCover) ? Color.blue : Color.white);

        animator.SetBool("inHighCover", inHighCover);
        animator.SetBool("inLowCover", inLowCover);
    }

    private bool IsNearCover()
    {
        RaycastHit lowHitInfo;

        if (Physics.Raycast(transform.position + RayOffset, transform.forward, out lowHitInfo, maxDistanceFromCover, coverLayerMask))
        {
            lowCoverPos = lowHitInfo.point + lowHitInfo.normal * 0.005f;
            return true;
        } 
        else
        {
            return false;
        };
    }

    private void TakeCover()
    {
        if (Input.GetKeyDown(coverKey) && !inCover || Input.GetKeyDown(KeyCode.C) && !inCover)
        {
            if (IsNearCover() && highCover)
            {
                inHighCover = true;
                transform.DOMove(new Vector3(highCoverPos.x, transform.position.y, highCoverPos.z), 0.2f);
                playerController.characterController.center = new Vector3(0f, 0.87f, -0.24f);
                StartCoroutine(CoverTimeout());
                Debug.Log("High Cover");
            }
            else if (IsNearCover() && !highCover)
            {
                inLowCover = true;
                transform.DOMove(new Vector3(lowCoverPos.x, transform.position.y, lowCoverPos.z), 0.2f);
                playerController.characterController.center = new Vector3(0f, 0.87f, -0.24f);
                StartCoroutine(CoverTimeout());
                Debug.Log("Low Cover");
            }
        }
    }

    private void LeaveCover()
    {
        if (Input.GetKeyDown(coverKey) && inCover || Input.GetKeyDown(KeyCode.C) && inCover)
        {
            if (highCover)
            {
                inHighCover = false;
                playerController.characterController.center = new Vector3(0f, 0.87f, 0.1f);
                inCover = false;
                Debug.Log("Left High Cover");
            }
            else if (!highCover)
            {
                inLowCover = false;
                playerController.characterController.center = new Vector3(0f, 0.87f, 0.1f);
                inCover = false;
                Debug.Log("Left Low Cover");
            }
        }
    }

    private void CoverTypeDetecter()
    {
        RaycastHit highHitInfo;

        if (Physics.Raycast(highCoverDetectionTransform.position, highCoverDetectionTransform.forward, out highHitInfo, maxDistanceFromCover, 
            coverLayerMask))
        {
            highCoverPos = highHitInfo.point + highHitInfo.normal * 0.005f;
            highCover = true;
        } 
        else
        {
            highCover = false;
        }
    }

    IEnumerator CoverTimeout()
    {
        yield return new WaitForSeconds(0.5f);
        inCover = true;
    }
}
