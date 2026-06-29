using UnityEngine;

[CreateAssetMenu(fileName = "RandomNPCDialogue_Data", menuName = "Scriptable Objects/RandomNPCDialogue")]
public class RandomNPCDialogue_Data : ScriptableObject
{
    public int _stageID;
    public string[] _randomDialogues;
}