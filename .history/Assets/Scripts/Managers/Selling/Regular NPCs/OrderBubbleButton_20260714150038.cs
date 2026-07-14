using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OrderBubbleButton : MonoBehaviour
{
    private CustomerOrder customerOrder;
    private Button button;
    private SalesTracker salesTracker;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnBubbleClicked);
    }

    private void Start()
    {
        salesTracker = SalesTracker.Instance;
        if (salesTracker == null)
        {
            Debug.LogWarning("SalesTracker instance missing in OrderBubbleButton");
        }
    }

    public void SetCustomer(CustomerOrder customer)
    {
        customerOrder = customer;
    }

    private void OnBubbleClicked()
    {
        Debug.Log("Order bubble clicked for customer: " + (customerOrder != null ? customerOrder.name : "null"));
        if (SalesTracker.Instance == null)
        {
            Debug.LogWarning("SalesTracker instance missing when tapping order bubble");
            return;
        }

        if (customerOrder == null)
        {
            Debug.LogWarning("Order bubble has no customer assigned");
            return;
        }

        SalesTracker.Instance.TryCompleteSale(customerOrder);
    }
}