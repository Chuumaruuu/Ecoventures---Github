using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInventory_Data", menuName = "Scriptable Objects/GameInventory_Data")]
public class GameInventory_Data : ScriptableObject
{
    [Header("Final Products Inventory")]
    public List<Item_Data> _finalProducts = new List<Item_Data>();

    [Header("Extra Materials Inventory")]
    public List<Item_Data> _recycledProducts = new List<Item_Data>();

    [Header("Currency")]
    public int _playerMoney = 0;

    public void ClearInventory()
    {
        _finalProducts.Clear();
        _recycledProducts.Clear();
    }

    public void AddProducts(Item_Data _item)
    {
                _finalProducts.Add(_item);
    }

    public void AddRecycledMaterials(Item_Data _item)
    {
        _recycledProducts.Add(_item);
    }

    public void RemoveProduct(Item_Data _item)
    {
        _finalProducts.Remove(_item);
    }

    public void RemoveRecycledItem(Item_Data _item)
    {
        _recycledProducts.Remove(_item);
    }

    public void AddMoney(int _value)
    {
        _playerMoney += _value;
    }

    public void SubtractMoney(int _value)
    {
        _playerMoney -= _value;
    }

    public bool HasRecycledItem(Item_Data _item)
    {
        return _recycledProducts.Contains(_item);
    }

}