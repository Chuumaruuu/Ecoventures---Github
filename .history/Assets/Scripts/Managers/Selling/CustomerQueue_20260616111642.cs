using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerQueue : MonoBehaviour
{
    public static CustomerQueue Instance;

    [Header("Queue Points (front of table)")]
    [SerializeField] Transform[] queuePoints; // 3 points in front of the table
    [SerializeField] Image[] queuePointImages; // Corresponding UI images for each queue point
    [SerializeField] Transform vendorLookTarget;

    private Queue<CustomerOrder> activeCustomers = new Queue<CustomerOrder>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanJoinQueue()
    {
        return activeCustomers.Count < queuePoints.Length;
    }

    public void AddCustomer(CustomerOrder customer)
    {
        if (!CanJoinQueue()) return;

        activeCustomers.Enqueue(customer);
        int index = activeCustomers.Count - 1;

        Image queueImage = queuePointImages != null && index < queuePointImages.Length
            ? queuePointImages[index]
            : null;

        // Move NPC to queue point and make it face the vendor when it arrives.
        // SetOrderImage is deferred: the bubble only appears once the customer
        // has arrived and is actually facing the vendor.
        customer.MoveToQueue(queuePoints[index].position, vendorLookTarget);

        customer.OnFacingVendor += () => customer.SetOrderImage(queueImage);
    }

    public void RemoveCustomer(CustomerOrder customer)
    {
        if (!activeCustomers.Contains(customer)) return;

        // Find which slot this customer occupied and hide its UI image
        var list = new System.Collections.Generic.List<CustomerOrder>(activeCustomers);
        int slotIndex = list.IndexOf(customer);

        // Hide the removed customer's order bubble
        customer.SetOrderImage(null);

        // Hide the UI image for the now-vacant slot
        if (queuePointImages != null && slotIndex >= 0 && slotIndex < queuePointImages.Length && queuePointImages[slotIndex] != null)
            queuePointImages[slotIndex].gameObject.SetActive(false);

        list.Remove(customer);
        activeCustomers = new Queue<CustomerOrder>(list);
    }
}