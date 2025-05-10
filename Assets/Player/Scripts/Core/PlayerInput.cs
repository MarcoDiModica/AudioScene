using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private UnityEngine.InputSystem.PlayerInput playerInput;
    
    [Header("Camera Reference")]
    [SerializeField] private Camera playerCamera;

    private Vector2 movementInput;
    private bool jumpInput;
    private bool jumpInputHeld;

    private Vector3 moveDirection;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        CalculateMoveDirection();
    }

    private void CalculateMoveDirection()
    {
        if (movementInput.magnitude <= 0.1f)
        {
            moveDirection = Vector3.zero;
            return;
        }

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        moveDirection = forward * movementInput.y + right * movementInput.x;

        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jumpInput = value.isPressed;
        jumpInputHeld = value.isPressed;
    }

    public bool ConsumeJumpInput()
    {
        bool jump = jumpInput;
        jumpInput = false;
        return jump;
    }

    public Vector3 MoveDirection => moveDirection;
    public Vector2 MovementInput => movementInput;
    public float MovementInputMagnitude => movementInput.magnitude;
    public bool JumpInput => jumpInput;
    public bool JumpInputHeld => jumpInputHeld;
}