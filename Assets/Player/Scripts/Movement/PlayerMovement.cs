using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MovementData movementData;

    private CharacterController characterController;
    private PlayerReferences playerReferences;
    private SlopeHandler slopeHandler;

    private Vector3 currentVelocity;
    private Vector3 moveVelocity;
    private Vector3 verticalVelocity;
    private float currentSpeed;
    private float targetSpeed;
    private float speedChangeFactor;

    private void Awake()
    {
        playerReferences = GetComponent<PlayerReferences>();
        characterController = playerReferences.characterController;
        slopeHandler = GetComponent<SlopeHandler>();
    }

    private void Update()
    {
        ApplyGravity();
    }

    public void Move(Vector3 moveDirection, float targetSpeed, float acceleration)
    {
        this.targetSpeed = targetSpeed;
        speedChangeFactor = acceleration;

        if (slopeHandler.IsOnSlope)
        {
            targetSpeed = ModifySpeedBasedOnSlope(targetSpeed);
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed * moveDirection.magnitude,
            speedChangeFactor * Time.deltaTime);

        if (moveDirection.magnitude > 0.1f)
        {
            moveVelocity = moveDirection * currentSpeed;

            if (slopeHandler.IsOnSlope)
            {
                moveVelocity = slopeHandler.GetSlopeMoveDirection(moveVelocity);
            }
        }
        else
        {
            moveVelocity = Vector3.MoveTowards(moveVelocity, Vector3.zero,
                speedChangeFactor * Time.deltaTime);
        }

        currentVelocity = moveVelocity + verticalVelocity;

        characterController.Move(currentVelocity * Time.deltaTime);
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity = Vector3.up * jumpForce;
    }

    private float ModifySpeedBasedOnSlope(float speed)
    {
        float slopeAngle = slopeHandler.SlopeAngle;

        if (slopeHandler.IsGoingUphill)
        {
            float slopeFactor = Mathf.Lerp(1f, movementData.uphillSpeedMultiplier,
                slopeAngle / movementData.maxSlopeAngle);
            return speed * slopeFactor;
        }
        else
        {
            float slopeFactor = Mathf.Lerp(1f, movementData.downhillSpeedMultiplier,
                slopeAngle / movementData.maxSlopeAngle);
            return speed * slopeFactor;
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity = new Vector3(0, -2f, 0);
        }
        else
        {
            verticalVelocity += Physics.gravity * Time.deltaTime;

            if (verticalVelocity.y < movementData.maxFallSpeed)
                verticalVelocity = new Vector3(0, movementData.maxFallSpeed, 0);
        }
    }

    public void ResetVerticalVelocity()
    {
        verticalVelocity = Vector3.zero;
    }

    public Vector3 CurrentVelocity => currentVelocity;
    public Vector3 MoveVelocity => moveVelocity;
    public Vector3 VerticalVelocity => verticalVelocity;
    public float CurrentSpeed => currentSpeed;
}