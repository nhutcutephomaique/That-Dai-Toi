using System;

/// <summary>
/// State machine dùng chung, không phụ thuộc MonoBehaviour.
/// Chỉ giữ state hiện tại và gọi Enter / Tick / Exit tương ứng.
/// </summary>
public class StateMachine
{
    public IState Current { get; private set; }

    /// <summary>Bắn ra sau khi đã đổi Current nhưng trước khi gọi Enter của state mới.</summary>
    public event Action<IState> StateChanged;

    /// <summary>Đổi state. Bỏ qua nếu null hoặc trùng state đang chạy.</summary>
    public void ChangeState(IState next)
    {
        if (next == null || ReferenceEquals(next, Current))
        {
            return;
        }

        Current?.Exit();
        Current = next;
        StateChanged?.Invoke(Current);
        Current.Enter();
    }

    public void Tick(float deltaTime)
    {
        Current?.Tick(deltaTime);
    }
}
