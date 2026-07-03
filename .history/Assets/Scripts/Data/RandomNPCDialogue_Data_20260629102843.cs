using UnityEngine;

[CreateAssetMenu(fileName = "New RandomNPCDialogue", menuName = "Data/RandomNPCDialogue")]
public class RandomNPCDialogue_Data : ScriptableObject
{
    public int _stageID;
    public string[] _randomDialogues;
}