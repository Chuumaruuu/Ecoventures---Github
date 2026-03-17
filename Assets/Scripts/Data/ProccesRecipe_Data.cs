using UnityEngine;

[CreateAssetMenu(fileName = "ProccesRecipe_Data", menuName = "Scriptable Objects/ProccesRecipe_Data")]
public class ProcessRecipe_Data : ScriptableObject
{
    public Item_Data _inputItem;
    public Item_Data _outputItem;
    public float _progressMax = 5;
}

