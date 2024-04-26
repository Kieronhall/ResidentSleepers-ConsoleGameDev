using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Thirdperson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent (typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] float moveSpeed = 3f;
        [SerializeField] float rotationSpeed = 500f;

        [Header("Ground Check")]
        [SerializeField] float groundCheckRadius = 0.2f;
        [SerializeField] Vector3 groundCheckOffset;
        [SerializeField] LayerMask groundLayer;

        bool isGrounded;
        bool isSprinting;
        bool isCrouching;

        float ySpeed;

        Quaternion targetRotation;

        KeyCode sprintKey = KeyCode.JoystickButton4;
        KeyCode crouchKey = KeyCode.JoystickButton1;

        CameraController cameraController;
        Animator animator;
        CoverController coverController;
        public CharacterController characterController;

        private void Awake()
        {
            cameraController = Camera.main.GetComponent<CameraController>();
            animator = GetComponent<Animator>();
            coverController = GetComponent<CoverController>();
            characterController = GetComponent<CharacterController>();
            characterController.height = 1.7f;
            characterController.radius = 0.2f;
            characterController.center = new Vector3(0f, 0.87f, 0.1f);
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            var moveInput = new Vector3(horizontal, 0, vertical).normalized;

            var moveDirection = cameraController.PlanarRoatation * moveInput;

            GroundCheck();
            Crouch();
            Sprint();

            if (isGrounded)
            {
                ySpeed = -0.5f;
            }
            else
            {
                ySpeed += Physics.gravity.y * Time.deltaTime;
            }

            var velocity = moveDirection * moveSpeed;
            velocity.y = ySpeed;

            if (coverController.inCover)
            {
                if (horizontal < 0)
                {
                    moveSpeed = 1.5f;
                    velocity.x = moveDirection.x * moveSpeed;
                    velocity.z = 0;
                }
                else if (horizontal > 0)
                {
                    moveSpeed = 1.5f;
                    velocity.x = moveDirection.x * moveSpeed;
                    velocity.z = 0;
                }
                else
                {
                    velocity = Vector3.zero;
                }

                targetRotation = transform.rotation;
            }
            else
            {
                if (moveAmount > 0)
                {
                    targetRotation = Quaternion.LookRotation(moveDirection);
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);
                moveSpeed = isSprinting ? 5f : isCrouching ? 1.5f : 3f;
            }

            characterController.Move(velocity * Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);

            animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
            animator.SetBool("isSprinting", isSprinting);
            animator.SetBool("isCrouching", isCrouching);
        }

        private void GroundCheck()
        {
            isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
        }

        private void Crouch()
        {
            if (Input.GetKeyDown(crouchKey) && !isCrouching || Input.GetKeyDown(KeyCode.LeftControl) && !isCrouching)
            {
                isCrouching = true;
                characterController.height = 1.31f;
                characterController.center = new Vector3(0f, 0.678f, 0.38f);
            }
            else if (Input.GetKeyDown(crouchKey) && isCrouching || Input.GetKeyDown(KeyCode.LeftControl) && isCrouching)
            {
                isCrouching = false;
                characterController.height = 1.7f;
                characterController.center = new Vector3(0f, 0.87f, 0.1f);
            }
        }

        private void Sprint()
        {
            if (Input.GetKeyDown(sprintKey) && !isCrouching || Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
            {
                isSprinting = true;
            }
            else if (Input.GetKeyUp(sprintKey) && !isCrouching || Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
            {
                isSprinting = false;
            }
        }
    }
}