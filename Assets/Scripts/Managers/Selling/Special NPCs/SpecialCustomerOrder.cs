using System.Collections.Generic;
using UnityEngine;

// Orders 3 items, revealed one at a time via the same bubble/button flow
// regular customers use. The sale only registers once all 3 are fulfilled;
// if the wait timer runs out early, anything already handed over is
// refunded to inventory and nothing is registered.
public class SpecialCustomerOrder : CustomerOrder
{
    [SerializeField] private int itemsPerOrder = 3;

    private List<Item_Data> requestedItems = new List<Item_Data>();
    private List<Item_Data> fulfilledItems = new List<Item_Data>();
    private int currentIndex = 0;

    protected override void GenerateOrder()
    {
        requestedItems.Clear();
        fulfilledItems.Clear();
        currentIndex = 0;

        if (OrderGenerator.Instance == null)
        {
            Debug.LogWarning("OrderGenerator instance missing");
            return;
        }

        for (int i = 0; i < itemsPerOrder; i++)
        {
            Item_Data item = OrderGenerator.Instance.GetRandomItem();
            if (item != null)
                requestedItems.Add(item);
        }

        if (requestedItems.Count == 0)
            return;

        requestedItem = requestedItems[0];
        ShowCurrentItemSprite();
    }

    private void ShowCurrentItemSprite()
    {
        if (orderImageUI == null || requestedItem == null) return;

        orderImageUI.sprite = requestedItem._itemSprite;
        orderImageUI.gameObject.SetActive(true);

        OrderBubbleButton bubbleButton = orderImageUI.GetComponent<OrderBubbleButton>();
        if (bubbleButton != null)
            bubbleButton.SetCustomer(this);
    }

    public override bool TryCompleteOrder(Item_Data selectedItem)
    {
        if (requestedItem == null || selectedItem == null || selectedItem != requestedItem)
            return false;

        if (SalesTracker.Instance == null)
        {
            Debug.LogWarning("SalesTracker instance missing when completing special order");
            return false;
        }

        // Physically hand the item over / remove from stock now, but don't
        // award money or count the sale until all 3 are done.
        if (!SalesTracker.Instance.ReserveStockForSpecialItem(selectedItem))
        {
            Debug.LogWarning("Item not available in inventory for special order!");
            return false;
        }

        fulfilledItems.Add(selectedItem);
        currentIndex++;

        if (currentIndex < requestedItems.Count)
        {
            requestedItem = requestedItems[currentIndex];
            ShowCurrentItemSprite();
            return true;
        }

        SalesTracker.Instance.CompleteSpecialSale(fulfilledItems);
        LeaveQueueAndResume();
        ResetOrderState();
        return true;
    }

    protected override void LeaveQueueWithoutOrder()
    {
        if (fulfilledItems.Count > 0 && SalesTracker.Instance != null)
            SalesTracker.Instance.RefundReservedItems(fulfilledItems);

        LeaveQueueAndResume();
        ResetOrderState();
    }

    protected override void ResetOrderState()
    {
        base.ResetOrderState();
        requestedItems.Clear();
        fulfilledItems.Clear();
        currentIndex = 0;
    }
}