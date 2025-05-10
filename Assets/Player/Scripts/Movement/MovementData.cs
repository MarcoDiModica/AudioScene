using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Player/Movement Data")]
public class MovementData : ScriptableObject
{
    [Header("General Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 5f;
    public float acceleration = 50f;
    public float deceleration = 50f;
    public float airControl = 0.3f;

    [Header("Jumping")]
    public float jumpForce = 10f;
    public float minJumpForce = 5f;
    public float jumpHoldTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float coyoteTime = 0.2f;
    public float jumpCooldown = 0.1f;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 45f;
    public float slopeCheckDistance = 0.5f;
    public float uphillSpeedMultiplier = 0.75f;
    public float downhillSpeedMultiplier = 1.4f;

    [Header("Physics")]
    public float gravity = -20f;
    public float maxFallSpeed = -20f;

    [Header("Ground Detection")]
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayers;
}