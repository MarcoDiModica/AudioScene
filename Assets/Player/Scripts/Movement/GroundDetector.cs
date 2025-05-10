using UnityEngine;
using System.Collections;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private MovementData movementData;

    private CharacterController characterController;
    private PlayerReferences playerReferences;

    private bool isGrounded;
    private bool wasGrounded;
    private float lastGroundedTime;
    private float lastJumpTime;

    private void Awake()
    {
        playerReferences = GetComponent<PlayerReferences>();
        characterController = playerReferences.characterController;
    }

    public void CheckGround()
    {
        wasGrounded = isGrounded;

        bool controllerGrounded = characterController.isGrounded;

        bool sphereGrounded = Physics.CheckSphere(
            transform.position - new Vector3(0, characterController.height / 2 - characterController.center.y, 0),
            movementData.groundCheckRadius,
            movementData.groundLayers
        );

        isGrounded = controllerGrounded || sphereGrounded;

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (isGrounded && !wasGrounded)
        {
            OnLanded();
        }
        else if (!isGrounded && wasGrounded)
        {
            OnLeftGround();
        }
    }

    private void OnLanded()
    {
        // Placeholder for landing events
    }

    private void OnLeftGround()
    {
        // Placeholder for leaving ground events
    }

    public void RecordJump()
    {
        lastJumpTime = Time.time;
    }

    public bool CanJump()
    {
        return Time.time - lastGroundedTime <= movementData.coyoteTime &&
               Time.time - lastJumpTime >= movementData.jumpCooldown;
    }

    public bool HasBufferedJump()
    {
        return Time.time - lastJumpTime <= movementData.jumpBufferTime;
    }

    public bool IsGrounded => isGrounded;
    public bool WasGrounded => wasGrounded;
    public float LastGroundedTime => lastGroundedTime;
}