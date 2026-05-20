using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder : MonoBehaviour
{
    private Item_Data requestedItem;
    private Image orderImageUI;
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform vendorLookTarget;
    private bool hasOrdered = false;
    private bool isFacingVendor = false;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        if (vendorLookTarget == null || agent == null)
            return;

        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance + 0.05f)
        {
            isFacingVendor = false;
            agent.updateRotation = true;
            return;
        }

        isFacingVendor = true;
        FaceVendorTarget();
    }

    public void MoveToQueue(Vector3 targetPosition, Transform lookTarget = null)
    {
        if (agent == null)
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        vendorLookTarget = lookTarget;
        isFacingVendor = false;

        if (agent != null)
        {
            agent.updateRotation = true;
            // Move NPC to queue point (NavMeshAgent logic)
            agent.SetDestination(targetPosition);
        }

        if (!hasOrdered)
        {
            GenerateOrder();
            hasOrdered = true;
        }
    }

    public void SetOrderImage(Image image)
    {
        if (orderImageUI != null)
        {
            OrderBubbleButton previousButton = orderImageUI.GetComponent<OrderBubbleButton>();
            if (previousButton != null)
                previousButton.SetCustomer(null);
        }

        orderImageUI = image;

        if (orderImageUI != null)
        {
            // always bind the bubble button to this customer when possible
            OrderBubbleButton bubbleButton = orderImageUI.GetComponent<OrderBubbleButton>();
            if (bubbleButton != null)
                bubbleButton.SetCustomer(this);

            // if the order already exists, update the sprite immediately
            if (requestedItem != null)
            {
                orderImageUI.sprite = requestedItem._itemSprite;
                orderImageUI.gameObject.SetActive(true);
            }
        }
    }

    private void GenerateOrder()
    {
        if (OrderGenerator.Instance == null)
        {
            Debug.LogWarning("OrderGenerator instance missing");
            return;
        }

        requestedItem = OrderGenerator.Instance.GetRandomItem();
        if (requestedItem != null)
        {
            if (orderImageUI != null)
            {
                // set sprite to match the requested item
                orderImageUI.sprite = requestedItem._itemSprite;
                orderImageUI.gameObject.SetActive(true);

                // ensure the bubble is bound to this customer
                OrderBubbleButton bubbleButton = orderImageUI.GetComponent<OrderBubbleButton>();
                if (bubbleButton != null)
                    bubbleButton.SetCustomer(this);
            }
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


    public bool TryCompleteOrder(Item_Data selectedItem)
    {
        if (requestedItem == null || selectedItem == null)
            return false;

        if (selectedItem != requestedItem)
            return false;

        // Player clicked correct product
        if (GameTracker.Instance != null)
        {
            GameTracker.Instance.RegisterSale(requestedItem);
        }
        else
        {
            Debug.LogWarning("GameTracker instance missing when completing order");
        }

        if (orderImageUI != null)
        {
            orderImageUI.gameObject.SetActive(false);
            orderImageUI = null;
        }

        if (CustomerQueue.Instance != null)
            CustomerQueue.Instance.RemoveCustomer(this);

        RoamingNPC roamingNpc = GetComponent<RoamingNPC>();
        if (roamingNpc != null)
        {
            roamingNpc.ResumeRoaming();
        }
        else
        {
            Debug.LogWarning("CustomerOrder completed but no RoamingNPC component was found to resume roaming");
        }

        requestedItem = null;
        hasOrdered = false;
        vendorLookTarget = null;
        isFacingVendor = false;

        return true;
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
