using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using Unity.VisualScripting;
using UnityEngine;

namespace Thirdperson
{
    public class CoverController : MonoBehaviour
    {
        [SerializeField] float maxDistanceFromCover;
        [SerializeField] LayerMask coverLayerMask;

        Collider coverCollider;
        Animator animator;
        ThirdPersonController thirdPersonController;

        public bool inCover;
        bool highCover;
        bool lowCover;
        public bool inLowCover;
        public bool inHighCover;

        Vector3 lowCoverPos;
        Vector3 highCoverPos;

        KeyCode coverKey = KeyCode.JoystickButton9;

        private void Start()
        {
            animator = GetComponent<Animator>();
            thirdPersonController = GetComponent<ThirdPersonController>();
            inCover = false;
            inLowCover = false;
            inHighCover = false;
        }

        private void Update()
        {
            TakeCover();
            LeaveCover();
            IsNearLowCover();
            IsNearHighCover();

            Debug.DrawRay(transform.position + new Vector3(0, 0.8f, 0), transform.forward * maxDistanceFromCover, (lowCover) ? Color.green : Color.white);
            Debug.DrawRay(transform.position + new Vector3(0, 2f, 0), transform.forward * maxDistanceFromCover, (highCover) ? Color.blue : Color.white);

            animator.SetBool("inHighCover", inHighCover);
            animator.SetBool("inLowCover", inLowCover);
        }

        //private void FixedUpdate()
        //{
        // if (inHighCover && coverCollider != null)
        //    {
        //        Debug.Log("High Cover Area: " + GetCoverArea());
        //        Debug.Log("High Cover Position: " + highCoverPos);
        //        Debug.Log("High Cover Collider Size: " + coverCollider.bounds.size);

        //        RestrictPlayerMovement(highCoverPos, coverCollider.bounds.size);
        //    }
        //    if (inLowCover && coverCollider != null)
        //    {
        //        Debug.Log("Low Cover Area: " + GetCoverArea());
        //        Debug.Log("Low Cover Position: " + lowCoverPos);
        //        Debug.Log("Low Cover Collider Size: " + coverCollider.bounds.size);

        //        RestrictPlayerMovement(lowCoverPos, coverCollider.bounds.size);
        //    }
        //}

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

            if (Physics.Raycast(transform.position + new Vector3(0, 2f, 0), transform.forward, out highHitInfo, maxDistanceFromCover, coverLayerMask))
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
                    transform.DOMove(new Vector3(highCoverPos.x, transform.position.y, highCoverPos.z), 0.2f);
                    StartCoroutine(CoverTimeout());
                    StartCoroutine(HighCoverTimeout());
                }
                if (lowCover && !highCover)
                {
                    transform.DOMove(new Vector3(lowCoverPos.x, transform.position.y, lowCoverPos.z), 0.2f);
                    StartCoroutine(CoverTimeout());
                    StartCoroutine(LowCoverTimeout());
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
                    inCover = false;
                    thirdPersonController.characterController.center = new Vector3(0f, 0.87f, 0.1f);   
                }
                else if (!highCover)
                {
                    inLowCover = false;
                    inCover = false;
                    thirdPersonController.characterController.center = new Vector3(0f, 0.87f, 0.1f);
                }
            }
        }

        IEnumerator CoverTimeout()
        {
            yield return new WaitForSeconds(0.1f);
            inCover = true;
        }

        IEnumerator HighCoverTimeout()
        {
            yield return new WaitForSeconds(0.1f);
            inHighCover = true;
        }

        IEnumerator LowCoverTimeout()
        {
            yield return new WaitForSeconds(0.1f);
            inLowCover = true;
        }

        //private void RestrictPlayerMovement(Vector3 coverPosition, Vector3 coverSize)
        //{
        //    float minX = coverPosition.x - coverSize.x / 2;
        //    float maxX = coverPosition.x + coverSize.x / 2;
        //    float minZ = coverPosition.z - coverSize.z / 2;
        //    float maxZ = coverPosition.z + coverSize.z / 2;

        //    Vector3 newPosition = new Vector3(
        //        Mathf.Clamp(transform.position.x, minX, maxX),
        //        transform.position.y,
        //        Mathf.Clamp(transform.position.z, minZ, maxZ)
        //    );

        //    transform.position = newPosition;
        //}
    }
}