public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateMachine stateMachine, PlayerStateFactory factory)
        : base(stateMachine, factory)
    {
    }

    public override void EnterState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetBool("isIdle", true);
    }

    public override void ExitState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetBool("isIdle", false);
    }

    public override void LogicUpdate()
    {
        CheckTransitions();
    }

    public override void PhysicsUpdate()
    {
        HandleMovement(0, stateMachine.MovementData.deceleration);
    }

    public override void CheckTransitions()
    {
        CheckForJumpInput();
        CheckForMovementInput();
        CheckForGrounded();
    }
}