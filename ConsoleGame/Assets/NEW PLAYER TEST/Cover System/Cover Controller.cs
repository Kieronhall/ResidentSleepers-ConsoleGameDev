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
        //if (inHighCover)
        //{
        //    RestrictPlayerMovement(highCoverPos, coverCollider.bounds.size);
        //}
        //if (inLowCover)
        //{
        //    RestrictPlayerMovement(lowCoverPos, coverCollider.bounds.size);
        //}

        if (inHighCover && coverCollider != null)
        {
            Debug.Log("High Cover Area: " + GetCoverArea());
            Debug.Log("High Cover Position: " + highCoverPos);
            Debug.Log("High Cover Collider Size: " + coverCollider.bounds.size);

            RestrictPlayerMovement(highCoverPos, coverCollider.bounds.size);
        }
        if (inLowCover && coverCollider != null)
        {
            Debug.Log("Low Cover Area: " + GetCoverArea());
            Debug.Log("Low Cover Position: " + lowCoverPos);
            Debug.Log("Low Cover Collider Size: " + coverCollider.bounds.size);

            RestrictPlayerMovement(lowCoverPos, coverCollider.bounds.size);
        }
    }

    private void IsNearLowCover()
    {
        RaycastHit lowHitInfo;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.8f, 0), transform.forward, out lowHitInfo, maxDistanceFromCover, coverLayerMask))
        {
            lowCoverPos = lowHitInfo.point + lowHitInfo.normal * 0.25f;
            coverCollider = lowHitInfo.collider;
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
            coverCollider = highHitInfo.collider;
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
        float coverArea = bounds.size.x * bounds.size.z;
        Debug.Log("Cover Area: " + coverArea);
        return coverArea;
    }

    private void TakeCover()
    {
        if (Input.GetKeyDown(coverKey) && !inCover || Input.GetKeyDown(KeyCode.C) && !inCover)
        {
            if (highCover)
            {
                inHighCover = true;
                MoveToCover(highCoverPos);
                StartCoroutine(CoverTimeout());
            }
            else if (!highCover)
            {
                inLowCover = true;
                MoveToCover(lowCoverPos);
                StartCoroutine(CoverTimeout());
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

    private void MoveToCover(Vector3 coverPosition)
    {
        transform.DOMove(new Vector3(coverPosition.x, transform.position.y, coverPosition.z), 0.2f);
        playerController.characterController.center = new Vector3(0f, 0.87f, -0.24f);
    }

    private void RestrictPlayerMovement(Vector3 coverPosition, Vector3 coverSize)
    {
        float minX = coverPosition.x - coverSize.x / 2;
        float maxX = coverPosition.x + coverSize.x / 2;
        float minZ = coverPosition.z - coverSize.z / 2;
        float maxZ = coverPosition.z + coverSize.z / 2;

        Vector3 newPosition = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minZ, maxZ)
        );

        transform.position = newPosition;
    }
}