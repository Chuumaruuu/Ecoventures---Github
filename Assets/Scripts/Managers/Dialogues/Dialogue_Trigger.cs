using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour
{
    public Dialogue[] _messages;
    public Actor[] _actors;

    public void StartDialogue()
    {
        Dialogue_Manager.Instance.OpenDialogue(_messages, _actors);
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

