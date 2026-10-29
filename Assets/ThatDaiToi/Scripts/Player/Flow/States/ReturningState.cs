using UnityEngine;

/// <summary>Bước 6: uống xong thì đi bộ về chỗ spawn.</summary>
public class ReturningState : PlayerState
{
    public IState Next;
    public IState Finished;

    public ReturningState(PlayerFlowController ctx) : base(ctx) { }

    public override PlayerFlowPhase Phase => PlayerFlowPhase.Returning;

    public override void Enter()
    {
        base.Enter();
        Ctx.SetCameraDrop(0f);

        if (!Ctx.MoveTo(Ctx.spawnPoint))
        {
            Ctx.ChangeState(Ctx.loop ? Next : Finished);
        }
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (Ctx.HasReachedDestination())
        {
            Ctx.StopAgent();
            Ctx.ChangeState(Ctx.loop ? Next : Finished);
            return;
        }

        if (Elapsed >= Ctx.travelTimeout)
        {
            Debug.LogWarning("[PlayerFlow] Đi về spawn quá lâu, kết thúc flow. Kiểm tra NavMesh / vật cản.", Ctx);
            Ctx.StopAgent();
            Ctx.ChangeState(Ctx.loop ? Next : Finished);
        }
    }
}
