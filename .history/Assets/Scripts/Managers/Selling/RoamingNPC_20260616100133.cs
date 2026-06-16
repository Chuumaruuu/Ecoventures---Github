using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class RoamingNPC : MonoBehaviour
{
    [Header("Movement")]
    public float waitTimeAtPoint = 2f;
    public float moveSpeed = 2.2f;
    public float acceleration = 6f;
    public float stoppingDistance = 0.6f;

    [Header("Anti-Stuck")]
    public float stuckCheckTime = 2f;
    public float minimumMoveDistance = 0.2f;

    // -------------------------------------------------------
    //  Roam Point Definition
    // -------------------------------------------------------

    [System.Serializable]
    public class RoamPoint
    {
        [Tooltip("Where the NPC stands.")]
        public Transform point;

        [Tooltip("If assigned, the NPC will face this target when it arrives (e.g. a vendor table). " +
                 "Leave null for a regular roam point.")]
        public Transform vendorLookTarget;
    }

    [Header("Roam Points")]
    [Tooltip("Add all destinations here. Assign a Vendor Look Target only for vendor stalls.")]
    public RoamPoint[] roamPoints;

    [Header("Facing")]
    [Tooltip("How fast the NPC rotates to face the vendor table (degrees/sec).")]
    public float facingSpeed = 120f;

    // -------------------------------------------------------
    //  Private state
    // -------------------------------------------------------

    private NavMeshAgent agent;
    private Animator animator;

    private bool isWaiting;
    private bool isFacingTarget;

    private int currentPointIndex = -1;

    private bool isInQueue = false;

    private Vector3 lastPosition;
    private float stuckTimer;

    private static Dictionary<Transform, RoamingNPC> reservedPoints
        = new Dictionary<Transform, RoamingNPC>();

    // -------------------------------------------------------
    //  Unity lifecycle
    // -------------------------------------------------------

    void Start()
    {
        GetComponent<CustomerOrder>().OnCustomerQueued += CustomerWaiting;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        SetupAgent();

        if (roamPoints == null || roamPoints.Length == 0)
        {
            Debug.LogError($"{name}: No roam points assigned!");
            enabled = false;
            return;
        }

        MoveToNextPoint();
    }

    void Update()
    {
        HandleAnimation();
        CheckIfStuck();

        if (isFacingTarget)
            FaceVendorTable();

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
                StartCoroutine(WaitAndMove());
        }
    }
    private void CustomerWaiting()
    {
        isWaiting = true;
        // HandleAnimation();
    }
    void OnDestroy()
    {
        ReleaseCurrentPoint();
    }

    // -------------------------------------------------------
    //  Agent setup
    // -------------------------------------------------------

    void SetupAgent()
    {
        agent.speed = moveSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;

        agent.updateRotation = true;
        agent.autoBraking = true;
        agent.angularSpeed = 400f;

        agent.obstacleAvoidanceType =
            ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        agent.avoidancePriority = Random.Range(45, 65);

        agent.radius *= 1.1f;
        agent.height = 2f;
        agent.baseOffset = 0f;

        agent.autoRepath = true;
        agent.autoTraverseOffMeshLink = true;
    }

    // -------------------------------------------------------
    //  Vendor facing
    // -------------------------------------------------------

    Transform GetCurrentLookTarget()
    {
        if (currentPointIndex < 0) return null;
        return roamPoints[currentPointIndex].vendorLookTarget;
    }

    void FaceVendorTable()
    {
        Transform lookTarget = GetCurrentLookTarget();
        if (lookTarget == null) { isFacingTarget = false; return; }

        Vector3 direction = lookTarget.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) { isFacingTarget = false; return; }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, facingSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            isFacingTarget = false;
    }

    // -------------------------------------------------------
    //  Animation
    // -------------------------------------------------------

    void HandleAnimation()
    {
        if (isWaiting)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
            return;
        }

        bool moving = agent.velocity.magnitude > 0.15f;
        animator.SetBool("Walk", moving);
        animator.SetBool("Idle", !moving);
    }

    // -------------------------------------------------------
    //  Anti-stuck
    // -------------------------------------------------------

    void CheckIfStuck()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            float moved = Vector3.Distance(transform.position, lastPosition);

            if (moved < minimumMoveDistance)
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer >= stuckCheckTime)
                {
                    ForceRepath();
                    stuckTimer = 0f;
                }
            }
            else
            {
                stuckTimer = 0f;
            }

            lastPosition = transform.position;
        }
    }

    void ForceRepath()
    {
        if (currentPointIndex >= 0)
        {
            agent.ResetPath();
            agent.SetDestination(roamPoints[currentPointIndex].point.position);
        }
    }

    // -------------------------------------------------------
    //  Navigation
    // -------------------------------------------------------

    /// <summary>
    /// Picks a random available roam point (regular OR vendor) and moves there.
    /// </summary>
    void MoveToNextPoint()
    {
        List<int> available = new List<int>();

        for (int i = 0; i < roamPoints.Length; i++)
        {
            Transform pt = roamPoints[i].point;
            if (pt != null && !reservedPoints.ContainsKey(pt))
                available.Add(i);
        }

        int chosen = available.Count > 0
            ? available[Random.Range(0, available.Count)]
            : Random.Range(0, roamPoints.Length);

        SetDestination(chosen);
    }

    public void ResumeRoaming()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        enabled = true;
        isInQueue = false;
        isWaiting = false;
        isFacingTarget = false;

        if (agent != null)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
        }

        MoveToNextPoint();
    }

    void SetDestination(int index)
    {
        ReleaseCurrentPoint();

        currentPointIndex = index;
        Transform target = roamPoints[index].point;

        reservedPoints[target] = this;
        agent.SetDestination(target.position);
    }

    void ReleaseCurrentPoint()
    {
        if (currentPointIndex < 0) return;

        Transform pt = roamPoints[currentPointIndex].point;
        if (pt != null &&
            reservedPoints.TryGetValue(pt, out RoamingNPC owner) &&
            owner == this)
        {
            reservedPoints.Remove(pt);
        }
    }

    // -------------------------------------------------------
    //  Wait coroutine
    // -------------------------------------------------------

    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        agent.isStopped = true;

        // If this is a vendor point, hand off rotation control and face the table
        bool isVendorPoint = GetCurrentLookTarget() != null;
        if (isVendorPoint)
        {
            CustomerOrder customerOrder = GetComponent<CustomerOrder>();
            if (CustomerQueue.Instance != null && customerOrder != null && CustomerQueue.Instance.CanJoinQueue())
            {
                CustomerQueue.Instance.AddCustomer(customerOrder);

                // mark as queued, release vendor reservation so others can use it,
                // and allow the agent to start moving to the assigned queue destination
                isInQueue = true;
                ReleaseCurrentPoint();
                agent.isStopped = false;

                // stop roaming behavior for this NPC to avoid overriding queue destination
                enabled = false;

                yield break; // exit coroutine - NPC is now handled by queue logic
            }

            agent.updateRotation = false;
            isFacingTarget = true;
        }

        yield return new WaitForSeconds(waitTimeAtPoint);

        // Restore NavMesh rotation control before moving again
        agent.updateRotation = true;
        isFacingTarget = false;
        agent.isStopped = false;
        isWaiting = false;

        MoveToNextPoint();
    }
}