using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SellingProductButton : MonoBehaviour
{
    //Pangsave lang
    [SerializeField] private Item_Data product;
    [SerializeField] private Image iconImage;

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
