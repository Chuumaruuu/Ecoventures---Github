using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInventory_Data", menuName = "Scriptable Objects/GameInventory_Data")]
public class GameInventory_Data : ScriptableObject
{
    public List<Item_Data> items = new List<Item_Data>();
    public int _keyChains = 0;
    public int _toteBags = 0;
    public int _buttonPins = 0;
    public int _figurines = 0;

    public void ClearInventory()
    {
        items.Clear();
    }
}