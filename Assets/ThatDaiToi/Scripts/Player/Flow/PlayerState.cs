/// <summary>
/// State base cho flow của player, giữ tham chiếu tới controller và đồng hồ riêng.
/// State con nhớ gọi base.Tick(deltaTime) nếu muốn dùng Elapsed.
/// </summary>
public abstract class PlayerState : IState
{
    protected readonly PlayerFlowController Ctx;
    protected float Elapsed;

    protected PlayerState(PlayerFlowController ctx)
    {
        Ctx = ctx;
    }

    public abstract PlayerFlowPhase Phase { get; }

    public virtual void Enter()
    {
        Elapsed = 0f;
    }

    public virtual void Tick(float deltaTime)
    {
        Elapsed += deltaTime;
    }

    public virtual void Exit()
    {
    }
}
