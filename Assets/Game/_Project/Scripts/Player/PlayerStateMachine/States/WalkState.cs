using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class WalkState : IPlayerState
{
    private readonly PlayerStateMachine ctx;
    private const float DeadZone = 0.3f;
    public WalkState(PlayerStateMachine currentContext) => ctx = currentContext;

    public void Enter()
    {
        //ctx.Animator.Play("Walk");
    }

    public void Update()
    {
        if (ctx.CheckAndExecuteDash()) return;

        Vector2 input = ctx.MoveAction.action.ReadValue<Vector2>();

        // 1. Jump out of walk
        if (input.y > DeadZone)
        {
            ctx.SwitchState(ctx.JumpState);
            return;
        }

        // 2. Crouch out of walk
        if (input.y < -DeadZone)
        {
            ctx.SwitchState(ctx.CrouchState);
            return;
        }

        // 3. Stop walking if horizontal released
        if (Mathf.Abs(input.x) <= DeadZone)
        {
            ctx.SwitchState(ctx.IdleState);
            return;
        }

        // Determine if walking forward or backward relative to the opponent
        Vector3 directionToOpponent = (ctx.Opponent.position - ctx.transform.position).normalized;
        float inputDirection = Mathf.Sign(input.x);
        float facingDirection = Mathf.Sign(directionToOpponent.x);

        float currentSpeed = (inputDirection == facingDirection) ? ctx.WalkSpeed : ctx.WalkBackwardsSpeed;

        // Move the character strictly along the X axis
        ctx.Velocity = new Vector3(input.x * currentSpeed, ctx.Velocity.y, 0f);
        ctx.Controller.Move(ctx.Velocity * Time.deltaTime);
    }

    public void FixedUpdate() { }
    public void Exit() { }
}
