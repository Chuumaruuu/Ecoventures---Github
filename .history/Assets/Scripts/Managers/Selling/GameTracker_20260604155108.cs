using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance;
    public GameInventory_Data inventory;
    private Item_Data selectedProduct;
    public int totalSales = 0;
    public int totalCoins = 0;

    [Header("Task")]
    [SerializeField] private int salesGoal = 10;
    [SerializeField] private TextMeshProUGUI tasksText;

    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateTaskUI();
        UpdateMoneyUI();
    }

    public void RegisterSale(Item_Data item)
    {
        if (inventory == null)
        {
            Debug.LogWarning("GameTracker has no inventory assigned");
            return;
        }

        if (inventory._finalProducts == null)
        {
            Debug.LogWarning("Inventory items list is null");
            return;
        }

        if (inventory._finalProducts.Contains(item))
        {
            inventory._finalProducts.Remove(item);
            totalSales++;
            totalCoins += Mathf.Max(0, item.sellprice);

            UpdateTaskUI();
            UpdateMoneyUI();

            Debug.Log("Sold: " + item.name + " | Total Sales: " + totalSales);
        }
        else
        {
            Debug.LogWarning("Item not available in inventory!");
        }
    }

    public void SetSelectedProduct(Item_Data item)
    {
        selectedProduct = item;
    }

    public Item_Data GetSelectedProduct()
    {
        return selectedProduct;
    }

    public bool TryCompleteSale(CustomerOrder customerOrder)
    {
        if (customerOrder == null)
            return false;

        if (selectedProduct == null)
        {
            Debug.LogWarning("No product selected before tapping the order bubble");
            return false;
        }

        return customerOrder.TryCompleteOrder(selectedProduct);
    }

    private void UpdateTaskUI()
    {
        if (tasksText == null)
            return;

        int current = Mathf.Clamp(totalSales, 0, salesGoal);
        tasksText.text = "Sell to customers: " + current + " / " + salesGoal;
    }

    private void UpdateMoneyUI()
    {
        if (moneyText == null)
            return;

        moneyText.text = totalCoins.ToString("N0");
    }
}
