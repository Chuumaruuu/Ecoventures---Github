using UnityEngine;
using System;

public class Unlock_Manager : MonoBehaviour
{
    public static Unlock_Manager Instance { get; private set; }

    public event Action<Item_Data, bool> OnUnlockStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
