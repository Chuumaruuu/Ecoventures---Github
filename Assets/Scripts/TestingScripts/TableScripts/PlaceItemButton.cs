using UnityEngine;

public class PlaceItemButton : MonoBehaviour
{
    public ShopTable shopTable;
    public Item_Data itemToPlace;

    public void PlaceItem()
    {
        if (shopTable == null || itemToPlace == null)
            return;

        shopTable.PlaceItemOnTable(itemToPlace);
    }
}
