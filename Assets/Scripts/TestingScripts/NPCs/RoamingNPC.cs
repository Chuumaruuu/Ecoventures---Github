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

    [Header("Roam Points")]
    public Transform[] roamPoints;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isWaiting;

    private int currentPointIndex = -1;

    private Vector3 lastPosition;
    private float stuckTimer;

    private static Dictionary<Transform, RoamingNPC> reservedPoints
        = new Dictionary<Transform, RoamingNPC>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        SetupAgent();

        if (roamPoints.Length == 0)
        {
            Debug.LogError("No roam points assigned!");
            enabled = false;
            return;
        }

        MoveToAvailablePoint();
    }

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

        // Roaming NPC priority slightly lower than moving customer
        agent.avoidancePriority = Random.Range(45, 65);

        // Personal space
        agent.radius *= 1.1f;
        agent.height = 2f;
        agent.baseOffset = 0f;

        agent.autoRepath = true;
        agent.autoTraverseOffMeshLink = true;
    }

    void Update()
    {
        HandleAnimation();
        CheckIfStuck();

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
                StartCoroutine(WaitAndMove());
        }
    }

    void HandleAnimation()
    {
        if (isWaiting)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
            return;
        }

        if (agent.velocity.magnitude > 0.15f)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
        }
    }

    void CheckIfStuck()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            float moved =
                Vector3.Distance(transform.position, lastPosition);

            if (moved < minimumMoveDistance)
            {
                stuckTimer += Time.deltaTime;

                if (stuckTimer >= stuckCheckTime)
                {
                    ForceRepath();
                    stuckTimer = 0;
                }
            }
            else
            {
                stuckTimer = 0;
            }

            lastPosition = transform.position;
        }
    }

    void ForceRepath()
    {
        if (currentPointIndex >= 0)
        {
            agent.ResetPath();
            agent.SetDestination(
                roamPoints[currentPointIndex].position);
        }
    }

    void MoveToAvailablePoint()
    {
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < roamPoints.Length; i++)
        {
            if (!reservedPoints.ContainsKey(roamPoints[i]))
                availableIndexes.Add(i);
        }

        int chosenIndex;

        if (availableIndexes.Count == 0)
            chosenIndex = Random.Range(0, roamPoints.Length);
        else
            chosenIndex =
                availableIndexes[Random.Range(0, availableIndexes.Count)];

        SetDestination(chosenIndex);
    }

    void SetDestination(int index)
    {
        if (currentPointIndex >= 0)
        {
            Transform previous = roamPoints[currentPointIndex];

            if (reservedPoints.ContainsKey(previous)
                && reservedPoints[previous] == this)
            {
                reservedPoints.Remove(previous);
            }
        }

        currentPointIndex = index;
        Transform target = roamPoints[index];

        reservedPoints[target] = this;

        agent.SetDestination(target.position);
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;

        agent.isStopped = true;
        yield return new WaitForSeconds(waitTimeAtPoint);

        agent.isStopped = false;
        isWaiting = false;

        MoveToAvailablePoint();
    }

    void OnDestroy()
    {
        if (currentPointIndex >= 0)
        {
            Transform point = roamPoints[currentPointIndex];

            if (reservedPoints.ContainsKey(point)
                && reservedPoints[point] == this)
            {
                reservedPoints.Remove(point);
            }
        }
    }
}