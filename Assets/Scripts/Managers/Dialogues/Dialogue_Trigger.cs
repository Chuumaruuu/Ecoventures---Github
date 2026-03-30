using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour
{
    public Message[] _messages;
    public Actor[] _actors;
}

[System.Serializable]
public class Mesage
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
