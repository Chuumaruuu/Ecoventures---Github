using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OrderBubbleButton : MonoBehaviour
{
    private CustomerOrder customerOrder;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnBubbleClicked);
    }

    public void SetCustomer(CustomerOrder customer)
    {
        customerOrder = customer;
    }

    private void OnBubbleClicked()
    {
        Debug.Log("Order bubble clicked for customer: " + (customerOrder != null ? customerOrder.name : "null"));
        if (GameTracker.Instance == null)
        {
            Debug.LogWarning("GameTracker instance missing when tapping order bubble");
            return;
        }

        if (customerOrder == null)
        {
            Debug.LogWarning("Order bubble has no customer assigned");
            return;
        }

        GameTracker.Instance.TryCompleteSale(customerOrder);
    }
}
