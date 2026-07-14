using UnityEngine;
using TMPro;
using System.Linq;

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
            Debug.LogWarning("RemainingProductsUI: NumberOfProductsText or InventoryManager or gameInventoryData is null.");
            return;
        }

        int productCount = _inventoryManager.gameInventoryData._finalProducts.Count;

        for (int i = 0; i < NumberOfProductsText.Length; i++)
        {
            if (i >= productCount)
            {
                NumberOfProductsText[i].text = "0"; // If there are fewer products than text fields, set remaining to 0.
                continue;
            }

            Item_Data product = _inventoryManager._randomizerData._allowedItems[i];
            NumberOfProductsText[i].text = productCount.ToString();
        }
    }
}