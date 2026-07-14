using System;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance;
    private GameInventory_Data inventory;
    private Item_Data selectedProduct;
    public int totalSales = 0;

    // Fired exactly once, the moment the sales objective is reached.
    // Unlock_Manager listens to this so any product that "passed" its quiz
    // while the objective was still incomplete can finally unlock.
    public event Action OnObjectivesCompleted;

    private bool objectivesCompleted = false;

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
            inventory.AddMoney(Mathf.Max(0, item.sellprice));

            UpdateTaskUI();
            UpdateMoneyUI();
            CheckObjectiveCompletion();

            Debug.Log("Sold: " + item.name + " | Total Sales: " + totalSales);
        }
        else
        {
            Debug.LogWarning("Item not available in inventory!");
        }
    }

    // True once every current objective (right now: the sales goal) is satisfied.
    // Add more conditions here (&&) as more objective types get introduced.
    public bool AreObjectivesMet()
    {
        return totalSales >= salesGoal;
    }

    private void CheckObjectiveCompletion()
    {
        if (objectivesCompleted || !AreObjectivesMet())
        {
            return;
        }

        objectivesCompleted = true;
        OnObjectivesCompleted?.Invoke();
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

        int playerMoney = inventory != null ? inventory._playerMoney : 0;
        moneyText.text = playerMoney.ToString("N0");
    }
}