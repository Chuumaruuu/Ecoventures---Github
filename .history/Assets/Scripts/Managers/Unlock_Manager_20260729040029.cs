using UnityEngine;
using System;
using System.Collections.Generic;

public class Unlock_Manager : MonoBehaviour
{
    public static Unlock_Manager Instance { get; private set; }

    public event Action<Item_Data, bool> OnUnlockStateChanged;

    // Items that answered their quiz correctly but are still waiting on
    // ObjectivesTracker's objectives (e.g. the sales goal) before they can unlock.
    private readonly HashSet<Item_Data> pendingUnlocks = new HashSet<Item_Data>();
    private bool subscribedToObjectives;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (subscribedToObjectives && ObjectivesTracker.Instance != null)
        {
            ObjectivesTracker.Instance.OnObjectivesCompleted -= HandleObjectivesCompleted;
        }
    }

    // Call this instead of Unlock() whenever a product "passes" its unlock
    // condition (e.g. a correct booth answer). It only actually unlocks the
    // item once ObjectivesTracker's objectives are also satisfied; otherwise it
    // queues the item and unlocks it automatically the moment they are.
    public void RequestUnlock(Item_Data itemData)
    {
        if (itemData == null || IsUnlocked(itemData))
        {
            return;
        }

        bool objectivesMet = ObjectivesTracker.Instance != null && ObjectivesTracker.Instance.AreObjectivesMet();

        if (objectivesMet)
        {
            pendingUnlocks.Remove(itemData);
            Unlock(itemData);
            return;
        }

        pendingUnlocks.Add(itemData);
        SubscribeToObjectivesIfNeeded();
    }

    // Lets UI know a product is "correct, but waiting" so it can show a
    // different state than fully locked or fully unlocked.
    public bool IsPendingUnlock(Item_Data itemData)
    {
        return itemData != null && pendingUnlocks.Contains(itemData);
    }

    private void SubscribeToObjectivesIfNeeded()
    {
        if (subscribedToObjectives || ObjectivesTracker.Instance == null)
        {
            return;
        }

        ObjectivesTracker.Instance.OnObjectivesCompleted += HandleObjectivesCompleted;
        subscribedToObjectives = true;
    }

    private void HandleObjectivesCompleted()
    {
        if (pendingUnlocks.Count == 0)
        {
            return;
        }

        // Copy first since Unlock() -> event handlers could mutate other state.
        List<Item_Data> itemsToUnlock = new List<Item_Data>(pendingUnlocks);
        pendingUnlocks.Clear();

        foreach (Item_Data item in itemsToUnlock)
        {
            Unlock(item);
        }
    }

    public void SetUnlocked(Item_Data itemData, bool isUnlocked)
    {
        if (itemData == null)
        {
            return;
        }

        if (itemData.isUnlocked == isUnlocked)
        {
            return;
        }

        itemData.isUnlocked = isUnlocked;
        OnUnlockStateChanged?.Invoke(itemData, isUnlocked);
    }

    public void Unlock(Item_Data itemData)
    {
        SetUnlocked(itemData, true);
        Achievement_Manager.Instance?.ReportItemUnlocked(itemData);
    }

    public void Lock(Item_Data itemData)
    {
        SetUnlocked(itemData, false);
    }

    public bool IsUnlocked(Item_Data itemData)
    {
        return itemData != null && itemData.isUnlocked;
    }
}