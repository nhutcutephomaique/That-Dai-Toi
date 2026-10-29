using UnityEngine;

/// <summary>Bước 4: đi từ quầy tới chỗ ngồi.</summary>
public class MoveToSeatState : PlayerState
{
    public IState Next;

    public MoveToSeatState(PlayerFlowController ctx) : base(ctx) { }

    public override PlayerFlowPhase Phase => PlayerFlowPhase.MoveToSeat;

    public override void Enter()
    {
        base.Enter();

        if (!Ctx.MoveTo(Ctx.seatPoint))
        {
            Ctx.ChangeState(Next);
        }
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (Ctx.HasReachedDestination())
        {
            Ctx.StopAgent();
            Ctx.ChangeState(Next);
            return;
        }

        if (Elapsed >= Ctx.travelTimeout)
        {
            Debug.LogWarning("[PlayerFlow] Đi tới chỗ ngồi quá lâu, bỏ qua bước này. Kiểm tra NavMesh / vật cản.", Ctx);
            Ctx.StopAgent();
            Ctx.ChangeState(Next);
        }
    }
}
