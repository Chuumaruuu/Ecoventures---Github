using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_Data", menuName = "Scriptable Objects/Dialogue_Data")]
public class Dialogue_Data : ScriptableObject
{
    public Dialogue[] _dialogues;
}


[System.Serializable]
public class Dialogue
{
    public string _dialogueTitle;
    public DialogueLine[] _dialogueLines;
}

[System.Serializable]
public class DialogueLine
{
    public string _sender;

    [TextArea]
    public string _message;
}