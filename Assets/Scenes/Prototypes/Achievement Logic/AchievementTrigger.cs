using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement_Manager : MonoBehaviour
{
    [SerializeField] private List<Achievement_Data> _achievementsList;
    [SerializeField] private Image _achievementBoxImage;
    [SerializeField] private Animator _achievementBoxAnimator;

    public void ShowAchievement(Achievement_Data _achievement)
    {
        _achievementBoxImage.sprite = _achievement._achievementImage;
        _achievementBoxAnimator.SetTrigger("PopUp");
    }

    public Achievement_Data GetAchievement(string _title)
    {
        foreach (Achievement_Data i in _achievementsList)
        {
            if (_title == i.GetTitle())
            {
                return i;
            }
        }

        Debug.LogError("Achievement with name " + _title + " does not exist");
        return null;
    }
}
