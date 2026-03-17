using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/New Inventory Data")]
public class InventoryData : ScriptableObject
{
    public List<ItemData> items = new List<ItemData>();

    // Optional: call this if you want to reset inventory at game start
    public void ClearInventory()
    {
        items.Clear();
    }
}