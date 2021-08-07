using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerInputManager playerInputManager;
    private PlayerController playerController;

    private int isWalking;
    private int isRunning;
    private int isJumping;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
        isJumping = Animator.StringToHash("isJumping");
    }

    private void Update()
    {
        if (playerController.IsMoving)
            animator.SetBool(isWalking, true);
        else
            animator.SetBool(isWalking, false);

        if (playerInputManager.IsRunning && playerController.IsMoving)
            animator.SetBool(isRunning, true);
        else
            animator.SetBool(isRunning, false);
    }

    public void SetJumpAnimationState(bool jump)
    {
        animator.SetBool(isJumping, jump);
    }    
}
