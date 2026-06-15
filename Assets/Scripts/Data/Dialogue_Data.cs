using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_Data", menuName = "Scriptable Objects/Dialogue_Data")]
public class Dialogue_Data : ScriptableObject
{
    public string _dialogueTitle;
    public Dialogue[] _messages;
    public Actor[] _actors;
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