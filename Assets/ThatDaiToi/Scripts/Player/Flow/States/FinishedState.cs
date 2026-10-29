/// <summary>Bước cuối: trả lại điều khiển FPS cho người chơi.</summary>
public class FinishedState : PlayerState
{
    public FinishedState(PlayerFlowController ctx) : base(ctx) { }

    public override PlayerFlowPhase Phase => PlayerFlowPhase.Finished;

    public override void Enter()
    {
        base.Enter();
        Ctx.EndScripted();
    }
}
