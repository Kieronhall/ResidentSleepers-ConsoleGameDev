using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class CoverController : MonoBehaviour
{
    [SerializeField] float maxDistanceFromCover;
    [SerializeField] LayerMask coverLayerMask;
    [SerializeField] Transform highCoverDetectionTransform;

    Animator animator;

    PlayerController playerController;

    Collider coverCollider;

    public bool inCover;
    bool highCover;
    bool lowCover;
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
        IsNearLowCover();
        IsNearHighCover();

        Debug.DrawRay(transform.position + new Vector3(0, 0.8f, 0), transform.forward * maxDistanceFromCover, (lowCover) ? Color.green : Color.white);
        Debug.DrawRay(transform.position + new Vector3(0, 1.5f, 0), transform.forward * maxDistanceFromCover, (highCover) ? Color.blue : Color.white);

        animator.SetBool("inHighCover", inHighCover);
        animator.SetBool("inLowCover", inLowCover);
    }

    private void FixedUpdate()
    {
        if (inHighCover)
        {
            float minX = highCoverPos.x - coverCollider.bounds.size.x / 2;
            float maxX = highCoverPos.x + coverCollider.bounds.size.x / 2;
            float minZ = highCoverPos.z - coverCollider.bounds.size.z / 2;
            float maxZ = highCoverPos.z + coverCollider.bounds.size.z / 2;

            Vector3 newPosition = new Vector3(
                        Mathf.Clamp(transform.position.x, minX, maxX),
                        transform.position.y,
                        Mathf.Clamp(transform.position.z, minZ, maxZ)
                    );
            
            transform.position = newPosition;
        }
        if (inLowCover)
        {
            float minX = lowCoverPos.x - coverCollider.bounds.size.x / 2;
            float maxX = lowCoverPos.x + coverCollider.bounds.size.x / 2;
            float minZ = lowCoverPos.z - coverCollider.bounds.size.z / 2;
            float maxZ = lowCoverPos.z + coverCollider.bounds.size.z / 2;

            Vector3 newPosition = new Vector3(
                        Mathf.Clamp(transform.position.x, minX, maxX),
                        transform.position.y,
                        Mathf.Clamp(transform.position.z, minZ, maxZ)
                    );
            
            transform.position = newPosition;
        }
    }

    private void IsNearLowCover()
    {
        RaycastHit lowHitInfo;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.8f, 0), transform.forward, out lowHitInfo, maxDistanceFromCover, coverLayerMask))
        {
            lowCoverPos = lowHitInfo.point + lowHitInfo.normal * 0.25f;
            lowCover = true;
        } 
        else
        {
            lowCover = false;
        }
    }

    private void IsNearHighCover()
    {
        RaycastHit highHitInfo;

        if (Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), transform.forward, out highHitInfo, maxDistanceFromCover, coverLayerMask))
        {
            highCoverPos = highHitInfo.point + highHitInfo.normal * 0.25f;
            highCover = true;
        }
        else
        {
            highCover = false;
        }
    }

    private float GetCoverArea()
    {
        Bounds bounds = coverCollider.bounds;
        return bounds.size.x * bounds.size.z;
    }

    private void TakeCover()
    {
        if (Input.GetKeyDown(coverKey) && !inCover || Input.GetKeyDown(KeyCode.C) && !inCover)
        {
            if (highCover)
            {
                inHighCover = true;
                transform.DOMove(new Vector3(highCoverPos.x, transform.position.y, highCoverPos.z), 0.2f);
                playerController.characterController.center = new Vector3(0f, 0.87f, -0.24f);
                StartCoroutine(CoverTimeout());
                Debug.Log("In High Cover");
            }
            else if (!highCover)
            {
                inLowCover = true;
                transform.DOMove(new Vector3(lowCoverPos.x, transform.position.y, lowCoverPos.z), 0.2f);
                playerController.characterController.center = new Vector3(0f, 0.87f, -0.24f);
                StartCoroutine(CoverTimeout());
                Debug.Log("In Low Cover");
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

    IEnumerator CoverTimeout()
    {
        yield return new WaitForSeconds(0.5f);
        inCover = true;
    }

    private Vector3 GetCoverSurfaceDirection(Vector3 coverPos)
    {
        return Vector3.Cross(coverPos, Vector3.up).normalized;
    }
}
