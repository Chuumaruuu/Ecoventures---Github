using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInventory_Data", menuName = "Scriptable Objects/GameInventory_Data")]
public class GameInventory_Data : ScriptableObject
{
    public List<Item_Data> items = new List<Item_Data>();

    public void ClearInventory()
    {
        items.Clear();
    }
}