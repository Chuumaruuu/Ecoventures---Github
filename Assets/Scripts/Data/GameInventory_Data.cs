using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInventory_Data", menuName = "Scriptable Objects/GameInventory_Data")]
public class GameInventory_Data : ScriptableObject
{
    [Header("Inventory")]
    public List<Item_Data> items = new List<Item_Data>();
    public int _keyChains = 0;
    public int _toteBags = 0;
    public int _buttonPins = 0;
    public int _figurines = 0;

    [Header("Currency")]
    public int _playerMoney = 0;


    public void ClearInventory()
    {
        items.Clear();
    }
}