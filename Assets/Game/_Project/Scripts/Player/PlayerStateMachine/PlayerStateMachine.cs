using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerStateMachine : MonoBehaviour
{
    private IPlayerState currentState;

    // Shared References
    [HideInInspector] public CharacterController Controller;
    [HideInInspector] public Transform Cam;
    [HideInInspector] public Vector3 Velocity;
    [HideInInspector] public Animator Animator;
    public InputBuffer InputBuffer;

    [Header("Movement Settings")]
    public float WalkSpeed = 4f;
    public float WalkBackwardsSpeed = 3f;

    [Header("Oponent Tracking")]
    public Transform Opponent;

    [Header("Physics Settings")]
    public float JumpForce = 12f;
    public float JumpHorizontalSpeed = 8f;
    public float Gravity = -30f;

    [Header("Dash Settings")]
    public float DashSpeed = 15f;
    public float DashDuration = 0.2f;
    public float DoubleTapWindow = 0.25f;

    [Header("Ground Check Settings")]
    public Transform GroundCheck; // Empty object placed EXACTLY at the character's feet soles
    public float GroundRayDistance = 0.08f; // Tiny ray distance (8cm) below feet
    public LayerMask GroundLayer;
    public bool IsGrounded { get; private set; }

    [Header("Inputs")]
    public InputActionReference MoveAction;

    //------------------------------------------------------------------------------------
    public DashState DashState { get; private set; }
    public IdleState IdleState { get; private set; }
    public WalkState WalkState { get; private set; }
    public CrouchState CrouchState { get; private set; }
    public JumpState JumpState { get; private set; }


    private void Awake()
    {
        if (InputBuffer == null) InputBuffer = GetComponent<InputBuffer>();

        Controller = GetComponent<CharacterController>();
        Cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        IdleState = new IdleState(this);
        WalkState = new WalkState(this);
        CrouchState = new CrouchState(this);
        JumpState = new JumpState(this);
        DashState = new DashState(this);
    }

    private void Start()
    {
        SwitchState(IdleState);
    }

    private void Update()
    {
        CheckGrounded();

        currentState?.Update();
        FaceOpponent();
    }

    public bool CheckAndExecuteDash()
    {
        if (Opponent == null) return false;

        Vector3 DirectionToOpponent = (Opponent.position - transform.position).normalized;
        float FacingSign = Mathf.Sign(DirectionToOpponent.x);

        // Check for explicit direction commands in buffer
        if (InputBuffer.TryConsume("DashRight"))
        {
            DashState.SetDashDirection(1);
            SwitchState(DashState);
            return true;
        }
        if (InputBuffer.TryConsume("DashLeft"))
        {
            DashState.SetDashDirection(-1);
            SwitchState(DashState);
           return true;
        }

        return false;
    }

    private void CheckGrounded()
    {
        Vector3 CheckPos = GroundCheck != null ? GroundCheck.position : transform.position;

        // Direct line down from the feet—no sphere volume to trigger early collisions!
        IsGrounded = Physics.Raycast(CheckPos, Vector3.down, GroundRayDistance, GroundLayer);
    }
    public void SwitchState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    private void FaceOpponent()
    {
        if (Opponent == null) return;

        // Calculate direction along the X axis to the opponent
        Vector3 direction = Opponent.position - transform.position;
        direction.y = 0; // Ignore height difference
        direction.z = 0; // Lock to 2D plane

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
