using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private PlayerMovement playerMovement;

    private Animator animator;

    private int isGroundedHash;
    private int speedHash;
    private int isJumpingHash;
    private int isFallingHash;

    private void Awake()
    {
        if (playerReferences == null)
            playerReferences = GetComponent<PlayerReferences>();

        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        animator = playerReferences.animator;

        isGroundedHash = Animator.StringToHash("isGrounded");
        speedHash = Animator.StringToHash("speed");
        isJumpingHash = Animator.StringToHash("isJumping");
        isFallingHash = Animator.StringToHash("isFalling");
    }

    private void Update()
    {
        if (animator == null)
            return;

        // This is just placeholder code - you would connect this to your state machine
        // animator.SetBool(isGroundedHash, playerController.GroundDetector.IsGrounded);
        // animator.SetFloat(speedHash, playerMovement.CurrentSpeed);
        // animator.SetBool(isJumpingHash, stateMachine.CurrentState is JumpState);
        // animator.SetBool(isFallingHash, stateMachine.CurrentState is FallState);
    }

    public void PlayJumpAnimation()
    {
        // animator.SetTrigger("jump");
    }

    public void PlayLandAnimation()
    {
    }
}