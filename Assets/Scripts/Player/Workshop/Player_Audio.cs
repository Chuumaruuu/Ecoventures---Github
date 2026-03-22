using UnityEngine;
using System;

public class Player_Audio : MonoBehaviour
{
    
    [SerializeField] private Player_Input _playerInput;
    [SerializeField] private AudioClip _footStepSFX;
    [SerializeField] private AudioClip _pickUpSFX;
    [SerializeField] private AudioClip _dropSFX;
    
    private AudioSource _footstepLoop = null;
    void Start()
    {
        _playerInput.OnInteractAction += PlayerOnInteractAction;
    }

    private void Update()
    {
        
        if (Player_Base.Instance.IsWalking())
        {
            if (_footstepLoop == null)
            {
                _footstepLoop = AudioManager.Instance.PlayLoopedSFX(_footStepSFX);
            }
        }
        else
        {
            if (_footstepLoop != null)
            {
                AudioManager.Instance.StopLoopedSFX(_footstepLoop);
                _footstepLoop = null;
            }
        }

    }

    private void OnPlayerWalkStepAudioCue()
    {
        AudioManager.Instance.PlaySFX(_footStepSFX);
    }

    private void PlayerOnInteractAction(object _sender, EventArgs e) //if the player detects a counter in front of it, call out that counter's Interact() and call out that the player has grabbed an object
    {
        if (Player_Base.Instance.DetectsACounter()) 
        {
            if (Player_Base.Instance.SelectedCounter().HasItem() && !Player_Base.Instance.HasItem()) // player dropped an item
            {
                AudioManager.Instance.PlaySFX(_pickUpSFX);
            }
            else if (!Player_Base.Instance.SelectedCounter().HasItem() && Player_Base.Instance.HasItem()) //player picked up an item
            {
                AudioManager.Instance.PlaySFX(_dropSFX);
            }
        }
    }
}
