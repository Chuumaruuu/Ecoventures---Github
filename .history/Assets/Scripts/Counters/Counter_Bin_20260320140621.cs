using System;
using UnityEngine;

public class Counter_Bin : Counter_Base
{

    public event EventHandler OnPlayerThrewObject;
    public override void Interact(Player_Base _player)
    {
        if(_player.HasItem())
        {
            OnPlayerThrewObject?.Invoke(this, EventArgs.Empty);
            _player.GiveItem().DestroySelf();
            
            // bin interact audio oneshot
            SoundManager.Instance.PlaySFX(SoundManager.Instance.binInteractClip);
        }
    }
}
