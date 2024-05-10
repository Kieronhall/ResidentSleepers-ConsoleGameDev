using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        // Player movement parameters
        [Header("Player")]
        public float moveSpeed = 3f;
        public float sprintSpeed = 6f;
        public float crouchSpeed = 1.5f;
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;
        public float speedChangeRate = 10.0f;
        public float sensitivity = 1f;
        public float gravity = -15.0f;
        public float fallTimeout = 0.15f;

        // Ground detection parameters
        [Header("Player grounded")]
        public bool grounded = true;
        public float groundedOffset = -0.14f;
        public float groundedRadius = 0.28f;
        public LayerMask groundLayers;

        // Cinemachine camera parameters
        [Header("Cinemachine")]
        public GameObject cinemachineCameraTarget;
        public float topClamp = 70.0f;
        public float bottomClamp = -30.0f;
        public float cameraAngleOverride = 0.0f;
        public bool lockCameraPosition = false;

        // Internal variables for camera rotation
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        // Internal variables for movement
        private float speed;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float terminalVelocity = 53.0f;

        // References to other components
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        PlayerInput playerInput;
        PlayerControls input;
        GameObject mainCamera;
        CoverController coverController;

        // Footstep sound variables
        private FMOD.Studio.EventInstance footsteps;
        private int walkType;

        // State variables
        bool isCrouching;
        private bool _rotateOnMove = true;
        private const float _threshold = 0.01f;

        // Property to check if current control scheme is mouse
        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            // Grab the MainCamera if it as not already been assigned
            if (mainCamera == null)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;

            // Initialization
            characterController = GetComponent<CharacterController>();
            input = GetComponent<PlayerControls>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
            coverController = GetComponent<CoverController>();

            // Set character controller properties
            characterController.height = 1.7f;
            characterController.radius = 0.2f;
            characterController.center = new Vector3(0f, 0.87f, 0.1f);
        }

        private void Update()
        {
            // Handle basic character movement
            Gravity();
            groundedCheck();
            Move();
            Crouch();

            // Adjust movement speed and character controller properties based on cover status
            moveSpeed = coverController.inCover ? 1.5f : 3f;
            characterController.center = input.crouch ? new Vector3(0f, 0.678f, 0.38f) : coverController.inHighCover ? new Vector3(0f, 0.87f, 0f)
                : coverController.inLowCover ? new Vector3(0f, 0.678f, 0.0f) : new Vector3(0f, 0.87f, 0.1f);
            characterController.height = input.crouch || coverController.inLowCover ? 1.31f : 1.7f;
        }

        private void LateUpdate()
        {
            // Handle camera rotation after character movement
            CameraRotation();
        }

        // Check if the player is grounded
        private void groundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
                transform.position.z);
            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);
        }

        // Handle camera rotation based on input
        private void CameraRotation()
        {
            if (input.look.sqrMagnitude >= _threshold && !lockCameraPosition)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                cinemachineTargetYaw += input.look.x * deltaTimeMultiplier * sensitivity;
                cinemachineTargetPitch += input.look.y * deltaTimeMultiplier * sensitivity;
            }

            // Clamp camera angles
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            // Apply camera rotation
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
        }

        // Handle player movement
        private void Move()
        {
            // Determine target speed based on input
            float targetSpeed = input.sprint ? sprintSpeed : input.crouch ? crouchSpeed : moveSpeed;

            // Determine walk type based on walk speed
            walkType = targetSpeed == sprintSpeed ? 1 : targetSpeed == crouchSpeed ? 2 : 0;

            // Prevents movement if there is no input
            if (input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            // Adjust target speed based on analog movement
            float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

            // Smoothly change speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            // Determine movement direction
            Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

            // Handle movement while in cover
            if (coverController.inCover)
            {
                if (input.move.x < 0 && coverController.coverRight)
                {
                    input.move.x = inputDirection.x;
                    targetRotation = -90f;
                }
                else if (input.move.x > 0 && coverController.coverLeft)
                {
                    input.move.x = inputDirection.x;
                    targetRotation = 90f;
                }
                else
                {
                    input.move = Vector3.zero;
                }
            }
            else
            {
                // Determine target rotation
                if (input.move != Vector2.zero)
                {
                    targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                        rotationSmoothTime);

                    // Apply rotation
                    if (_rotateOnMove)
                    {
                        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    }
                }
            }

            // Move the character controller
            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

            // Update animator parameters
            animator.SetFloat("moveAmount", targetSpeed);
            animator.SetFloat("moveDirection", input.move.x);
        }

        // Handle crouching
        private void Crouch()
        {
            if (input.crouch)
            {
                isCrouching = true;
                animator.SetBool("isCrouching", true);
            }
            else if (!input.crouch)
            {
                isCrouching = false;
                animator.SetBool("isCrouching", false);
            }
        }

        // Handle gravity
        private void Gravity()
        {
            if (grounded)
            {
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }
            }

            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }

        // Clamp an angle between a minimum and maximum value
        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        // Draw ground check gizmo
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z),
                groundedRadius);
        }

        // Set the camera sensitivity
        public void SetSensitivity(float newSensitivity)
        {
            sensitivity = newSensitivity;
        }

        // Set whether the player rotates while moving
        public void SetRotationOnMove(bool newRotateOnMove)
        {
            _rotateOnMove = newRotateOnMove;
        }

        // Play footstep sounds
        private void PlayFootstep(int walkType)
        {
            footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Dylan/Player/Footsteps");
            footsteps.setParameterByName("Walk Type", walkType);
            footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            footsteps.start();
            footsteps.release();
        }

        // Select and play footstep sounds
        public void SelectAndPlayFootstep()
        {
            PlayFootstep(walkType);
        }
    }
}