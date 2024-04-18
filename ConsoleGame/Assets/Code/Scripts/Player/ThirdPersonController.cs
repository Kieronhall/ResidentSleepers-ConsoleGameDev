using System.Collections;
using Thirdperson;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        public float moveSpeed = 3f;
        public float sprintSpeed = 6f;
        public float crouchSpeed = 1.5f;
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;
        public float speedChangeRate = 10.0f;
        public float sensitivty = 1f;
        public float gravity = -15.0f;
        public float fallTimeout = 0.15f;

        [Header("Player grounded")]
        public bool grounded = true;
        public float groundedOffset = -0.14f;
        public float groundedRadius = 0.28f;
        public LayerMask groundLayers;

        [Header("Cinemachine")]
        public GameObject cinemachineCameraTarget;
        public float topClamp = 70.0f;
        public float bottomClamp = -30.0f;
        public float cameraAngleOverride = 0.0f;
        public bool lockCameraPosition = false;

        // cinemachine
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        // player
        private float speed;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float terminalVelocity = 53.0f;

        public CharacterController characterController;
        public Animator animator;
        PlayerInput playerInput;
        PlayerControls input;
        GameObject mainCamera;
        CoverController coverController;

        bool isCrouching;
        bool crouchTImeout;
        private bool _rotateOnMove = true;
        private const float _threshold = 0.01f;

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
            // get a reference to the main camera
            if (mainCamera == null)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            characterController = GetComponent<CharacterController>();
            input = GetComponent<PlayerControls>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();

            characterController.height = 1.7f;
            characterController.radius = 0.2f;
            characterController.center = new Vector3(0f, 0.87f, 0.1f);
        }

        private void Update()
        {
            Gravity();
            groundedCheck();
            Move();
            Crouch();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void groundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
                transform.position.z);
            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (input.look.sqrMagnitude >= _threshold && !lockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                cinemachineTargetYaw += input.look.x * deltaTimeMultiplier * sensitivty;
                cinemachineTargetPitch += input.look.y * deltaTimeMultiplier * sensitivty;
            }

            // clamp rotations so values are limited 360 degrees
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed, crouch speed if crouch is pressed
            float targetSpeed = input.sprint ? sprintSpeed : input.crouch ? crouchSpeed : moveSpeed;

            // if there is no input, set the target speed to 0
            if (input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

            // if there is a move input rotate player when the player is moving
            if (input.move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

                // rotate to face input direction relative to camera position
                if (_rotateOnMove)
                {
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // move the player
            characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

            animator.SetFloat("moveAmount", targetSpeed, 0.2f, Time.deltaTime);
        }

        private void Crouch()
        {
            if (input.crouch)
            {
                isCrouching = true;
                animator.SetBool("isCrouching", true);
                characterController.height = 1.31f;
                characterController.center = new Vector3(0f, 0.678f, 0.38f);
            }
            else if (!input.crouch)
            {
                isCrouching = false;
                animator.SetBool("isCrouching", false);
                characterController.height = 1.7f;
                characterController.center = new Vector3(0f, 0.87f, 0.1f);
            }
        }

        IEnumerator CrouchTimeout()
        {
            crouchTImeout = true;
            yield return new WaitForSeconds(1);
            crouchTImeout = false;
        }

        private void Gravity()
        {
            if (grounded)
            {
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z),
                groundedRadius);
        }

        public void SetSensitivity(float newSensitivity)
        {
            sensitivty = newSensitivity;
        }

        public void SetRotationOnMove(bool newRotateOnMove)
        {
            _rotateOnMove = newRotateOnMove;
        }
    }
}