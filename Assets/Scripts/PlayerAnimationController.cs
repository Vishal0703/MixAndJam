using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerInputManager playerInputManager;

    private int isWalking;
    private int isRunning;


    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        animator = GetComponent<Animator>();

        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
    }

    private void Update()
    {
        if (playerInputManager.CurrentMovementInput.sqrMagnitude > 0f)
            animator.SetBool(isWalking, true);
        else
            animator.SetBool(isWalking, false);

        if (playerInputManager.IsRunning)
            animator.SetBool(isRunning, true);
        else
            animator.SetBool(isRunning, false);

    }
}
