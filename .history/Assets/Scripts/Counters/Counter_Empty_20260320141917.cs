using System;
using UnityEngine;

public class Counter_Empty : Counter_Base 
{
    [SerializeField] private ItemCombinationRecipe_Data[] _combinationRecipeArray;
    public override void Interact(Player_Base _player) 
    {
        if (!HasItem()) //counter has no item
        {
            if (_player.HasItem()) // player has item
            {
                _player.GiveItem().SetItemParent(this);
                // player pickup audio oneshot
                SoundManager.Instance.PlaySFX(SoundManager.Instance.pickupClip);
            }
            else // player has nothing
            {
                // No audio needed
            }
        }
        else // counter has item
        {
            if (_player.HasItem()) // player has item
            {
                // if player's Item and counter's item are inputs to a combination recipe, destroy both to get the output
                if (this.HasCombinationWithInput(this.GiveItem().GetItemData(), _player.GiveItem().GetItemData()))
                {

                    Item_Data _outputItemData = GetOutputForInputs(this.GiveItem().GetItemData(), _player.GiveItem().GetItemData());
                    
                    _player.GiveItem().DestroySelf();
                    this.GiveItem().DestroySelf();

                    Item_Base.SpawnItem(_outputItemData, this);

                    // player drop audio oneshot
                    SoundManager.Instance.PlaySFX(SoundManager.Instance.dropClip);
                }
                
            }
            else // player has nothing 
            {
                this.GiveItem().SetItemParent(_player);
                // player pickup audio oneshot
                SoundManager.Instance.PlaySFX(SoundManager.Instance.pickupClip);
            }
        }
    }

    private bool HasCombinationWithInput(Item_Data _inputItemData1, Item_Data _inputItemData2)
    {
        ItemCombinationRecipe_Data _combinationRecipe = GetCombinationRecipeDataWithInput(_inputItemData1, _inputItemData2);
        return _combinationRecipe != null;
    }

    private Item_Data GetOutputForInputs(Item_Data _inputItemData1, Item_Data _inputItemData2)
    {
        ItemCombinationRecipe_Data _combinationRecipe = GetCombinationRecipeDataWithInput(_inputItemData1, _inputItemData2);
        if(_combinationRecipe != null)
        {
            return _combinationRecipe._outputItem;
        }
        else
        {
            return null;
        }
    }

    private ItemCombinationRecipe_Data GetCombinationRecipeDataWithInput(Item_Data _inputItemData1, Item_Data _inputItemData2)
    {
        foreach (ItemCombinationRecipe_Data _recipe in _combinationRecipeArray)
        {
            if (_inputItemData1 == _recipe._item1 && _inputItemData2 == _recipe._item2 || _inputItemData1 == _recipe._item2 && _inputItemData2 == _recipe._item1)
            {
                Debug.Log(_inputItemData1 + " " + _inputItemData2);
                return _recipe;
            }
        }
        return null;
    }
}