using UnityEngine;

public class JumpState : PlayerBaseState
{
    private float jumpStartTime;
    private float jumpHoldTime;
    private bool jumpReleased;

    public JumpState(PlayerStateMachine stateMachine, PlayerStateFactory factory)
        : base(stateMachine, factory)
    {
    }

    public override void EnterState()
    {
        // Animation handling (placeholder)
        // stateMachine.PlayerReferences.animator.SetTrigger("jump");

        stateMachine.PlayerMovement.Jump(stateMachine.MovementData.minJumpForce);

        jumpStartTime = Time.time;
        jumpHoldTime = stateMachine.MovementData.jumpHoldTime;
        jumpReleased = false;
    }

    public override void ExitState()
    {
        // Clean up when leaving jump state
    }

    public override void LogicUpdate()
    {
        if (!stateMachine.PlayerInput.JumpInputHeld && !jumpReleased)
        {
            jumpReleased = true;
        }

        if (stateMachine.PlayerInput.JumpInputHeld &&
            Time.time < jumpStartTime + jumpHoldTime &&
            !jumpReleased)
        {
            float additionalForce = (stateMachine.MovementData.jumpForce -
                stateMachine.MovementData.minJumpForce) *
                Time.deltaTime / jumpHoldTime;

            stateMachine.PlayerMovement.Jump(additionalForce);
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
        if (stateMachine.PlayerMovement.VerticalVelocity.y <= 0.1f)
        {
            stateMachine.ChangeState(factory.Fall());
        }
    }
}