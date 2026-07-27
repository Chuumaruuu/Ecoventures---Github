using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_Data", menuName = "Scriptable Objects/Dialogue_Data")]
public class Dialogue_Data : ScriptableObject
{
    public string _dialogueTitle;
    public DialogueType _dialogueType;
    public Dialogue[] _messages;
    public Actor[] _actors;

    public string GetTitle()
    {
        return _dialogueTitle;
    }

    public DialogueType GetDialogueType()
    {
        return _dialogueType;
    }
}

[System.Serializable]
public class Dialogue
{
    public int _actorID;
    public string _message;
}

[System.Serializable]
public class Actor
{
    public string _actorName;
    public Sprite _avatar;
}

[System.Serializable]
public enum DialogueType
{
    standard, start, middle, end
}