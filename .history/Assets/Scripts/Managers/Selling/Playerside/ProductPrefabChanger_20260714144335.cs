using UnityEngine;
using System.Linq;

// Sits on the same spawned groupedItems prefab as ItemGroup_Base. Watches
// InventoryManager for changes to this prefab's item and toggles entries in
// ItemGroup_Base.itemState to visually show how much stock is left.
//
// Stages scale automatically with however many entries itemState has: with N
// stages, each one represents roughly 1/N of the starting stock. E.g. with 4
// entries that's the 100/75/50/25(->0) breakdown; add or remove entries on
// the prefab and this adjusts without any code changes.
[RequireComponent(typeof(ItemGroup_Base))]
public class ProductPrefabChanger : MonoBehaviour
{
    private ItemGroup_Base itemGroup;
    private Item_Data itemData;
    private int initialCount;

    private void Awake()
    {
        itemGroup = GetComponent<ItemGroup_Base>();
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += HandleInventoryChanged;
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= HandleInventoryChanged;
        }
    }

    private void Start()
    {
        itemData = itemGroup.GetItemData();

        if (itemData == null || InventoryManager.Instance == null || InventoryManager.Instance.gameInventoryData == null)
        {
            Debug.LogWarning("ProductPrefabChanger is missing item data or InventoryManager.");
            return;
        }

        // Snapshot how much stock this item had at the start of the selling
        // phase (when this prefab was spawned) - everything else is a
        // percentage of this.
        initialCount = InventoryManager.Instance.gameInventoryData._finalProducts.Count(i => i == itemData);

        UpdateVisualState(initialCount);
    }

    private void HandleInventoryChanged(Item_Data changedItem, int newCount)
    {
        if (changedItem != itemData)
        {
            return;
        }

        UpdateVisualState(newCount);
    }

    private void UpdateVisualState(int currentCount)
    {
        if (itemGroup == null || itemGroup.itemState == null || itemGroup.itemState.Length == 0)
        {
            return;
        }

        if (initialCount <= 0)
        {
            return;
        }

        int totalStages = itemGroup.itemState.Length;
        float remainingPercent = (float)currentCount / initialCount;
        int activeStages = Mathf.Clamp(Mathf.CeilToInt(remainingPercent * totalStages), 0, totalStages);

        for (int i = 0; i < totalStages; i++)
        {
            if (itemGroup.itemState[i] != null)
            {
                itemGroup.itemState[i].SetActive(i < activeStages);
            }
        }
    }
}