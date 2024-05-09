using UnityEngine;
using Cinemachine;

namespace ThirdPerson
{
    public class ThirdPersonAim : MonoBehaviour
    {
        // Reference to the aiming virtual camera
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        // Sensitivity values for normal and aiming mode
        public float normalSensitivity;
        public float aimSensitivity;
        // Layer mask for aiming collision detection
        [SerializeField] private LayerMask aimColldierMask;
        // Transform for debug purposes
        [SerializeField] private Transform debugTransform;
        // Reference to the player's pistol
        [SerializeField] private Pistol_Player playerPistol;
        // Reference to the health bar UI
        [SerializeField] private HealthBar healthBar;

        // References to other components
        private ThirdPersonController controller;
        private PlayerControls input;
        private CoverController coverController;

        // Gun game object and damage value
        [Header("Gun")]
        public GameObject gun;
        public float damage = 100f;

        private void Awake()
        {
            // Initialization
            input = GetComponent<PlayerControls>();
            controller = GetComponent<ThirdPersonController>();
            coverController = GetComponent<CoverController>();
        }

        private void Update()
        {
            // Variables for aiming
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);
            Transform hitTransform = null;

            // Perform raycast to detect target for aiming
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColldierMask))
            {
                mouseWorldPosition = raycastHit.point;
                hitTransform = raycastHit.transform;
            }

            // Handle aiming behavior
            if (input.aim && !coverController.inCover)
            {
                // Perform aiming actions
                playerAnimAim();
                gunShow();
                healthBar.ammoBar.SetActive(true);

                aimVirtualCamera.gameObject.SetActive(true);
                controller.SetSensitivity(aimSensitivity);
                controller.SetRotationOnMove(false);

                // Align character's forward direction with aiming direction
                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

                // Handle shooting
                if (input.aim && input.shoot)
                {
                    if (hitTransform != null)
                    {
                        if (playerPistol.bulletsLeft > 0)
                        {
                            playerAnimShoot();
                        }

                        if (playerPistol.bulletsLeft > 0 && playerPistol.readyToShoot)
                        {
                            playerPistol.bulletsShot = playerPistol.bulletsPerTap;
                            playerPistol.Shoot(raycastHit);
                            // Play shooting sound
                            FMODUnity.RuntimeManager.PlayOneShot("event:/Dylan/Player/Shooting", gun.transform.position);
                            input.shoot = false;
                        }
                    }
                    else
                    {
                        // Log warning if hitTransform is null
                        Debug.LogWarning("hitTransform is null. Shooting action aborted.");
                    }
                }
            }
            else
            {
                // Reset aiming actions when not aiming
                input.shoot = false;
                playerAnimAimFalse();
                healthBar.ammoBar.SetActive(false);

                aimVirtualCamera.gameObject.SetActive(false);
                controller.SetSensitivity(normalSensitivity);
                controller.SetRotationOnMove(true);
            }
        }

        // Show the gun game object
        public void gunShow()
        {
            if (gun != null)
            {
                gun.SetActive(true);
            }
        }

        // Hide the gun game object
        public void gunHide()
        {
            if (gun != null)
            {
                gun.SetActive(false);
            }
        }

        // Set the aiming animation for the player
        public void playerAnimAim()
        {
            controller.animator.SetBool("isAiming", true);
        }

        // Reset the aiming animation for the player
        public void playerAnimAimFalse()
        {
            controller.animator.SetBool("isAiming", false);
        }

        // Set the shooting animation for the player
        public void playerAnimShoot()
        {
            controller.animator.SetBool("isShooting", true);
            // Reset shooting animation after a delay
            Invoke("ResetShootAnimation", 0.1f);
        }

        // Reset the shooting animation for the player
        void ResetShootAnimation()
        {
            controller.animator.SetBool("isShooting", false);
        }

        // Reset the shooting animation for the player
        public void playerAnimShootFalse()
        {
            controller.animator.SetBool("isShooting", false);
        }
    }
}
