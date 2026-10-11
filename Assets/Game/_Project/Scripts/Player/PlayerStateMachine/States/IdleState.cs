using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class IdleState : IPlayerState
{
    private readonly PlayerStateMachine ctx;
    private const float DeadZone = 0.3f;
    public IdleState(PlayerStateMachine currentContext) => ctx = currentContext;

    public void Enter()
    {
        ctx.Velocity = new Vector3(0f, -5f, 0f);
        // ctx.Animator.Play("Idle");
    }

    public void Update()
    {
        if (ctx.CheckAndExecuteDash()) return;

        Vector2 input = ctx.MoveAction.action.ReadValue<Vector2>();

        // 1. Check JUMP FIRST (Handles Up, Up-Forward, and Up-Backward)
        if (input.y > DeadZone)
        {
            ctx.SwitchState(ctx.JumpState);
            return;
        }

        // 2. Check Crouch
        if (input.y < -DeadZone)
        {
            ctx.SwitchState(ctx.CrouchState);
            return;
        }

        // 3. Check Walk
        if (Mathf.Abs(input.x) > DeadZone)
        {
            ctx.SwitchState(ctx.WalkState);
            return;
        }

    }

    public void FixedUpdate() { }
    public void Exit() { }
}
