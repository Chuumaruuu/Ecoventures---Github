using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Achievement_Manager : MonoBehaviour
{
    public static Achievement_Manager Instance {get ; private set;}
    [SerializeField] private List<Achievement_Data> _achievementsList;
    [SerializeField] private Achievement_Progress _progress;
    [SerializeField] private Image _achievementBoxImage;
    [SerializeField] private Animator _achievementBoxAnimator;

    // Titles must match the _achievementTitle set on each Achievement_Data asset.
    // TITLES
    private const string TITLE_QUIZ_RUSH = "QuizRush";
    private const string TITLE_ECO_VENTURED = "EcoVentured";
    private const string TITLE_CURIOUS_JED = "CuriousJed";
    private const string TITLE_INFINITE_SOLUTIONS = "InfiniteSolutions";
    private const string TITLE_FOR_REAL = "ForReal?";

    // Constants for achievement conditions
    private const int TOTAL_ITEMS = 6;
    private const int SALES_TARGET = 50;
    private const int TOTAL_HINTS = 9;
    private const int TOTAL_REGISTERED_PRODUCTS = 5; //palitan mo
    private const int TOTAL_GARBAGED = 5; //palitan mo

    // Tracks unique hints viewed this session (Curious Jed needs "view ALL 9", not 9 views total).
    private readonly HashSet<Hints_Data> _viewedHints = new HashSet<Hints_Data>();
    private readonly HashSet<Item_Data> _unlockedItems = new HashSet<Item_Data>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 
    }

    private void ShowAchievement(Achievement_Data _achievement)
    {
        _achievementBoxImage.sprite = _achievement._achievementImage;
        _achievementBoxAnimator.SetTrigger("PopUp");
    }

    public void TriggerAchievement(string _title)
    {
        foreach (Achievement_Data data in _achievementsList)
        {
            if (_title == data.GetTitle())
            {
                ShowAchievement(data);
                return;
            }
        }
        Debug.LogError("Achievement with name " + _title + " does not exist");
    }

    // --- Progress reporting API -------------------------------------------
    // Gameplay systems call these whenever something relevant happens.
    // Each one is safe to call repeatedly/redundantly - it no-ops once earned.

    /// <summary>Call whenever an item's isUnlocked flips to true, passing the full item catalog.</summary>
    public void ReportItemUnlocked(Item_Data itemData)
    {
        if (_progress == null || _progress.QUIZ_RUSH) return;
        if (itemData == null) return;

        _unlockedItems.Add(itemData);
        Debug.Log("Item unlocked: " + itemData.GetItemName() + " | Total unlocked: " + _unlockedItems.Count);
        if (_unlockedItems.Count >= TOTAL_ITEMS)
        {
            _progress.QUIZ_RUSH = true;
            Debug.Log("Achievement unlocked: " + TITLE_QUIZ_RUSH);
            TriggerAchievement(TITLE_QUIZ_RUSH);
        }
    }

    /// <summary>Call whenever a sale is registered, passing the running total.</summary>
    public void ReportSaleRegistered(int totalSales)
    {
        if (_progress == null || _progress.ECO_VENTURED) return;

        if (totalSales >= SALES_TARGET)
        {
            _progress.ECO_VENTURED = true;
            TriggerAchievement(TITLE_ECO_VENTURED);
        }
    }

    /// <summary>Call whenever a hint is viewed, passing its Hints_Data asset.</summary>
    public void ReportHintViewed(Hints_Data hint)
    {
        if (_progress == null || _progress.CURIOUS_JED) return;
        if (hint == null) return;

        _viewedHints.Add(hint);
        if (_viewedHints.Count >= TOTAL_HINTS)
        {
            _progress.CURIOUS_JED = true;
            TriggerAchievement(TITLE_CURIOUS_JED);
        }
    }

    /// <summary>Call whenever a product is registered, passing the running total.</summary>
    public void ReportProductRegistered(int totalRegistered)
    {
        if (_progress == null || _progress.INFINITE_SOLUTIONS) return;

        if (totalRegistered >= TOTAL_REGISTERED_PRODUCTS)
        {
            _progress.INFINITE_SOLUTIONS = true;
            TriggerAchievement(TITLE_INFINITE_SOLUTIONS);
        }
    }

    /// <summary>Call whenever a product is garbaged, passing the running total.</summary>
    public void ReportProductGarbaged(int totalGarbaged)
    {
        if (_progress == null || _progress.FOR_REAL) return;

        if (totalGarbaged >= TOTAL_GARBAGED)
        {
            _progress.FOR_REAL = true;
            TriggerAchievement(TITLE_FOR_REAL);
        }
    }
}