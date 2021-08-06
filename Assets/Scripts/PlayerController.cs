using System;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    [SerializeField] private float rotationRatePerSecond = 10f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float descendingGravityMultiplier = 1f; 
    [SerializeField] private float ascendingGravityMultiplier = 1f;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayerMask;
    
    private PlayerInputManager playerInputManager;
    private CharacterController characterController;
    private Rigidbody rigidbody;
    
    private bool isMoving;
    private Vector3 currentMovement;
    private bool jumpStart;
    private float charachterYVelocity;

    private bool IsGrounded => GroundCheck();

    private bool GroundCheck()
    {
        return Physics.OverlapSphere(groundCheckTransform.position, groundCheckRadius, groundLayerMask, QueryTriggerInteraction.Ignore) != null;
           
    }

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        characterController = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CalculateMovement();
        Jump();
        characterController.Move(moveSpeed * Time.deltaTime * currentMovement);
        RotatePlayer();
    }

    private void CalculateMovement()
    {
        currentMovement.x = playerInputManager.CurrentMovementInput.x;
        currentMovement.z = playerInputManager.CurrentMovementInput.y;
        isMoving = currentMovement.x != 0 || currentMovement.z != 0;
        float runMultiplier = playerInputManager.IsRunning ? runSpeed/moveSpeed : 1;
        currentMovement *= runMultiplier;
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

        if(jumpStart && IsGrounded)
        {
            var initialVelocity = Mathf.Sqrt(2 * Physics.gravity.magnitude * ascendingGravityMultiplier * jumpHeight);
            rigidbody.velocity = rigidbody.velocity.SetDirectionalValue(y : initialVelocity);
            Debug.Log($"Initial velocity given : {initialVelocity}, rigidbody velocity : {rigidbody.velocity}");
        }

        charachterYVelocity = rigidbody.velocity.y >= -1f ? rigidbody.velocity.y : -1f;
        currentMovement.SetDirectionalValue(y: charachterYVelocity);
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
