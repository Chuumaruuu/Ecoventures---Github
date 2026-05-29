using System;
using Unity.VisualScripting;
using UnityEngine;

public class Counter_Container : Counter_Base
{

    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private GameInventory_Data _mainData;
    [SerializeField] private Item_Data _itemData;
    [SerializeField] private int _itemPrice;

    public override void Interact(Player_Base _player) 
    {
        if(_itemData != null)
        {
            if (!_player.HasItem()) //player has no item
            {
                if (!this.HasItem()) // counter has no item
                {  
                    OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

                    if (_mainData.HasRecycledItem(_itemData)) // if there's a recycled base product, no cost to take an item
                    {
                        _mainData.RemoveRecycledItem(_itemData);
                    }
                    else 
                    {
                        _mainData.SubtractMoney(_itemPrice);
                    }
                    Item_Base.SpawnItem(_itemData, _player);

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
        else
        {
            Debug.LogError(this + " has no Item_Data attached");
        }
    }


}
