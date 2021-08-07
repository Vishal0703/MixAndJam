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
    [SerializeField] private GameEvent jumpEvent;
    [SerializeField] private GameEvent landEvent;
    public bool IsMoving { get; private set; }

    private PlayerInputManager playerInputManager;
    private CharacterController characterController;
    
    private Vector3 currentVelocity;
    private readonly float gravity = -9.8f;
    private bool isJumpAllowed = true;
    private bool prevCharacterControllerGroundedState = true;

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
        LandEventRaise(); //This must be called before raising jumpevent, since in the same frame player can land an jump again, and calling this later will make the isJumping variable of animator false

        var onGroundAndJumpStart = characterController.isGrounded && playerInputManager.IsJumping;
        var onGroundIdle = characterController.isGrounded && !playerInputManager.IsJumping;
        var onAirAscending = !characterController.isGrounded && currentVelocity.y >= 0f;
        var onAirDescending = !characterController.isGrounded && currentVelocity.y < 0f;

        if (onGroundAndJumpStart && isJumpAllowed)
        {
            var initialVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * maxJumpHeight);
            currentVelocity.y = initialVelocity;
            jumpEvent.Raise();
            isJumpAllowed = false;
        }
        else if (onGroundIdle)
        {
            currentVelocity.y = -0.5f;
        }
        else if (onAirAscending)
        {
            currentVelocity.y += 0.5f * gravity * ascendingGravityMultiplier * Time.deltaTime;
        }
        else if (onAirDescending)
        {
            currentVelocity.y += 0.5f * gravity * descendingGravityMultiplier * Time.deltaTime;
        }

        if (!isJumpAllowed && !playerInputManager.IsJumping)
        {
            isJumpAllowed = true;
        }

    }

    private void LandEventRaise()
    {
        if (!prevCharacterControllerGroundedState && characterController.isGrounded)
        {
            landEvent.Raise();
        }
        prevCharacterControllerGroundedState = characterController.isGrounded;
    }

    private void CalculateXZMovement()
    {
        float runMultiplier = playerInputManager.IsRunning ? runSpeed / moveSpeed : 1f;
        currentVelocity.x = playerInputManager.CurrentMovementInput.x * runMultiplier * moveSpeed;
        currentVelocity.z = playerInputManager.CurrentMovementInput.y * runMultiplier * moveSpeed;
        IsMoving = currentVelocity.x != 0 || currentVelocity.z != 0;
    }

    private void RotatePlayer()
    {
        if(IsMoving)
        {
            var initialRotation = transform.rotation;
            var finalRotation = Quaternion.LookRotation(new Vector3(playerInputManager.CurrentMovementInput.x, 0f, playerInputManager.CurrentMovementInput.y));
            transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, rotationRatePerSecond * Time.deltaTime);
        }
    }
}
