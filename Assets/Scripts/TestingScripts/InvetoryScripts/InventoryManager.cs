using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // 🔥 GLOBAL STATE
    public static bool IsInventoryOpen = false;

    [Header("References")]
    public GameInventory_Data inventoryData;
    public InventoryButton inventoryButtonPrefab;
    public Transform gridParent;
    public ShopTable shopTable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
            if (inventoryData != null)
                inventoryData.ClearInventory();
#endif

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        shopTable = Object.FindFirstObjectByType<ShopTable>();
        PopulateInventoryUI();
    }

    public void SetGridParent(Transform newGrid)
    {
        gridParent = newGrid;
    }

    // 🔥 CALL THIS WHEN OPENING INVENTORY UI
    public void OpenInventory()
    {
        IsInventoryOpen = true;
    }

    // 🔥 CALL THIS WHEN CLOSING INVENTORY UI
    public void CloseInventory()
    {
        IsInventoryOpen = false;
    }

    public void AddItemToInventory(Item_Data newItem)
    {
        if (inventoryData == null)
        {
            Debug.LogError("InventoryData is not assigned!");
            return;
        }

        inventoryData._finalProducts.Add(newItem);
        Debug.Log("✅ Item added: " + newItem._objectName);
        PopulateInventoryUI();
    }

    public void RemoveItemFromInventory(Item_Data itemToRemove)
    {
        if (inventoryData._finalProducts.Contains(itemToRemove))
        {
            inventoryData._finalProducts.Remove(itemToRemove);
            Debug.Log("🗑️ Item removed: " + itemToRemove._objectName);
            PopulateInventoryUI();
        }
    }

    public void PopulateInventoryUI()
    {
        if (gridParent == null)
        {
            Debug.LogWarning("⚠️ gridParent is not assigned yet.");
            return;
        }

        if (inventoryButtonPrefab == null)
        {
            Debug.LogWarning("⚠️ inventoryButtonPrefab is not assigned!");
            return;
        }

        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (Item_Data item in inventoryData._finalProducts)
        {
            InventoryButton btn = Instantiate(inventoryButtonPrefab, gridParent);
            btn.item = item;

            if (shopTable != null)
                btn.shopTable = shopTable;

            btn.UpdateUI();
        }
    }

    public void ClearAll()
    {
        if (inventoryData == null)
            return;

        inventoryData.ClearInventory();
        Debug.Log("🗑️ Inventory cleared.");
        PopulateInventoryUI();
    }
}