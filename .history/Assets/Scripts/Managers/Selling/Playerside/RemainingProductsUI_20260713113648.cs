using UnityEngine;
using TMPro;

public class RemainingProductsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] NumberOfProductsText;
    private InventoryManager _inventoryManager;

    private void Awake()
    {
        _inventoryManager = InventoryManager.Instance;
    }

    private void Update()
    {
        UpdateProductCountUI();
    }

    private void UpdateProductCountUI()
    {
        if (NumberOfProductsText == null || _inventoryManager.gameInventoryData == null)
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
