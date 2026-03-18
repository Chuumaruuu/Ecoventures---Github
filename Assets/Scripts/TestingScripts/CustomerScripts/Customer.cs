using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public enum CustomerState
{
    Moving,
    Waiting,
    Leaving,
    Served,
    Left
}

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour, IDropHandler
{
    [Header("References")]
    public Transform destination;
    public Transform exitPoint;
    public ShopTable shopTable;

    private Transform assignedSpot;

    [Header("Patience")]
    public float maxWaitTime = 10f;
    public float waitTimer;

    [Header("Order")]
    public Item_Data desiredItem;

    [HideInInspector]
    public CustomerOrderUI orderUI;

    public CustomerState state = CustomerState.Moving;

    private NavMeshAgent agent;
    private Animator animator;

    [Header("Avoidance Settings")]
    public int waitingPriority = 5;
    public int movingPriorityMin = 20;
    public int movingPriorityMax = 40;
    public int leavingPriority = 90;

    private float originalSpeed;
    private float originalRadius;

    // ── Blocking detection ──────────────────────────────────────────────────
    [Header("Block Detection")]
    [Tooltip("Radius of the overlap sphere cast in front of this customer")]
    public float blockCheckRadius = 0.5f;
    [Tooltip("How far ahead to cast the check")]
    public float blockCheckDistance = 0.9f;
    [Tooltip("Seconds to wait before rechecking after being unblocked")]
    public float resumeDelay = 0.15f;

    private bool isBlocked = false;
    private float resumeTimer = 0f;
    // ───────────────────────────────────────────────────────────────────────

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = true;
        agent.stoppingDistance = 0.1f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        originalSpeed = agent.speed;
        originalRadius = agent.radius;

        agent.avoidancePriority = Random.Range(movingPriorityMin, movingPriorityMax);

        assignedSpot = CustomerQueue.Instance.RequestSpot(this);

        if (assignedSpot == null)
        {
            destination = exitPoint;
            state = CustomerState.Leaving;
            SetLeavingBehavior();
        }
        else
        {
            destination = assignedSpot;
            state = CustomerState.Moving;
        }

        if (destination != null)
            agent.SetDestination(destination.position);

        agent.radius *= 1.1f;
        agent.height = 2f;
        agent.baseOffset = 0f;
        agent.autoRepath = true;
        agent.autoTraverseOffMeshLink = true;
    }

    private void Update()
    {
        switch (state)
        {
            case CustomerState.Moving:
                HandleBlockDetection();
                CheckArrival();
                break;

            case CustomerState.Waiting:
                HandleWaiting();
                FaceTable();
                break;

            case CustomerState.Leaving:
                HandleBlockDetection();
                CheckArrival();
                break;
        }

        HandleAnimation();
    }

    // ── Block detection ─────────────────────────────────────────────────────

    void HandleBlockDetection()
    {
        // Only check when the agent is supposed to be moving
        if (agent.isStopped && !isBlocked) return;

        bool blockerFound = CheckForBlocker();

        if (blockerFound && !isBlocked)
        {
            // A customer is in the way — go idle
            isBlocked = true;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else if (!blockerFound && isBlocked)
        {
            // Path is clear — small delay then resume
            resumeTimer += Time.deltaTime;
            if (resumeTimer >= resumeDelay)
            {
                isBlocked = false;
                resumeTimer = 0f;
                agent.isStopped = false;

                // Refresh destination so the agent re-paths cleanly
                if (destination != null)
                    agent.SetDestination(destination.position);
            }
        }
    }

    bool CheckForBlocker()
    {
        // Cast a sphere slightly ahead in the direction of travel
        Vector3 moveDir = agent.desiredVelocity.sqrMagnitude > 0.01f
            ? agent.desiredVelocity.normalized
            : transform.forward;

        Vector3 origin = transform.position + Vector3.up * 0.5f; // chest height
        Vector3 center = origin + moveDir * blockCheckDistance;

        Collider[] hits = Physics.OverlapSphere(center, blockCheckRadius);
        foreach (Collider col in hits)
        {
            if (col.gameObject == gameObject) continue;           // skip self
            Customer other = col.GetComponent<Customer>();
            if (other == null) continue;                          // only care about customers

            // Only be blocked by customers who are stationary (waiting or also blocked)
            if (other.state == CustomerState.Waiting ||
                other.isBlocked ||
                other.agent.velocity.magnitude < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    // ───────────────────────────────────────────────────────────────────────

    void CheckArrival()
    {
        if (isBlocked) return; // don't trigger arrival while standing still due to blocker

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (state == CustomerState.Leaving)
                CleanupAndLeave();
            else if (state == CustomerState.Moving)
                Arrived();
        }
    }

    void Arrived()
    {
        isBlocked = false;
        state = CustomerState.Waiting;
        waitTimer = maxWaitTime;
        agent.ResetPath();
        SetWaitingBehavior();
        ShowOrderUI();
    }

    void HandleWaiting()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f)
            LeaveAngry();
    }

    void FaceTable()
    {
        if (shopTable == null) return;
        Vector3 dir = shopTable.transform.position - transform.position;
        dir.y = 0f;
        if (dir == Vector3.zero) return;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5f * Time.deltaTime);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (state != CustomerState.Waiting) return;

        DraggableItem dragged = eventData.pointerDrag?.GetComponent<DraggableItem>();
        if (dragged == null) return;

        if (dragged.itemData == desiredItem)
        {
            dragged.wasAccepted = true;

            int price = dragged.itemData.sellprice;
            CurrencyManager.AddMoney(price);

            dragged.sourceSlot.TakeItem();

            state = CustomerState.Served;

            CustomerTaskManager.Instance.RegisterServed();

            //Play item received sound here

            LeaveHappy();
        }
    }

    void LeaveHappy() => BeginLeaving();
    void LeaveAngry() => BeginLeaving();

    void BeginLeaving()
    {
        isBlocked = false;
        CustomerQueue.Instance.ReleaseSpot(this);

        if (orderUI != null)
            Destroy(orderUI.gameObject);

        state = CustomerState.Leaving;
        SetLeavingBehavior();

        destination = exitPoint;
        if (destination != null)
            agent.SetDestination(destination.position);
    }

    void CleanupAndLeave()
    {
        bool wasServed = (state == CustomerState.Served);
        state = CustomerState.Left;

        if (CustomerTaskManager.Instance != null && !wasServed)
            CustomerTaskManager.Instance.RegisterCustomerExit();

        Destroy(gameObject);
    }

    void ShowOrderUI()
    {
        if (orderUI != null)
        {
            orderUI.gameObject.SetActive(true);
            orderUI.Setup(this);
        }
    }

    void HandleAnimation()
    {
        if (animator == null) return;

        // Treat "blocked/idle" the same as standing still for animation
        float speed = isBlocked ? 0f : agent.velocity.magnitude;
        animator.SetBool("Walk", speed > 0.1f);
        animator.SetBool("Idle", speed <= 0.1f);
    }

    void SetWaitingBehavior()
    {
        agent.isStopped = true;
        agent.speed = 0f;
        agent.avoidancePriority = waitingPriority;
        agent.radius = originalRadius * 1.2f;
    }

    void SetLeavingBehavior()
    {
        agent.isStopped = false;
        agent.speed = originalSpeed;
        agent.avoidancePriority = leavingPriority;
        agent.radius = originalRadius;
    }
}