using System;
using UnityEngine;
using TMPro;

// Owns "what counts as an objective" for the current stage. Right now that's
// just the sales goal, tracked by listening to SalesTracker.OnSaleRegistered
// rather than reading SalesTracker.totalSales directly - keeps this the
// single source of truth as more objective types get added later.
public class ObjectivesTracker : MonoBehaviour
{
    public static ObjectivesTracker Instance;

    // Fired exactly once, the moment all objectives are met.
    // Unlock_Manager listens to this so any product that "passed" its quiz
    // while objectives were still incomplete can finally unlock.
    public event Action OnObjectivesCompleted;

    private bool objectivesCompleted = false;
    private int currentSales = 0;

    [Header("Task")]
    [SerializeField] private int salesGoal = 10;
    [SerializeField] private TextMeshProUGUI tasksText;

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
        }
    }

    private void OnDestroy()
    {
        if (SalesTracker.Instance != null)
        {
            SalesTracker.Instance.OnSaleRegistered -= HandleSaleRegistered;
        }
    }

    private void HandleSaleRegistered(int totalSales)
    {
        currentSales = totalSales;
        UpdateTaskUI();
        CheckObjectiveCompletion();
    }

    // True once every current objective (right now: the sales goal) is satisfied.
    // Add more conditions here (&&) as more objective types get introduced.
    public bool AreObjectivesMet()
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
        if (tasksText == null)
            return;

        int current = Mathf.Clamp(currentSales, 0, salesGoal);
        tasksText.text = "Sell to customers: " + current + " / " + salesGoal;
    }
}