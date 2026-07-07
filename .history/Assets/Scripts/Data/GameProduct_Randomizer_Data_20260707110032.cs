using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameProduct_Randomizer_Data", menuName = "Scriptable Objects/GameProduct_Randomizer_Data")]
public class GameProduct_Randomizer_Data : ScriptableObject
{
    public List<Item_Data> _allowedItems = new List<Item_Data>();

    public void ClearAllowedProduct()
    {
        _allowedItems.Clear();
    }

    public void AddAllowedProduct(Item_Data _item)
    {
        if (!_item.IsUnlocked())
        {
            return;
        }
        _allowedItems.Add(_item);
    }

    public bool HasAllowedProduct(Item_Data _item)
    {
        return _allowedItems.Contains(_item);
    }

}