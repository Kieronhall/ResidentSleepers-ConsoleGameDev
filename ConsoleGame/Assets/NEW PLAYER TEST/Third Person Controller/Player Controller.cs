using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CoverController))]
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

    private PlayerControls _input;
    private PlayerInput _playerInput;

    [Header("Cinemachine")]
    public GameObject cinemachineCameraTarget;
    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;
    public float cameraAngleOverride = 0.0f;
    public bool lockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;
    public float speedChangeRate = 10.0f;
    public float sensitivty = 1f;
    public float gravity = -15.0f;
    public float fallTimeout = 0.15f;

    private const float _threshold = 0.01f;


    private void Awake()
    {
        _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _input = GetComponent<PlayerControls>();

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

        var moveDirection = cinemachineCameraTarget.transform.rotation * moveInput;

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

        // Restrict movement and rotation if in cover
        if (coverController.inCover)
        {
            // Check if moving left or right
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
                // If not moving left or right, restrict all movement
                velocity = Vector3.zero;
            }

            // Set rotation to current rotation if in cover
            targetRotation = transform.rotation;
        }
        else
        {
            // Only rotate if not in cover
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

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !lockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier =  1.0f;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * sensitivty;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * sensitivty;
        }

        // clamp rotations so values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
