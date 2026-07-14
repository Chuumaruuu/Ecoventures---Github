using System;
using UnityEngine;
using TMPro;

// Handles the actual selling flow: product selection, matching a sale to a
// customer order, and applying the economy effects (removing stock, adding
// money). Does not know or care about objectives/unlocks - it just reports
// each successful sale via OnSaleRegistered for others (ObjectivesTracker) to
// react to.
public class SalesTracker : MonoBehaviour
{
    public static SalesTracker Instance;

    private InventoryManager inventoryManager;
    private Item_Data selectedProduct;
    public int totalSales = 0;

    // Fired every time a sale is successfully registered, with the running
    // total sales count.
    public event Action<int> OnSaleRegistered;

    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateMoneyUI();
        inventoryManager = InventoryManager.Instance;
    }

    public void RegisterSale(Item_Data item)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning("SalesTracker has no InventoryManager assigned");
            return;
        }

        if (!inventoryManager.RemoveProduct(item))
        {
            Debug.LogWarning("Item not available in inventory!");
            return;
        }

        totalSales++;
        inventoryManager.gameInventoryData.AddMoney(Mathf.Max(0, item.sellprice));

        UpdateMoneyUI();
        OnSaleRegistered?.Invoke(totalSales);

        Debug.Log("Sold: " + item.name + " | Total Sales: " + totalSales);
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

    private void UpdateMoneyUI()
    {
        if (moneyText == null)
            return;

        int playerMoney = inventoryManager != null ? inventoryManager.gameInventoryData._playerMoney : 0;
        moneyText.text = playerMoney.ToString("N0");
    }
}