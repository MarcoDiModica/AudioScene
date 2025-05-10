public class PlayerStateMachine
{
    public PlayerBaseState CurrentState { get; private set; }
    public PlayerStateFactory StateFactory { get; private set; }

    private PlayerController playerController;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private GroundDetector groundDetector;
    private SlopeHandler slopeHandler;
    private PlayerReferences playerReferences;
    private MovementData movementData;

    public PlayerStateMachine(PlayerController playerController, PlayerInput playerInput,
        PlayerMovement playerMovement, GroundDetector groundDetector, SlopeHandler slopeHandler,
        PlayerReferences playerReferences, MovementData movementData)
    {
        this.playerController = playerController;
        this.playerInput = playerInput;
        this.playerMovement = playerMovement;
        this.groundDetector = groundDetector;
        this.slopeHandler = slopeHandler;
        this.playerReferences = playerReferences;
        this.movementData = movementData;

        StateFactory = new PlayerStateFactory(this);
    }

    public void Initialize()
    {
        CurrentState = StateFactory.Idle();
        CurrentState.EnterState();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

    public PlayerController PlayerController => playerController;
    public PlayerInput PlayerInput => playerInput;
    public PlayerMovement PlayerMovement => playerMovement;
    public GroundDetector GroundDetector => groundDetector;
    public SlopeHandler SlopeHandler => slopeHandler;
    public PlayerReferences PlayerReferences => playerReferences;
    public MovementData MovementData => movementData;
}