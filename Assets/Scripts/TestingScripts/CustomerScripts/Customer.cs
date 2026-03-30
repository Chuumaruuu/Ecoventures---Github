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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = true;
        agent.stoppingDistance = 0.1f;

        // 🔥 DISABLE avoidance so customers DON'T dodge each other
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.avoidancePriority = 50;

        originalSpeed = agent.speed;
        originalRadius = agent.radius;

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

        agent.autoRepath = true;

        // 🔥 OPTIONAL: allow physical pass-through
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void Update()
    {
        switch (state)
        {
            case CustomerState.Moving:
                CheckArrival();
                break;

            case CustomerState.Waiting:
                HandleWaiting();
                FaceTable();
                break;

            case CustomerState.Leaving:
                CheckArrival();
                break;
        }

        HandleAnimation();
    }

    void CheckArrival()
    {
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

            LeaveHappy();
        }
    }

    void LeaveHappy() => BeginLeaving();
    void LeaveAngry() => BeginLeaving();

    void BeginLeaving()
    {
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

        float speed = agent.velocity.magnitude;

        animator.SetBool("Walk", speed > 0.1f);
        animator.SetBool("Idle", speed <= 0.1f);
    }

    void SetWaitingBehavior()
    {
        agent.isStopped = true;
        agent.speed = 0f;
        agent.avoidancePriority = waitingPriority;
        agent.radius = originalRadius;
    }

    void SetLeavingBehavior()
    {
        agent.isStopped = false;
        agent.speed = originalSpeed;
        agent.avoidancePriority = leavingPriority;
        agent.radius = originalRadius;
    }
}