using UnityEngine;

public class DashState : IPlayerState
{
    private readonly PlayerStateMachine ctx;
    private float StateTimer;
    private int DashDirection;
    private float CurrentDashSpeed;

    public DashState(PlayerStateMachine currentContext) => ctx = currentContext;

    public void SetDashDirection(int xDirection)
    {
        DashDirection = xDirection;
    }

    public void Enter()
    {
        StateTimer = 0f;

        // Determine if this is a forward or backwards dash
        Vector3 DirectionToOpponent = (ctx.Opponent.position - ctx.transform.position).normalized;
        float FacingDirection = Mathf.Sign(DirectionToOpponent.x);

        bool IsForwardDash = (DashDirection == FacingDirection);

        // Backdash cover less ground
        CurrentDashSpeed = IsForwardDash ? ctx.DashSpeed : ctx.DashSpeed * 0.85f;
        CurrentDashSpeed *= DashDirection;

        //ctx.Animator.Play(IsForwardDash ? "DashForward" : "DashBackward");
    }

    public void Update()
    {
        StateTimer += Time.deltaTime;

        // Lock velocity to dash speed and ignore Y
        ctx.Velocity = new Vector3(CurrentDashSpeed, 0f, 0f);
        ctx.Controller.Move(ctx.Velocity * Time.deltaTime);

        // Exit state after duration
        if (StateTimer >= ctx.DashDuration)
        {
            Vector2 HeldInput = ctx.MoveAction.action.ReadValue<Vector2>();

            if (HeldInput.y < -0.3f)
                ctx.SwitchState(ctx.CrouchState);
            else if (Mathf.Abs(HeldInput.x) > 0.3f)
                ctx.SwitchState(ctx.WalkState);
            else
                ctx.SwitchState(ctx.IdleState);
        }
    }

    public void FixedUpdate() { }
    public void Exit()
    {
        ctx.Velocity = Vector3.zero;
    }
}
