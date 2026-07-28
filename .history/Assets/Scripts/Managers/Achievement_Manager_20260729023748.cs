using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement_Manager : MonoBehaviour
{
    public static Achievement_Manager Instance {get ; private set;}
    [SerializeField] private List<Achievement_Data> _achievementsList;
    [SerializeField] private Image _achievementBoxImage;
    [SerializeField] private Animator _achievementBoxAnimator;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 
    }

    private void ShowAchievement(Achievement_Data _achievement)
    {
        _achievementBoxImage.sprite = _achievement._achievementImage;
        _achievementBoxAnimator.SetTrigger("PopUp");
    }

    public void TriggerAchievement(string _title)
    {
        foreach (Achievement_Data data in _achievementsList)
        {
            if (_title == data.GetTitle())
            {
                ShowAchievement(data);
                return;
            }
        }
        Debug.LogError("Achievement with name " + _title + " does not exist");
    }

}
