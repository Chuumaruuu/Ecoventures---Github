using System;
using UnityEngine;
using TMPro;

public class ObjectivesTracker : MonoBehaviour
{
    public static ObjectivesTracker Instance;

    public event Action OnObjectivesCompleted;

    private bool objectivesCompleted = false;
    private int currentSales = 0;
    private int currentSpecialSales = 0;

    [Header("Task")]
    [SerializeField] private int salesGoal = 10;
    [SerializeField] private int specialSalesGoal = 1;
    [SerializeField] private TextMeshProUGUI tasksText;
    [SerializeField] private TextMeshProUGUI specialTaskText; // optional, can leave unassigned

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateTaskUI();

        if (SalesTracker.Instance != null)
        {
            SalesTracker.Instance.OnSaleRegistered += HandleSaleRegistered;
            SalesTracker.Instance.OnSpecialSaleRegistered += HandleSpecialSaleRegistered;
        }
    }

    private void OnDestroy()
    {
        if (SalesTracker.Instance != null)
        {
            SalesTracker.Instance.OnSaleRegistered -= HandleSaleRegistered;
            SalesTracker.Instance.OnSpecialSaleRegistered -= HandleSpecialSaleRegistered;
        }
    }

    private void HandleSaleRegistered(int totalSales)
    {
        currentSales = totalSales;
        UpdateTaskUI();
        CheckObjectiveCompletion();
    }

    private void HandleSpecialSaleRegistered(int totalSpecialSales)
    {
        currentSpecialSales = totalSpecialSales;
        UpdateTaskUI();
        CheckObjectiveCompletion();
    }

    public bool AreObjectivesMet()
    {
        return currentSales >= salesGoal && currentSpecialSales >= specialSalesGoal;
    }

    // Used by RoamingNPC to know when to start favoring the vendor point
    // for special customers.
    public bool IsRegularGoalMet()
    {
        return currentSales >= salesGoal;
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

    private void UpdateTaskUI()
    {
        if (tasksText != null)
        {
            int current = Mathf.Clamp(currentSales, 0, salesGoal);
            tasksText.text = "Sell to customers: " + current + " / " + salesGoal;
        }

        if (specialTaskText != null)
        {
            int currentSpecial = Mathf.Clamp(currentSpecialSales, 0, specialSalesGoal);
            specialTaskText.text = "Special orders: " + currentSpecial + " / " + specialSalesGoal;
        }
    }
}