public class WalkState : PlayerBaseState
{
    public WalkState(PlayerStateMachine stateMachine, PlayerStateFactory factory)
        : base(stateMachine, factory)
    {
    }

    public override void EnterState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetBool("isWalking", true);
    }

    public override void ExitState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetBool("isWalking", false);
    }

    public override void LogicUpdate()
    {
        CheckTransitions();
    }

    public override void PhysicsUpdate()
    {
        HandleMovement(stateMachine.MovementData.walkSpeed,
            stateMachine.MovementData.acceleration);
    }

    public override void CheckTransitions()
    {
        CheckForJumpInput();
        CheckForNoMovementInput();
        CheckForGrounded();

        if (stateMachine.PlayerInput.MovementInputMagnitude > 0.5f)
        {
            stateMachine.ChangeState(factory.Run());
        }
    }
}