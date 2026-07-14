using System;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] public GameInventory_Data gameInventoryData;
    [SerializeField] public GameProduct_Randomizer_Data randomizerData;

    // Fired whenever a product is added to or removed from the inventory.
    // Passes the affected item and its new remaining count in _finalProducts.
    // Subscribers (RemainingProductsUI, ProductPrefabChanger, etc.) should
    // react to this instead of polling gameInventoryData every frame.
    public event Action<Item_Data, int> OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("InventoryManager instance created");
        }
        else
            Destroy(gameObject);
    }

    // Removes one instance of item from the inventory. Returns false if the
    // item wasn't available (e.g. already sold before this call went through).
    public bool RemoveProduct(Item_Data item)
    {
        if (item == null || gameInventoryData == null || gameInventoryData._finalProducts == null)
        {
            return false;
        }

        if (!gameInventoryData._finalProducts.Contains(item))
        {
            return false;
        }

        gameInventoryData._finalProducts.Remove(item);
        NotifyInventoryChanged(item);
        return true;
    }

    // Adds one instance of item to the inventory (e.g. from the workshop/
    // production phase).
    public void AddProduct(Item_Data item)
    {
        if (item == null || gameInventoryData == null)
        {
            return;
        }

        if (gameInventoryData._finalProducts == null)
        {
            Debug.LogWarning("InventoryManager has no product list to add to.");
            return;
        }

        gameInventoryData._finalProducts.Add(item);
        NotifyInventoryChanged(item);
    }

    private void NotifyInventoryChanged(Item_Data item)
    {
        int newCount = gameInventoryData._finalProducts.Count(i => i == item);
        OnInventoryChanged?.Invoke(item, newCount);
    }
}