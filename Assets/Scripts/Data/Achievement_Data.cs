using UnityEngine;

[CreateAssetMenu(fileName = "Achievement_Data", menuName = "Scriptable Objects/Achievement_Data")]
public class Achievement_Data : ScriptableObject
{
    public string _achievementTitle;
    public Sprite _achievementImage;


    public string GetTitle()
    {
        return _achievementTitle;
    }

}