public class FallState : PlayerBaseState
{
    public FallState(PlayerStateMachine stateMachine, PlayerStateFactory factory)
        : base(stateMachine, factory)
    {
    }

    public override void EnterState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetBool("isFalling", true);
    }

    public override void ExitState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetBool("isFalling", false);
    }

    public override void LogicUpdate()
    {
        if (stateMachine.PlayerInput.ConsumeJumpInput())
        {
            stateMachine.GroundDetector.RecordJump();
        }

        CheckTransitions();
    }

    public override void PhysicsUpdate()
    {
        HandleMovement(
            stateMachine.PlayerInput.MovementInputMagnitude > 0.5f ?
                stateMachine.MovementData.runSpeed : stateMachine.MovementData.walkSpeed,
            stateMachine.MovementData.acceleration * stateMachine.MovementData.airControl
        );
    }

    public override void CheckTransitions()
    {
        if (stateMachine.GroundDetector.IsGrounded)
        {
            if (stateMachine.GroundDetector.HasBufferedJump() &&
                stateMachine.GroundDetector.CanJump())
            {
                stateMachine.ChangeState(factory.Jump());
                return;
            }

            if (stateMachine.PlayerInput.MovementInputMagnitude < 0.1f)
            {
                stateMachine.ChangeState(factory.Idle());
            }
            else if (stateMachine.PlayerInput.MovementInputMagnitude <= 0.5f)
            {
                stateMachine.ChangeState(factory.Walk());
            }
            else
            {
                stateMachine.ChangeState(factory.Run());
            }
        }
    }
}