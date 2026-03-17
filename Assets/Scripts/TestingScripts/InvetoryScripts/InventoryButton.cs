using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public Item_Data item;
    public ShopTable shopTable;

    [Header("UI References")]
    public Image iconImage;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnClick);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (iconImage != null && item != null)
            iconImage.sprite = item._itemSprite;
    }

    private void OnClick()
    {
        if (shopTable != null && item != null)
        {
            bool success = shopTable.PlaceItemOnTable(item);
            if (success)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Table is full!");
            }
        }
    }
}