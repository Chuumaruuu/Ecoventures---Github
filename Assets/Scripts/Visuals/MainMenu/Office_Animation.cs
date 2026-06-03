using System;
using UnityEngine;

public class Office_Animation : MonoBehaviour
{
    [SerializeField] private Scene_Manager _sceneManager;
    [SerializeField] private Animator _targetAnimator;

    [SerializeField] private GameObject _optionsBox;

    public void TriggerAnimation(string _triggerName)
    {
        _targetAnimator.SetTrigger(_triggerName);
    }

    private void SetActive()
    {
        _optionsBox.SetActive(false);
    }

    public void SwitchToWorkshop()
    {
        _sceneManager.FadeToScene(1);
    }

}
