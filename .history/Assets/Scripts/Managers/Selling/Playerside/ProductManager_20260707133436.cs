using UnityEngine;
using UnityEngine.UI;

public class ProductManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] NumberOfProductsText;
    private GameInventoryManager gameInventoryManager;

    private void Awake()
    {
        gameInventoryManager = GameInventoryManager.Instance;
    }

    private void Update()
    {
        UpdateProductCountUI();
    }

    private void UpdateProductCountUI()
    {
        if (gameInventoryManager == null)
            return;

        for (int i = 0; i < NumberOfProductsText.Length; i++)
        {
            int productCount = gameInventoryManager.GetProductCount(i);
            NumberOfProductsText[i].text = productCount.ToString();
        }
    }
}
