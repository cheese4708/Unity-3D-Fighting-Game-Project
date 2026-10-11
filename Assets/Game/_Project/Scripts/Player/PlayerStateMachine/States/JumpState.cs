using UnityEngine;

public class JumpState : IPlayerState
{
    private readonly PlayerStateMachine ctx;
    private const float VerticalDeadzone = 0.4f;
    private const float HorizontalDeadzone = 0.3f; // Low threshold for easy diagonal detection
    public JumpState(PlayerStateMachine currentContext) => ctx = currentContext;

    public void Enter()
    {
        // Read Input as jump starts
        Vector2 input = ctx.MoveAction.action.ReadValue<Vector2>();

        // Lock in vertical jump and horizontal air speed
        ctx.Velocity.y = ctx.JumpForce;

        if (Mathf.Abs(input.x) > HorizontalDeadzone)
        {
            Vector3 directionToOpponent = (ctx.Opponent.position - ctx.transform.position).normalized;
            float inputDirection = Mathf.Sign(input.x);
            float facingDirection = Mathf.Sign(directionToOpponent.x);

            bool isJumpingForward = (inputDirection == facingDirection);
            float airSpeed = isJumpingForward ? ctx.JumpHorizontalSpeed : ctx.JumpHorizontalSpeed * 0.8f;

            ctx.Velocity.x = inputDirection * airSpeed;
           // ctx.Animator.Play(isJumpingForward ? "JumpForward" : "JumpBackward");
        } else
        {
            ctx.Velocity.x = 0f;
            //ctx.Animator.Play("JumpNeutral");
        }

        ctx.Velocity.z = 0f;
    }

    public void Update()
    {
        // Apply grav
        ctx.Velocity.y += ctx.Gravity * Time.deltaTime;

        // Move char
        ctx.Controller.Move(ctx.Velocity * Time.deltaTime);

        // Landing & Input Buffer Check
        if (ctx.Velocity.y <= 0f && (ctx.IsGrounded || ctx.Controller.isGrounded))
        {
            Vector2 heldInput = ctx.MoveAction.action.ReadValue<Vector2>();

            // CHAIN JUMP: Holding Up (or Up-Diagonal) immediately jumps again
            if (heldInput.y > VerticalDeadzone)
            {
                ctx.SwitchState(ctx.JumpState);
            }
            // CONTINUOUS WALK: Holding Left/Right transitions directly into walking
            else if (Mathf.Abs(heldInput.x) > HorizontalDeadzone)
            {
                ctx.SwitchState(ctx.WalkState);
            }
            // CROUCH: Holding Down transitions directly into crouching
            else if (heldInput.y < -VerticalDeadzone)
            {
                ctx.SwitchState(ctx.CrouchState);
            }
            // IDLE: No movement inputs held
            else
            {
                ctx.Velocity = Vector3.zero;
                ctx.SwitchState(ctx.IdleState);
            }
        }
    }

    public void FixedUpdate() { }
    public void Exit() { }
}
