using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Questions_Data", menuName = "Scriptable Objects/Questions_Data")]
public class Questions_Data : ScriptableObject
{
    public String _questionBody;
    public String _rightAnswer;
    public String _wrongAnswer1;
    public String _wrongAnswer2;
    public String _wrongAnswer3;
    public Sprite _NPCSprite;
}
