using UnityEngine;

/// <summary>Bước 5: ngồi uống nước. Hạ camera xuống, quay mặt ra bàn và chờ drinkDuration giây.</summary>
public class SittingState : PlayerState
{
    public IState Next;

    public SittingState(PlayerFlowController ctx) : base(ctx) { }

    public override PlayerFlowPhase Phase => PlayerFlowPhase.Sitting;

    public override void Enter()
    {
        base.Enter();
        Ctx.StopAgent();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (Ctx.seatPoint != null)
        {
            Ctx.FaceDirection(Ctx.seatPoint.forward, deltaTime, Ctx.turnSpeed);
        }

        float blend = Ctx.sitBlendTime <= 0f ? 1f : Mathf.Clamp01(Elapsed / Ctx.sitBlendTime);
        Ctx.SetCameraDrop(blend);

        if (Elapsed >= Ctx.drinkDuration)
        {
            Ctx.ChangeState(Next);
        }
    }

    public override void Exit()
    {
        // Đứng dậy trước khi đi tiếp.
        Ctx.SetCameraDrop(0f);
    }
}
