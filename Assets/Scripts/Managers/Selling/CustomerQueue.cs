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

        // Assign the correct UI image from the queue point
        customer.SetOrderImage(queueImage);

        // Move NPC to queue point and make it face the vendor when it arrives
        customer.MoveToQueue(queuePoints[index].position, vendorLookTarget);
    }

    public void RemoveCustomer(CustomerOrder customer)
    {
        if (!activeCustomers.Contains(customer)) return;

        // Rebuild the queue without the removed customer so we can reindex positions/UI
        var list = new System.Collections.Generic.List<CustomerOrder>(activeCustomers);
        // hide the removed customer's UI (if still assigned)
        if (customer != null)
        {
            customer.SetOrderImage(null);
        }

        list.Remove(customer);
        activeCustomers = new Queue<CustomerOrder>(list);

        // Reassign remaining customers to queue points and UI images
        for (int i = 0; i < queuePoints.Length; i++)
        {
            if (i < list.Count)
            {
                var c = list[i];
                Image queueImage = queuePointImages != null && i < queuePointImages.Length
                    ? queuePointImages[i]
                    : null;

                // move them to the correct point and assign the proper UI image
                c.SetOrderImage(queueImage);
                c.MoveToQueue(queuePoints[i].position, vendorLookTarget);
                if (queueImage != null)
                    queueImage.gameObject.SetActive(true);
            }
            else
            {
                // hide any leftover UI images for empty queue slots
                if (queuePointImages != null && i < queuePointImages.Length && queuePointImages[i] != null)
                    queuePointImages[i].gameObject.SetActive(false);
            }
        }
    }
}
