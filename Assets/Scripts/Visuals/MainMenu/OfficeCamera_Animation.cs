using System;
using UnityEngine;

public class OfficeCamera_Animation : MonoBehaviour
{
    [SerializeField] private Scene_Manager _sceneManager;
    [SerializeField] private Animator _officeCameraAnimator;

    public void TriggerOfficeCameraAnimation()
    {
        _officeCameraAnimator.SetTrigger("Transition");
    }

    public void SwitchToWorkshop()
    {
        _sceneManager.FadeToScene(1);
    }



}
