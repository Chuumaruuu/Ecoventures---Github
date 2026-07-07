using UnityEngine;
using TMPro;

public class ProductManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] NumberOfProductsText;
    [SerializeField] private GameInventory_Data gameInventoryData;

    private void Update()
    {
        UpdateProductCountUI();
    }

    private void UpdateProductCountUI()
    {
        if (NumberOfProductsText == null || gameInventoryData == null)
        {
            return;
        }

        int productCount = gameInventoryData._finalProducts.Count;

        foreach (var text in NumberOfProductsText)
        {
            if (text != null)
            {
                text.text = productCount.ToString();
            }
        }
    }
}
