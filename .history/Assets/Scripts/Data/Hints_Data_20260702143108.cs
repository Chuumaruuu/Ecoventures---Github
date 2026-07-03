using UnityEngine;

[CreateAssetMenu(fileName = "Hints_Data", menuName = "Scriptable Objects/Hints_Data")]
public class Hints_Data : ScriptableObject
{
    public string _stageName;
    public Sprite[] _hintSprites;
}
