using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_Data", menuName = "Scriptable Objects/Dialogue_Data")]
public class Dialogue_Data : ScriptableObject
{
    public Dialogue[] _dialogues;
    public Sender[] _senders;
}


[System.Serializable]
public class Dialogue
{
    public string _dialogueTitle;
    public Message[] _dialogueLines;
}

[System.Serializable]
public class Message
{
    public int _senderID;
    [TextArea]
    public string _message;
}

[System.Serializable]
public class Sender
{
    public string _name;
    public Sprite _senderSprite;
}