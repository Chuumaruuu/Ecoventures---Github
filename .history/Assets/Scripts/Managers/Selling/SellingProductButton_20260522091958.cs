using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SellingProductButton : MonoBehaviour
{
    [SerializeField] private Item_Data product;
    [SerializeField] private Image iconImage;
    [SerializeField] private Text productNumberText;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(SelectProduct);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (iconImage != null && product != null)
            iconImage.sprite = product._itemSprite;
        
        UpdateProductNumber();
    }

    private void UpdateProductNumber()
    {
        if (productNumberText == null || product == null)
            return;

        int inventoryCount = GameTracker.Instance != null ? GameTracker.Instance.GetProductInventoryCount(product) : 0;
        productNumberText.text = inventoryCount.ToString();
    }

    private void SelectProduct()
    {
        Debug.Log("Product button clicked: " + (product != null ? product.name : "null"));
        if (GameTracker.Instance == null)
        {
            Debug.LogWarning("GameTracker instance missing when selecting a product");
            return;
        }

        GameTracker.Instance.SetSelectedProduct(product);
    }
}
