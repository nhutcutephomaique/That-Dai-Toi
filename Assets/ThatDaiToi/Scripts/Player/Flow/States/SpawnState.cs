using UnityEngine;

/// <summary>Bước 1: đặt player về điểm spawn, chờ một chút rồi bắt đầu đi tới quầy.</summary>
public class SpawnState : PlayerState
{
    public IState Next;

    public SpawnState(PlayerFlowController ctx) : base(ctx) { }

    public override PlayerFlowPhase Phase => PlayerFlowPhase.Spawn;

    public override void Enter()
    {
        base.Enter();

        Ctx.BeginScripted();
        Ctx.StopAgent();
        Ctx.SetCameraDrop(0f);
        Ctx.TeleportTo(Ctx.spawnPoint, Ctx.orderPoint);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (Elapsed >= Ctx.spawnDelay)
        {
            Ctx.ChangeState(Next);
        }
    }
}
