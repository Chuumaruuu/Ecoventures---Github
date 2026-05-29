using UnityEngine;

[CreateAssetMenu(fileName = "DisassemblyRecipe_Data", menuName = "Scriptable Objects/Disassembly_Data")]
public class ItemDisassemblyRecipe_Data : ScriptableObject
{
    public Item_Data _inputItem;
    public Item_Data[] _outputItems;
}

