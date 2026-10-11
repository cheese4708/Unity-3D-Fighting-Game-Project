using UnityEngine;

public class CrouchState : IPlayerState
{
    private readonly PlayerStateMachine ctx;
    public CrouchState(PlayerStateMachine currentContext) => ctx = currentContext;

    public void Enter()
    {
        ctx.Velocity = Vector3.zero;

       // ctx.Animator.Play("Crouch");
    }

    public void Update()
    {
        if (ctx.CheckAndExecuteDash()) return;

        Vector2 input = ctx.MoveAction.action.ReadValue<Vector2>();

        if (input.y >= -0.5f)
        {
            ctx.SwitchState(ctx.IdleState);
        }
    }

    public void FixedUpdate() { }
    public void Exit() { }
}
