using UnityEngine;

public class MovementVisualizers : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private bool showGroundCheck = true;
    [SerializeField] private bool showSlopeCheck = true;
    [SerializeField] private bool showVelocity = true;
    [SerializeField] private Color groundCheckColor = Color.green;
    [SerializeField] private Color slopeCheckColor = Color.blue;
    [SerializeField] private Color velocityColor = Color.red;

    private void OnDrawGizmos()
    {
        if (playerController == null || playerReferences == null)
            return;

        if (showGroundCheck && playerController.GroundDetector != null)
        {
            Gizmos.color = playerController.GroundDetector.IsGrounded ? groundCheckColor : Color.red;
            Gizmos.DrawWireSphere(
                transform.position - new Vector3(0, playerReferences.characterController.height / 2 - playerReferences.characterController.center.y, 0),
                playerController.MovementData.groundCheckRadius
            );
        }

        if (showSlopeCheck && playerController.SlopeHandler != null)
        {
            if (playerController.SlopeHandler.IsOnSlope)
            {
                Gizmos.color = slopeCheckColor;
                Gizmos.DrawRay(
                    transform.position - new Vector3(0, playerReferences.characterController.height / 2, 0),
                    playerController.SlopeHandler.SlopeNormal * 2f
                );
            }
        }

        if (showVelocity && playerController.Movement != null)
        {
            Gizmos.color = velocityColor;
            Gizmos.DrawRay(transform.position, playerController.Movement.CurrentVelocity);
        }
    }
}