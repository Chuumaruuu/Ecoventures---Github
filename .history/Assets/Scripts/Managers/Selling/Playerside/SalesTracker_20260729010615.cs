using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SalesTracker : MonoBehaviour
{
    public static SalesTracker Instance;

    private InventoryManager inventoryManager;
    private Item_Data selectedProduct;
    public int totalSales = 0;

    public event Action<int> OnSaleRegistered;
    public event Action<int> OnSpecialSaleRegistered;

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

    // Called each time a special customer's individual item is handed over.
    // Removes stock immediately but does NOT award money or count as a sale.
    public bool ReserveStockForSpecialItem(Item_Data item)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning("SalesTracker has no InventoryManager assigned");
            return false;
        }

        return inventoryManager.RemoveProduct(item);
    }

    // Called if a special order times out with some items already handed over.
    public void RefundReservedItems(List<Item_Data> items)
    {
        if (inventoryManager == null || items == null) return;

        foreach (var item in items)
            inventoryManager.AddProduct(item);
    }

    // Called once all 3 items of a special order are fulfilled - this is
    // the point money and the sale actually register.
    public void CompleteSpecialSale(List<Item_Data> items)
    {
        if (items == null || items.Count == 0) return;

        int totalValue = 0;
        foreach (var item in items)
            totalValue += Mathf.Max(0, item.sellprice);

        if (inventoryManager != null)
            inventoryManager.gameInventoryData.AddMoney(totalValue);

        totalSales++;
        UpdateMoneyUI();
        OnSpecialSaleRegistered?.Invoke(totalSales);

        Debug.Log("Special order completed | Total Special Sales: " + totalSales);
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