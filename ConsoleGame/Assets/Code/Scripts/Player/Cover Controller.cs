using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using Unity.VisualScripting;
using UnityEngine;

namespace ThirdPerson
{
    public class CoverController : MonoBehaviour
    {
        [SerializeField] float maxDistanceFromCover;
        [SerializeField] LayerMask coverLayerMask;

        Collider coverCollider;
        Animator animator;
        ThirdPersonController thirdPersonController;
        ThirdPersonAim thirdPersonAim;

        public bool inCover;
        internal bool coverLeft;
        internal bool coverRight;
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
            thirdPersonAim = GetComponent<ThirdPersonAim>();

            inCover = false;
            inLowCover = false;
            inHighCover = false;
            coverLeft = false;
            coverRight = false;
        }

        private void Update()
        {
            IsNearLowCover();
            IsNearHighCover();

            CoverLeft();
            CoverRight();
            
            TakeCover();
            LeaveCover();

            Debug.DrawRay(transform.position + new Vector3(0, 0.8f, 0), transform.forward * maxDistanceFromCover, lowCover ? Color.green : Color.white);
            Debug.DrawRay(transform.position + new Vector3(1, 0.8f, 0), transform.forward * maxDistanceFromCover, coverLeft ? Color.green : Color.white);
            Debug.DrawRay(transform.position + new Vector3(-1, 0.8f, 0), transform.forward * maxDistanceFromCover, coverRight ? Color.green : Color.white);
            Debug.DrawRay(transform.position + new Vector3(0, 2f, 0), transform.forward * maxDistanceFromCover, highCover ? Color.blue : Color.white);

            animator.SetBool("inHighCover", inHighCover);
            animator.SetBool("inLowCover", inLowCover);
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

        private void CoverLeft()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + new Vector3(1f, 0.8f, 0), transform.forward, out hit, maxDistanceFromCover, coverLayerMask))
            {
                coverLeft = true;
            }
            else
            {
                coverLeft = false;
            }
        }

        private void CoverRight()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + new Vector3(-1f, 0.8f, 0), transform.forward, out hit, maxDistanceFromCover, coverLayerMask))
            {
                coverRight = true;
            }
            else
            {
                coverRight = false;
            }
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
                    thirdPersonAim.gunHide();
                }
                if (lowCover && !highCover)
                {
                    transform.DOMove(new Vector3(lowCoverPos.x, transform.position.y, lowCoverPos.z), 0.2f);
                    StartCoroutine(CoverTimeout());
                    StartCoroutine(LowCoverTimeout());
                    thirdPersonAim.gunHide();
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
                    thirdPersonAim.gunShow();
                }
                else if (!highCover)
                {
                    inLowCover = false;
                    inCover = false;
                    thirdPersonController.characterController.center = new Vector3(0f, 0.87f, 0.1f);
                    thirdPersonAim.gunShow();
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
    }
}