public class RunState : PlayerBaseState
{
    public RunState(PlayerStateMachine stateMachine, PlayerStateFactory factory)
        : base(stateMachine, factory)
    {
    }

    public override void EnterState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetBool("isRunning", true);
    }

    public override void ExitState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetBool("isRunning", false);
    }

    public override void LogicUpdate()
    {
        CheckTransitions();
    }

    public override void PhysicsUpdate()
    {
        HandleMovement(stateMachine.MovementData.runSpeed,
            stateMachine.MovementData.acceleration);
    }

    public override void CheckTransitions()
    {
        CheckForJumpInput();
        CheckForNoMovementInput();
        CheckForGrounded();

        if (stateMachine.PlayerInput.MovementInputMagnitude <= 0.5f &&
            stateMachine.PlayerInput.MovementInputMagnitude > 0.1f)
        {
            stateMachine.ChangeState(factory.Walk());
        }
    }
}