using UnityEngine;

public class SlopeHandler : MonoBehaviour
{
    [SerializeField] private MovementData movementData;

    private CharacterController characterController;
    private PlayerReferences playerReferences;
    private GroundDetector groundDetector;

    private RaycastHit slopeHit;
    private Vector3 slopeNormal;
    private float slopeAngle;
    private bool isOnSlope;
    private bool isGoingUphill;

    private void Awake()
    {
        playerReferences = GetComponent<PlayerReferences>();
        characterController = playerReferences.characterController;
        groundDetector = GetComponent<GroundDetector>();
    }

    public void HandleSlope()
    {
        if (!groundDetector.IsGrounded)
        {
            isOnSlope = false;
            return;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit,
            characterController.height / 2 + movementData.slopeCheckDistance,
            movementData.groundLayers))
        {
            slopeNormal = slopeHit.normal;
            slopeAngle = Vector3.Angle(slopeNormal, Vector3.up);

            isOnSlope = slopeAngle > 0 && slopeAngle <= movementData.maxSlopeAngle;

            if (isOnSlope)
            {
                Vector3 moveDirection = GetComponent<PlayerInput>().MoveDirection;
                isGoingUphill = Vector3.Dot(moveDirection.normalized,
                    Vector3.ProjectOnPlane(Vector3.up, slopeNormal).normalized) < 0;
            }
        }
        else
        {
            isOnSlope = false;
        }
    }

    public Vector3 GetSlopeMoveDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeNormal);
    }

    public bool IsOnSlope => isOnSlope;
    public bool IsGoingUphill => isGoingUphill;
    public float SlopeAngle => slopeAngle;
    public Vector3 SlopeNormal => slopeNormal;
}