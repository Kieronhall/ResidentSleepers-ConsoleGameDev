using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace ThirdPerson
{
    public class CoverController : MonoBehaviour
    {
        // Maximum distance from cover
        [SerializeField] float maxDistanceFromCover;
        // Layer mask for cover objects
        [SerializeField] LayerMask coverLayerMask;

        // References
        Collider coverCollider;
        Animator animator;
        ThirdPersonController thirdPersonController;
        ThirdPersonAim thirdPersonAim;

        // Cover states
        public bool inCover;
        internal bool coverLeft;
        internal bool coverRight;
        bool highCover;
        bool lowCover;
        public bool inLowCover;
        public bool inHighCover;

        // Positions for low and high cover
        Vector3 lowCoverPos;
        Vector3 highCoverPos;

        // Input key for taking cover
        KeyCode coverKey = KeyCode.JoystickButton9;

        
        private void Start()
        {
            // Initialization
            animator = GetComponent<Animator>();
            thirdPersonController = GetComponent<ThirdPersonController>();
            thirdPersonAim = GetComponent<ThirdPersonAim>();

            // Initial cover states
            inCover = false;
            inLowCover = false;
            inHighCover = false;
            coverLeft = false;
            coverRight = false;
        }

        private void Update()
        {
            // Check if near low cover
            IsNearLowCover();
            // Check if near high cover
            IsNearHighCover();

            // Check cover on the left side
            CoverLeft();
            // Check cover on the right side
            CoverRight();

            // Take cover if input detected
            TakeCover();
            // Leave cover if input detected
            LeaveCover();

            // Debug visualization of cover rays
            Debug.DrawRay(transform.position + new Vector3(0, 0.8f, 0), transform.forward * maxDistanceFromCover, lowCover ? Color.green : Color.white);
            Debug.DrawRay(transform.position + new Vector3(1, 0.8f, 0), transform.forward * maxDistanceFromCover, coverLeft ? Color.green : Color.white);
            Debug.DrawRay(transform.position + new Vector3(-1, 0.8f, 0), transform.forward * maxDistanceFromCover, coverRight ? Color.green : Color.white);
            Debug.DrawRay(transform.position + new Vector3(0, 2f, 0), transform.forward * maxDistanceFromCover, highCover ? Color.blue : Color.white);

            // Update animator parameters
            animator.SetBool("inHighCover", inHighCover);
            animator.SetBool("inLowCover", inLowCover);
        }

        // Check if near low cover
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

        // Check if near high cover
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

        // Check if cover is available on the left side
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

        // Check if cover is available on the right side
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

        // Take cover based on input
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

        // Leave cover based on input
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

        // Timeout coroutine for cover state
        IEnumerator CoverTimeout()
        {
            yield return new WaitForSeconds(0.1f);
            inCover = true;
        }

        // Timeout coroutine for high cover state
        IEnumerator HighCoverTimeout()
        {
            yield return new WaitForSeconds(0.1f);
            inHighCover = true;
        }

        // Timeout coroutine for low cover state
        IEnumerator LowCoverTimeout()
        {
            yield return new WaitForSeconds(0.1f);
            inLowCover = true;
        }
    }
}
