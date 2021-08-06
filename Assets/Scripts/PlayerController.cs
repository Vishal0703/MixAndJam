using System;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    [SerializeField] private float rotationRatePerSecond = 10f;
    [SerializeField] private float maxJumpHeight = 3f;
    [SerializeField] private float ascendingGravityMultiplier = 1f;
    [SerializeField] private float descendingGravityMultiplier = 1f; 
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayerMask;
    
    private PlayerInputManager playerInputManager;
    private CharacterController characterController;
    
    private bool isMoving;
    private Vector3 currentMovement;
    private bool jumpStart;
    private float charachterYVelocity;
    private float gravity = -9.8f;

    //private bool IsGrounded => GroundCheck();

    //private bool GroundCheck()
    //{
    //    return Physics.OverlapSphere(groundCheckTransform.position, groundCheckRadius, groundLayerMask, QueryTriggerInteraction.Ignore) != null;
           
    //}

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CalculateXZMovement();
        CalculateYMovement();
        //Jump();
        characterController.Move(currentMovement * Time.deltaTime);
        RotatePlayer();
    }

    private void CalculateYMovement()
    {
        var onGroundAndJumpStart = characterController.isGrounded && playerInputManager.IsJumping;
        var onGroundIdle = characterController.isGrounded && !playerInputManager.IsJumping;
        var onAirAscending = !characterController.isGrounded && currentMovement.y >= 0f;
        var onAirDescending = !characterController.isGrounded && currentMovement.y < 0f;

        if (onGroundAndJumpStart)
        {
            var initialVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * maxJumpHeight);
            currentMovement.y = initialVelocity;
        }
        else if(onGroundIdle)
        {
            currentMovement.y = -0.5f;
        }
        else if(onAirAscending)
        {
            currentMovement.y += 0.5f * gravity * ascendingGravityMultiplier * Time.deltaTime;
        }
        else if(onAirDescending)
        {
            currentMovement.y += 0.5f * gravity * descendingGravityMultiplier * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void CalculateXZMovement()
    {
        float runMultiplier = playerInputManager.IsRunning ? runSpeed / moveSpeed : 1f;
        currentMovement.x = playerInputManager.CurrentMovementInput.x * runMultiplier * moveSpeed;
        currentMovement.z = playerInputManager.CurrentMovementInput.y * runMultiplier * moveSpeed;
        isMoving = currentMovement.x != 0 || currentMovement.z != 0;
    }

    private void Jump()
    {
        if(playerInputManager.IsJumping && !jumpStart)
        {
            Debug.Log("Jump Initiated");
            jumpStart = true;
        }
        else if(jumpStart)
        {
            Debug.Log("Jump Start set to false");
            jumpStart = false;
        }

        if(jumpStart && characterController.isGrounded)
        {
            var initialVelocity = Mathf.Sqrt(2 * Physics.gravity.magnitude * ascendingGravityMultiplier * maxJumpHeight);
            currentMovement = currentMovement.SetDirectionalValue(y: initialVelocity);
        }
        else if(characterController.isGrounded)
        {
            currentMovement.y = -0.5f; // just a downward push for grounded case
        }
        else
        {
            currentMovement.y += -Physics.gravity.magnitude * ascendingGravityMultiplier * Time.deltaTime;
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
