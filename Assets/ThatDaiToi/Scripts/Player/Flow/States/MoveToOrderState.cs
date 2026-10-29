using UnityEngine;

/// <summary>Bước 2: đi tới điểm gọi món bằng NavMeshAgent.</summary>
public class MoveToOrderState : PlayerState
{
    public IState Next;

    public MoveToOrderState(PlayerFlowController ctx) : base(ctx) { }

    public override PlayerFlowPhase Phase => PlayerFlowPhase.MoveToOrder;

    public override void Enter()
    {
        base.Enter();

        if (!Ctx.MoveTo(Ctx.orderPoint))
        {
            // Không có đường tới quầy thì bỏ qua bước di chuyển.
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
            Debug.LogWarning("[PlayerFlow] Đi tới quầy quá lâu, bỏ qua bước này. Kiểm tra NavMesh / vật cản.", Ctx);
            Ctx.StopAgent();
            Ctx.ChangeState(Next);
        }
    }
}
