using System;
using UnityEngine;

public class Counter_StorageBin : Counter_Base
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private GameInventory_Data _mainData;

    public override void Interact(Player_Base _player) 
    {
        if (!_player.HasItem()) //player has no item
        {
            if (!this.HasItem()) // counter has no item on top of it
            {  
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

                if (_mainData._finalProducts != null && _mainData._finalProducts.Count > 0)
                {
                    Item_Data _item = _mainData._finalProducts[0];
                    _mainData._finalProducts.RemoveAt(0);
                    Item_Base.SpawnItem(_item,_player);
                }
                AudioManager.Instance.PlaySFX(_counterAudio._counterInteractSFX);
            } 
            else // counter has item
            { 
                this.GiveItem().SetItemParent(_player);
            }
        }
        else // player has item
        {
            if (!this.HasItem()) //counter does not have an item
            {
                _player.GiveItem().SetItemParent(this);
            }
        }
    }


}
