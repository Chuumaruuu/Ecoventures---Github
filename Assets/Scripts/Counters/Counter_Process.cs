using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Counter_Process : Counter_Base
{
    public event EventHandler OnProcessCounterInteract;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float _progressTimerNormalized;
    }
    [SerializeField] private ProcessRecipe_Data[] _processRecipeArray;

    private int _processTimer;
    public override void Interact(Player_Base _player)
    {
        if (!HasItem()) //counter has no item
        {
            if (_player.HasItem()) //player is carrying something
            {
                if (HasRecipeWithInput(_player.GiveItem().GetItemData())) //player is carrying an item with a progress recipe
                {
                    _player.GiveItem().SetItemParent(this);
                    _processTimer = 0;

                    ProcessRecipe_Data _processRecipeData = GetProcessRecipeDataWithInput(this.GiveItem().GetItemData());
                    
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        _progressTimerNormalized = (float)_processTimer / _processRecipeData._progressMax
                    });
                }
            }
            else
            {
                //player has nothing
            }
        }
        else
        {
            //there is an WorkshopObject here
            if (_player.HasItem())
            {
                //player is carrying something
            }
            else
            {
                // player has nothing 
                this.GiveItem().SetItemParent(_player);
            }
        }
    }

    public override void InteractAlternate(Player_Base _player)
    {
        if (this.HasItem() && HasRecipeWithInput(this.GiveItem().GetItemData()))
        {
            // There is a Kitchen object and it can be processed
            _processTimer++;

            ProcessRecipe_Data _processRecipeData = GetProcessRecipeDataWithInput(this.GiveItem().GetItemData());
            
            if (_processTimer >= _processRecipeData._progressMax)
            {
                _processTimer = 0;
                Item_Data _outputItemData = GetOutputForInput(this.GiveItem().GetItemData());
                //there is a workshop object here
                this.GiveItem().DestroySelf();

                Item_Base.SpawnItem(_outputItemData, this);
            }
            OnProcessCounterInteract?.Invoke(this, EventArgs.Empty);
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                _progressTimerNormalized = (float)_processTimer / _processRecipeData._progressMax
            });
            
            
        }
    }

    private bool HasRecipeWithInput(Item_Data _inputItemData)
    {
        ProcessRecipe_Data _processRecipeData = GetProcessRecipeDataWithInput(_inputItemData);
        return _processRecipeData != null;
    }

    private Item_Data GetOutputForInput(Item_Data _inputItemData)
    {
        ProcessRecipe_Data _processRecipeData = GetProcessRecipeDataWithInput(_inputItemData);
        if(_processRecipeData != null)
        {
            return _processRecipeData._outputItem;
        }
        else
        {
            return null;
        }
    }

    private ProcessRecipe_Data GetProcessRecipeDataWithInput(Item_Data _inputItemData)
    {
        foreach (ProcessRecipe_Data _recipe in _processRecipeArray)
        {
            if (_recipe._inputItem == _inputItemData)
            {
                return _recipe;
            }
        }
        return null;
    }
}
