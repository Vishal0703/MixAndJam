using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    
    public Vector2 CurrentMovementInput { get; private set; }
    public float CurrentRotationInput { get; private set; }
    public bool IsRunning { get; private set; }

    public bool IsJumping { get; private set; }
    
    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();

        playerInput.CharacterControls.Move.performed += SetMovementValue;
        playerInput.CharacterControls.Move.canceled += SetMovementValue;

        playerInput.CharacterControls.Rotate.performed += SetRotationValue;
        playerInput.CharacterControls.Rotate.canceled += SetRotationValue;

        playerInput.CharacterControls.Run.performed += SetRunValue;
        playerInput.CharacterControls.Run.canceled += SetRunValue;

        playerInput.CharacterControls.Jump.performed += SetJumpValue;
        playerInput.CharacterControls.Jump.canceled += SetJumpValue;
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();

        playerInput.CharacterControls.Move.performed -= SetMovementValue;
        playerInput.CharacterControls.Move.canceled -= SetMovementValue;

        playerInput.CharacterControls.Rotate.performed -= SetRotationValue;
        playerInput.CharacterControls.Rotate.canceled -= SetRotationValue;

        playerInput.CharacterControls.Run.performed -= SetRunValue;
        playerInput.CharacterControls.Run.canceled -= SetRunValue;

        playerInput.CharacterControls.Jump.performed -= SetJumpValue;
        playerInput.CharacterControls.Jump.canceled -= SetJumpValue;
    }

    private void SetRunValue(InputAction.CallbackContext context)
    {
        IsRunning = context.ReadValueAsButton();
    }

    private void SetRotationValue(InputAction.CallbackContext context)
    {
        CurrentRotationInput = context.ReadValue<float>();
        //Debug.Log(CurrentRotationInput);
    }

    private void SetMovementValue(InputAction.CallbackContext context)
    {
        CurrentMovementInput = context.ReadValue<Vector2>();
        //Debug.Log(CurrentMovementInput);
    }
    private void SetJumpValue(InputAction.CallbackContext context)
    {
        IsJumping = context.ReadValueAsButton();
    }

}
