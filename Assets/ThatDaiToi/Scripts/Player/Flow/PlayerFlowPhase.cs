/// <summary>Các bước của flow người chơi, dùng để query / debug từ bên ngoài.</summary>
public enum PlayerFlowPhase
{
    None,
    Spawn,
    MoveToOrder,
    Ordering,
    MoveToSeat,
    Sitting,
    Returning,
    Finished
}
