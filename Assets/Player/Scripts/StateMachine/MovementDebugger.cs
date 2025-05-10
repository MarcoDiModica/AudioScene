using UnityEngine;

public class MovementDebugger : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private bool showStateInfo = true;
    [SerializeField] private bool showMovementInfo = true;
    [SerializeField] private bool showGroundInfo = true;
    [SerializeField] private bool showSlopeInfo = true;

    private void OnGUI()
    {
        if (!showDebugInfo)
            return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 500));
        GUILayout.BeginVertical("box");

        // State info
        if (showStateInfo && playerController.StateMachine != null)
        {
            GUILayout.Label("<b>STATE INFO</b>");
            GUILayout.Label($"Current State: {playerController.StateMachine.CurrentState.GetType().Name}");
            GUILayout.Space(10);
        }

        // Movement info
        if (showMovementInfo && playerController.Movement != null)
        {
            GUILayout.Label("<b>MOVEMENT INFO</b>");
            GUILayout.Label($"Current Speed: {playerController.Movement.CurrentSpeed:F2}");
            GUILayout.Label($"Velocity: {playerController.Movement.CurrentVelocity:F2}");
            GUILayout.Label($"Vertical Velocity: {playerController.Movement.VerticalVelocity.y:F2}");
            GUILayout.Space(10);
        }

        // Ground info
        if (showGroundInfo && playerController.GroundDetector != null)
        {
            GUILayout.Label("<b>GROUND INFO</b>");
            GUILayout.Label($"Is Grounded: {playerController.GroundDetector.IsGrounded}");
            GUILayout.Label($"Was Grounded: {playerController.GroundDetector.WasGrounded}");
            GUILayout.Label($"Last Grounded Time: {Time.time - playerController.GroundDetector.LastGroundedTime:F2}s ago");
            GUILayout.Space(10);
        }

        // Slope info
        if (showSlopeInfo && playerController.SlopeHandler != null)
        {
            GUILayout.Label("<b>SLOPE INFO</b>");
            GUILayout.Label($"Is On Slope: {playerController.SlopeHandler.IsOnSlope}");
            GUILayout.Label($"Is Going Uphill: {playerController.SlopeHandler.IsGoingUphill}");
            GUILayout.Label($"Slope Angle: {playerController.SlopeHandler.SlopeAngle:F2}°");
            GUILayout.Space(10);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}