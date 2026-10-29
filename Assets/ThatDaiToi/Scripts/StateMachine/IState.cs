/// <summary>
/// Giao diện tối giản cho một trạng thái trong state machine.
/// Mỗi state tự xử lý logic vào / cập nhật / thoát của mình.
/// </summary>
public interface IState
{
    void Enter();
    void Tick(float deltaTime);
    void Exit();
}
