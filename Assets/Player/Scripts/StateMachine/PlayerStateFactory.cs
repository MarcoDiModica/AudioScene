public class PlayerStateFactory
{
    private PlayerStateMachine stateMachine;

    public PlayerStateFactory(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public PlayerBaseState Idle()
    {
        return new IdleState(stateMachine, this);
    }

    public PlayerBaseState Walk()
    {
        return new WalkState(stateMachine, this);
    }

    public PlayerBaseState Run()
    {
        return new RunState(stateMachine, this);
    }

    public PlayerBaseState Jump()
    {
        return new JumpState(stateMachine, this);
    }

    public PlayerBaseState Fall()
    {
        return new FallState(stateMachine, this);
    }
}