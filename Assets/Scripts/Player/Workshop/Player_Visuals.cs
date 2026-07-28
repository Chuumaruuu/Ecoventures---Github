using UnityEngine;
using System;

public class Player_Visuals : MonoBehaviour
{
    [SerializeField] Player_Base _player;

    private Animator _playerAnimator;
    private const string IS_WALKING = "Move";
    private const string IS_CARRYING = "Carry";


    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _player.OnPlayerGrabbedObject+= OnPlayerCarryingObject;
    }

    private void Update()
    {
       _playerAnimator.SetBool(IS_WALKING, _player.IsWalking());
    }

    private void OnPlayerCarryingObject()
    {
        _playerAnimator.SetBool(IS_CARRYING, _player.HasItem());
    }

    private void WalkStepInvoke()
    {
        
    }

}
