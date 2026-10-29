using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Điều khiển flow: Spawn -> tới quầy gọi món -> order -> tới chỗ ngồi -> uống nước -> về chỗ spawn.
/// Khi flow chạy, script tắt điều khiển FPS và dùng NavMeshAgent để di chuyển.
/// Khi flow kết thúc, NavMeshAgent bị tắt và trả lại điều khiển cho người chơi.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[DisallowMultipleComponent]
public class PlayerFlowController : MonoBehaviour
{
    [Header("Tham chiếu (để trống sẽ tự tìm)")]
    public NavMeshAgent agent;
    public Rigidbody body;
    public FirstPersonMovement movement;
    public FirstPersonLook look;
    public Crouch crouch;
    public Jump jump;
    public Transform cameraTransform;

    [Header("Điểm mốc trong scene")]
    [Tooltip("Nơi player xuất hiện và cũng là nơi quay về.")]
    public Transform spawnPoint;

    [Tooltip("Nơi player đứng để gọi món. Xoay trục Z (mũi tên xanh) hướng về quầy.")]
    public Transform orderPoint;

    [Tooltip("Nơi player ngồi uống nước. Xoay trục Z (mũi tên xanh) hướng ra bàn.")]
    public Transform seatPoint;

    [Header("Thời gian từng bước")]
    [Min(0f)] public float spawnDelay = 1f;
    [Min(0f)] public float orderDuration = 2.5f;
    [Min(0f)] public float drinkDuration = 4f;
    [Min(0.1f)] public float arriveDistance = 0.4f;
    [Tooltip("Nếu đi quá lâu (kẹt đường) thì tự bỏ qua bước hiện tại.")]
    [Min(1f)] public float travelTimeout = 25f;

    [Header("Hành vi")]
    public bool playOnStart = true;
    [Tooltip("Bật để sau khi về spawn lại chạy lại từ đầu.")]
    public bool loop = false;
    [Min(1f)] public float turnSpeed = 8f;

    [Header("Ngồi xuống")]
    [Tooltip("Hạ camera bao nhiêu mét khi ngồi.")]
    public float sitCameraDrop = 0.55f;
    [Min(0f)] public float sitBlendTime = 0.35f;

    /// <summary>Bắn ra mỗi khi sang bước mới, kèm phase mới.</summary>
    public event Action<PlayerFlowPhase> PhaseChanged;

    StateMachine machine;
    Vector3 cameraStandLocalPosition;
    bool flowRunning;
    bool hasDestination;
    Vector3 destination;

    IState startState;
    IState finishedState;

    public PlayerFlowPhase Phase =>
        machine != null && machine.Current is PlayerState state ? state.Phase : PlayerFlowPhase.None;

    public bool IsRunning => flowRunning;

    /// <summary>True khi người chơi đang bị flow điều khiển (không tự bấm di chuyển được).</summary>
    public bool ControlLocked => flowRunning;

    void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (body == null) body = GetComponent<Rigidbody>();
        if (movement == null) movement = GetComponent<FirstPersonMovement>();
        if (look == null) look = GetComponentInChildren<FirstPersonLook>(true);
        if (crouch == null) crouch = GetComponent<Crouch>();
        if (jump == null) jump = GetComponent<Jump>();

        if (cameraTransform == null && look != null)
        {
            cameraTransform = look.transform;
        }

        if (cameraTransform == null)
        {
            Camera cam = GetComponentInChildren<Camera>(true);
            if (cam != null) cameraTransform = cam.transform;
        }

        if (cameraTransform != null)
        {
            cameraStandLocalPosition = cameraTransform.localPosition;
        }

        BuildStateMachine();

        // Chỉ bật agent khi flow chạy, tránh giành quyền di chuyển với Rigidbody.
        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    void Start()
    {
        if (playOnStart) StartFlow();
    }

    void Update()
    {
        if (flowRunning)
        {
            machine.Tick(Time.deltaTime);
        }
    }

    void OnDisable()
    {
        if (flowRunning) EndScripted();
    }

    /// <summary>Bắt đầu (hoặc chạy lại) flow từ bước spawn.</summary>
    public void StartFlow()
    {
        if (flowRunning) return;

        if (spawnPoint == null || orderPoint == null || seatPoint == null)
        {
            Debug.LogError("[PlayerFlow] Cần gán đủ spawnPoint, orderPoint và seatPoint trước khi chạy.", this);
            return;
        }

        flowRunning = true;
        machine.ChangeState(startState);
    }

    /// <summary>Dừng flow ngay và trả lại điều khiển cho người chơi.</summary>
    public void StopFlow()
    {
        if (machine == null || !flowRunning) return;

        machine.ChangeState(finishedState);
        EndScripted();
    }

    void BuildStateMachine()
    {
        machine = new StateMachine();
        machine.StateChanged += _ => PhaseChanged?.Invoke(Phase);

        var spawn = new SpawnState(this);
        var moveToOrder = new MoveToOrderState(this);
        var ordering = new OrderingState(this);
        var moveToSeat = new MoveToSeatState(this);
        var sitting = new SittingState(this);
        var returning = new ReturningState(this);
        var finished = new FinishedState(this);

        spawn.Next = moveToOrder;
        moveToOrder.Next = ordering;
        ordering.Next = moveToSeat;
        moveToSeat.Next = sitting;
        sitting.Next = returning;
        returning.Next = spawn;
        returning.Finished = finished;

        startState = spawn;
        finishedState = finished;
    }

    // ---------------------------------------------------------------------
    // API cho các state dùng
    // ---------------------------------------------------------------------

    internal void ChangeState(IState next)
    {
        machine.ChangeState(next);
    }

    /// <summary>Khoá điều khiển FPS và bật NavMeshAgent.</summary>
    internal void BeginScripted()
    {
        if (body != null)
        {
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.isKinematic = true;
        }

        if (movement != null) movement.enabled = false;
        if (look != null) look.enabled = false;
        if (crouch != null) crouch.enabled = false;
        if (jump != null) jump.enabled = false;

        if (agent != null)
        {
            agent.enabled = true;
            agent.updateRotation = true;
            agent.isStopped = false;
        }
    }

    /// <summary>Tắt NavMeshAgent và trả lại điều khiển FPS.</summary>
    internal void EndScripted()
    {
        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        if (body != null)
        {
            body.isKinematic = false;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }

        if (movement != null) movement.enabled = true;
        if (look != null) look.enabled = true;
        if (crouch != null) crouch.enabled = true;
        if (jump != null) jump.enabled = true;

        SetCameraDrop(0f);
        flowRunning = false;
    }

    /// <summary>Dịch chuyển tới một điểm (có sample lại trên NavMesh) và quay mặt về lookAt.</summary>
    internal void TeleportTo(Transform point, Transform lookAt)
    {
        if (point == null) return;

        Quaternion rotation = point.rotation;
        if (lookAt != null)
        {
            Vector3 direction = lookAt.position - point.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
            {
                rotation = Quaternion.LookRotation(direction);
            }
        }

        TeleportTo(point.position, rotation);
    }

    internal void TeleportTo(Vector3 position, Quaternion rotation)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            position = hit.position;
        }

        // Tắt agent trước khi set transform để agent không kéo ngược lại.
        agent.enabled = false;
        transform.SetPositionAndRotation(position, rotation);
        agent.enabled = true;

        if (!agent.Warp(position))
        {
            Debug.LogWarning("[PlayerFlow] Không thể đặt agent lên NavMesh. Kiểm tra đã bake NavMesh chưa.", this);
        }
    }

    /// <summary>Ra lệnh đi tới target. Trả về false nếu không tìm được đường.</summary>
    internal bool MoveTo(Transform target)
    {
        if (target == null) return false;

        destination = target.position;
        hasDestination = true;

        if (!agent.enabled) agent.enabled = true;

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("[PlayerFlow] Agent không đứng trên NavMesh. Hãy bake NavMesh cho sàn quán.", this);
            return false;
        }

        agent.isStopped = false;
        agent.updateRotation = true;
        agent.ResetPath();
        return agent.SetDestination(destination);
    }

    internal void StopAgent()
    {
        hasDestination = false;

        if (agent == null || !agent.enabled) return;

        agent.isStopped = true;
        agent.updateRotation = false;
        agent.ResetPath();
    }

    /// <summary>Đã tới đích chưa (đo cả khoảng cách thực tế lẫn remainingDistance của agent).</summary>
    internal bool HasReachedDestination()
    {
        if (!hasDestination) return false;

        Vector3 self = transform.position;
        Vector3 target = destination;
        self.y = 0f;
        target.y = 0f;

        float tolerance = Mathf.Max(agent.stoppingDistance, arriveDistance) + 0.15f;
        if ((self - target).sqrMagnitude <= tolerance * tolerance) return true;

        if (!agent.enabled || !agent.isOnNavMesh || agent.pathPending || !agent.hasPath) return false;
        return agent.remainingDistance <= tolerance;
    }

    /// <summary>Xoay dần về một hướng trên mặt phẳng ngang.</summary>
    internal void FaceDirection(Vector3 worldDirection, float deltaTime, float speed)
    {
        worldDirection.y = 0f;
        if (worldDirection.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
        float t = 1f - Mathf.Exp(-speed * deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
    }

    /// <summary>0 = đứng, 1 = ngồi hẳn. Chỉ hạ camera, không đổi collider.</summary>
    internal void SetCameraDrop(float amount)
    {
        if (cameraTransform == null) return;

        cameraTransform.localPosition =
            cameraStandLocalPosition + Vector3.down * (sitCameraDrop * Mathf.Clamp01(amount));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (spawnPoint != null) Gizmos.DrawWireSphere(spawnPoint.position, 0.35f);

        Gizmos.color = Color.yellow;
        if (orderPoint != null)
        {
            Gizmos.DrawWireSphere(orderPoint.position, 0.35f);
            Gizmos.DrawRay(orderPoint.position, orderPoint.forward * 1.2f);
        }

        Gizmos.color = Color.cyan;
        if (seatPoint != null)
        {
            Gizmos.DrawWireSphere(seatPoint.position, 0.35f);
            Gizmos.DrawRay(seatPoint.position, seatPoint.forward * 1.2f);
        }

        Gizmos.color = Color.white;
        if (spawnPoint != null && orderPoint != null) Gizmos.DrawLine(spawnPoint.position, orderPoint.position);
        if (orderPoint != null && seatPoint != null) Gizmos.DrawLine(orderPoint.position, seatPoint.position);
        if (seatPoint != null && spawnPoint != null) Gizmos.DrawLine(seatPoint.position, spawnPoint.position);
    }
}
