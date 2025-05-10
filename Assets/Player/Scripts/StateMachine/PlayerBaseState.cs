public abstract class PlayerBaseState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerStateFactory factory;

    public PlayerBaseState(PlayerStateMachine stateMachine, PlayerStateFactory factory)
    {
        this.stateMachine = stateMachine;
        this.factory = factory;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void CheckTransitions();

    protected void CheckForJumpInput()
    {
        if (stateMachine.PlayerInput.ConsumeJumpInput() && stateMachine.GroundDetector.CanJump())
        {
            stateMachine.GroundDetector.RecordJump();
            stateMachine.ChangeState(factory.Jump());
        }
    }

    protected void CheckForGrounded()
    {
        if (!stateMachine.GroundDetector.IsGrounded &&
            stateMachine.PlayerMovement.VerticalVelocity.y < 0)
        {
            stateMachine.ChangeState(factory.Fall());
        }
    }

    protected void CheckForMovementInput()
    {
        if (stateMachine.PlayerInput.MovementInputMagnitude > 0.1f)
        {
            if (stateMachine.PlayerInput.MovementInputMagnitude > 0.5f)
            {
                stateMachine.ChangeState(factory.Run());
            }
            else
            {
                stateMachine.ChangeState(factory.Walk());
            }
        }
    }

    protected void CheckForNoMovementInput()
    {
        if (stateMachine.PlayerInput.MovementInputMagnitude < 0.1f)
        {
            stateMachine.ChangeState(factory.Idle());
        }
    }

    protected void HandleMovement(float targetSpeed, float acceleration)
    {
        stateMachine.PlayerMovement.Move(
            stateMachine.PlayerInput.MoveDirection,
            targetSpeed,
            acceleration
        );
    }
}