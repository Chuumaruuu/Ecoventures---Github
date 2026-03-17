using UnityEngine;

[CreateAssetMenu(fileName = "ProccesRecipe_Data", menuName = "Scriptable Objects/Combination_Data")]
public class ItemCombinationRecipe_Data : ScriptableObject
{
    public Item_Data _item1,_item2;
    public Item_Data _outputItem;
    public float _progressMax = 5;
}

