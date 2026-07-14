using UnityEngine;
using TMPro;

public class RemainingProductsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] NumberOfProductsText;
    private InventoryManager _inventoryManager;

    private void Start()
    {
        _inventoryManager = InventoryManager.Instance;

        if (_inventoryManager != null)
        {
            _inventoryManager.OnInventoryChanged += HandleInventoryChanged;
        }

        // Set the initial display without waiting for the first change.
        UpdateProductCountUI();
    }

    private void OnDestroy()
    {
        if (_inventoryManager != null)
        {
            _inventoryManager.OnInventoryChanged -= HandleInventoryChanged;
        }
    }

    private void HandleInventoryChanged(Item_Data item, int newCount)
    {
        UpdateProductCountUI();
    }

    private void UpdateProductCountUI()
    {
        if (NumberOfProductsText == null || _inventoryManager == null || _inventoryManager.gameInventoryData == null)
        {
            return;
        }

        int productCount = _inventoryManager.gameInventoryData._finalProducts.Count;

        foreach (var text in NumberOfProductsText)
        {
            if (text != null)
            {
                text.text = productCount.ToString();
            }
        }
    }
}