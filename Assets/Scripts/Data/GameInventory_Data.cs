using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInventory_Data", menuName = "Scriptable Objects/GameInventory_Data")]
public class GameInventory_Data : ScriptableObject
{
    [Header("Final Products Inventory")]
    public List<Item_Data> items = new List<Item_Data>();

    [Header("Extra Materials Inventory")]
    public int _scrapMetal = 0;
    public int _scrapWood = 0;
    public int _scrapGlass = 0;
    public int _scrapPaint = 0;
    public int _scrapFabric = 0;

    [Header("Currency")]
    public int _playerMoney = 0;


    public void ClearInventory()
    {
        items.Clear();
    }
}