using UnityEngine;

[CreateAssetMenu(fileName = "ProgressRecipe_Data", menuName = "Scriptable Objects/ProgressRecipe_Data")]
public class ProgressRecipe_Data : ScriptableObject
{
    public Item_Data _unfinishedItem;
    public Item_Data _finishedItem;
    public Item_Data _overcookedItem;
    public float _timerMax = 5;
}
