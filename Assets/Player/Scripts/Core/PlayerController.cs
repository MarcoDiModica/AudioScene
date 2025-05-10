using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementData movementData;

    private PlayerStateMachine stateMachine;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerReferences playerReferences;
    private GroundDetector groundDetector;
    private SlopeHandler slopeHandler;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerReferences = GetComponent<PlayerReferences>();
        groundDetector = GetComponent<GroundDetector>();
        slopeHandler = GetComponent<SlopeHandler>();

        stateMachine = new PlayerStateMachine(this, playerInput, playerMovement,
            groundDetector, slopeHandler, playerReferences, movementData);
    }

    private void Start()
    {
        stateMachine.Initialize();
    }

    private void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        groundDetector.CheckGround();
        slopeHandler.HandleSlope();
        stateMachine.CurrentState.PhysicsUpdate();
    }

    public PlayerStateMachine StateMachine => stateMachine;
    public PlayerInput Input => playerInput;
    public PlayerMovement Movement => playerMovement;
    public PlayerReferences References => playerReferences;
    public GroundDetector GroundDetector => groundDetector;
    public SlopeHandler SlopeHandler => slopeHandler;
    public MovementData MovementData => movementData;
}