using System;
using UnityEngine;

public class Counter_Disassemble : Counter_Base
{
    public event EventHandler OnItemDisassemble;

    [SerializeField] private GameInventory_Data _mainData;
    [SerializeField] private ItemDisassemblyRecipe_Data[] _disassemblyRecipeArray;
    [SerializeField] private RecyclingTable_UI _recyclingTableUI;


    public override void Interact(Player_Base _player)
    {
        if (_player.HasItem() && HasRecipeWithInput(_player.GiveItem().GetItemData())) // player has a final product item that can be disassembled
        {
            AudioManager.Instance.PlaySFX(_counterAudio._counterInteractSFX);
            
            ItemDisassemblyRecipe_Data _currentRecipe = CheckIfDisassemblyRecipeExists(_player.GiveItem().GetItemData());

            foreach (Item_Data output in _currentRecipe._outputItems)
            {
                _mainData.AddRecycledMaterials(output);
                // _recyclingTableUI.NotifyItemAdd(output);
            }
            _player.GiveItem().DestroySelf();

            OnItemDisassemble?.Invoke(this, EventArgs.Empty);

            // submit table interact audio oneshot
            // Debug.Log($"Added {randomAmount}x {submittedItem._objectName} to inventory.");
        }
        else // wrong combination
        {
            // nothing happens
        }
    }

    private bool HasRecipeWithInput(Item_Data _inputItemData)
    {
        return CheckIfDisassemblyRecipeExists(_inputItemData) != null;
    }

    private ItemDisassemblyRecipe_Data CheckIfDisassemblyRecipeExists(Item_Data _inputItemData)
    {
        foreach (ItemDisassemblyRecipe_Data _recipe in _disassemblyRecipeArray)
        {
            if (_recipe._inputItem == _inputItemData)
            {
                return _recipe;
            }
        }
        return null;
    }

}