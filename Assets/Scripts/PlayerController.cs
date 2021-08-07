using System;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float rotationRatePerSecond = 10f;
    [SerializeField] private float maxJumpHeight = 2f;
    [SerializeField] private float ascendingGravityMultiplier = 2f;
    [SerializeField] private float descendingGravityMultiplier = 4f; 

    private PlayerInputManager playerInputManager;
    private CharacterController characterController;
    
    private bool isMoving;
    private Vector3 currentVelocity;
    private bool jumpStart;
    private readonly float gravity = -9.8f;
    private bool isJumpAllowed = true;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CalculateXZMovement();
        CalculateYMovement();
        characterController.Move(currentVelocity * Time.deltaTime);
        RotatePlayer();
    }

    private void CalculateYMovement()
    {
        var onGroundAndJumpStart = characterController.isGrounded && playerInputManager.IsJumping;
        var onGroundIdle = characterController.isGrounded && !playerInputManager.IsJumping;
        var onAirAscending = !characterController.isGrounded && currentVelocity.y >= 0f;
        var onAirDescending = !characterController.isGrounded && currentVelocity.y < 0f;

        if (onGroundAndJumpStart && isJumpAllowed)
        {
            var initialVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * maxJumpHeight);
            currentVelocity.y = initialVelocity;
            isJumpAllowed = false;
        }
        else if(onGroundIdle)
        {
            currentVelocity.y = -0.5f;
        }
        else if(onAirAscending)
        {
            currentVelocity.y += 0.5f * gravity * ascendingGravityMultiplier * Time.deltaTime;
        }
        else if(onAirDescending)
        {
            currentVelocity.y += 0.5f * gravity * descendingGravityMultiplier * Time.deltaTime;
        }

        if(!isJumpAllowed && !playerInputManager.IsJumping)
        {
            isJumpAllowed = true;
        }

    }

    private void CalculateXZMovement()
    {
        float runMultiplier = playerInputManager.IsRunning ? runSpeed / moveSpeed : 1f;
        currentVelocity.x = playerInputManager.CurrentMovementInput.x * runMultiplier * moveSpeed;
        currentVelocity.z = playerInputManager.CurrentMovementInput.y * runMultiplier * moveSpeed;
        isMoving = currentVelocity.x != 0 || currentVelocity.z != 0;
    }

    private void RotatePlayer()
    {
        if(isMoving)
        {
            var initialRotation = transform.rotation;
            var finalRotation = Quaternion.LookRotation(new Vector3(playerInputManager.CurrentMovementInput.x, 0f, playerInputManager.CurrentMovementInput.y));
            transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, rotationRatePerSecond * Time.deltaTime);
        }
    }
}
