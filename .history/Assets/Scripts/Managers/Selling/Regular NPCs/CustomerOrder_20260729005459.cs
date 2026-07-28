using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private float customerWaitTime = 60f;

    protected Item_Data requestedItem;
    protected Image orderImageUI;
    protected UnityEngine.AI.NavMeshAgent agent;
    protected Transform vendorLookTarget;
    protected bool hasOrdered = false;
    public bool isFacingVendor;
    private bool wasFacingVendor = false;
    protected float waitTimer;

    public event System.Action OnCustomerQueued;
    public event System.Action OnFacingVendor;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        if (hasOrdered && requestedItem != null)
        {
            waitTimer += Time.deltaTime;
            if (customerWaitTime > 0f && waitTimer >= customerWaitTime)
            {
                LeaveQueueWithoutOrder();
                return;
            }
        }

        if (vendorLookTarget == null || agent == null)
            return;

        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance + 0.05f)
        {
            isFacingVendor = false;
            agent.updateRotation = true;
            return;
        }

        isFacingVendor = true;
        if (!wasFacingVendor)
        {
            wasFacingVendor = true;
            OnFacingVendor?.Invoke();
        }
        FaceVendorTarget();
        OnCustomerQueued?.Invoke();
    }

    public void MoveToQueue(Vector3 targetPosition, Transform lookTarget = null)
    {
        if (agent == null)
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        vendorLookTarget = lookTarget;
        isFacingVendor = false;
        wasFacingVendor = false;

        if (agent != null)
        {
            agent.updateRotation = true;
            agent.SetDestination(targetPosition);
        }

        if (!hasOrdered)
        {
            GenerateOrder();
            hasOrdered = true;
            waitTimer = 0f;
        }
    }

    public void SetOrderImage(Image image)
    {
        if (orderImageUI != null)
        {
            OrderBubbleButton previousButton = orderImageUI.GetComponent<OrderBubbleButton>();
            if (previousButton != null)
                previousButton.SetCustomer(null);

            orderImageUI.gameObject.SetActive(false);
        }

        orderImageUI = image;

        if (orderImageUI != null)
        {
            OrderBubbleButton bubbleButton = orderImageUI.GetComponent<OrderBubbleButton>();
            if (bubbleButton != null)
                bubbleButton.SetCustomer(this);

            if (requestedItem != null)
            {
                orderImageUI.sprite = requestedItem._itemSprite;
                orderImageUI.gameObject.SetActive(true);
            }
            else
            {
                orderImageUI.gameObject.SetActive(false);
            }
        }
    }

    // virtual: SpecialCustomerOrder generates a list of 3 instead of 1
    protected virtual void GenerateOrder()
    {
        if (OrderGenerator.Instance == null)
        {
            Debug.LogWarning("OrderGenerator instance missing");
            return;
        }

        requestedItem = OrderGenerator.Instance.GetRandomItem();
        if (requestedItem != null && orderImageUI != null)
        {
            orderImageUI.sprite = requestedItem._itemSprite;
            orderImageUI.gameObject.SetActive(true);

            OrderBubbleButton bubbleButton = orderImageUI.GetComponent<OrderBubbleButton>();
            if (bubbleButton != null)
                bubbleButton.SetCustomer(this);
        }
    }

    private void FaceVendorTarget()
    {
        if (vendorLookTarget == null)
        {
            isFacingVendor = false;
            return;
        }

        if (agent != null)
            agent.updateRotation = false;

        Vector3 direction = vendorLookTarget.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            isFacingVendor = false;
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            120f * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            isFacingVendor = false;
    }

    // virtual: SpecialCustomerOrder only registers the sale after all 3 items
    public virtual bool TryCompleteOrder(Item_Data selectedItem)
    {
        if (requestedItem == null || selectedItem == null)
            return false;

        if (selectedItem != requestedItem)
            return false;

        if (SalesTracker.Instance != null)
            SalesTracker.Instance.RegisterSale(requestedItem);
        else
            Debug.LogWarning("SalesTracker instance missing when completing order");

        LeaveQueueAndResume();
        ResetOrderState();
        return true;
    }

    // virtual: SpecialCustomerOrder refunds any already-handed-over items first
    protected virtual void LeaveQueueWithoutOrder()
    {
        LeaveQueueAndResume();
        ResetOrderState();
    }

    // Shared cleanup: remove from queue, hide bubble, resume roaming.
    // Used by both success and timeout paths in this class and in subclasses.
    protected void LeaveQueueAndResume()
    {
        Image currentOrderImage = orderImageUI;

        if (CustomerQueue.Instance != null)
            CustomerQueue.Instance.RemoveCustomer(this);

        if (currentOrderImage != null)
            currentOrderImage.gameObject.SetActive(false);

        RoamingNPC roamingNpc = GetComponent<RoamingNPC>();
        if (roamingNpc != null)
            roamingNpc.ResumeRoaming();
        else
            Debug.LogWarning("CustomerOrder completed but no RoamingNPC component was found to resume roaming");
    }

    protected virtual void ResetOrderState()
    {
        requestedItem = null;
        hasOrdered = false;
        waitTimer = 0f;
        vendorLookTarget = null;
        isFacingVendor = false;
        wasFacingVendor = false;
    }

    public void CompleteOrder()
    {
        TryCompleteOrder(requestedItem);
    }

    public Item_Data GetRequestedItem()
    {
        return requestedItem;
    }
}