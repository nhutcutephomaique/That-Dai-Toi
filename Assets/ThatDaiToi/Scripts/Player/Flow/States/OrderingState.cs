using UnityEngine;

/// <summary>Bước 3: đứng tại quầy, quay mặt vào quầy và chờ trong orderDuration giây.</summary>
public class OrderingState : PlayerState
{
    public IState Next;

    public OrderingState(PlayerFlowController ctx) : base(ctx) { }

    public override PlayerFlowPhase Phase => PlayerFlowPhase.Ordering;

    public override void Enter()
    {
        base.Enter();
        Ctx.StopAgent();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        // orderPoint.forward là hướng mũi tên xanh của marker, trỏ vào quầy.
        if (Ctx.orderPoint != null)
        {
            Ctx.FaceDirection(Ctx.orderPoint.forward, deltaTime, Ctx.turnSpeed);
        }

        if (Elapsed >= Ctx.orderDuration)
        {
            Ctx.ChangeState(Next);
        }
    }
}
