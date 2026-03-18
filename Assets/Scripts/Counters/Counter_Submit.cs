using System;
using UnityEngine;

public class Counter_Submit : Counter_Base
{
    public event EventHandler OnItemSubmit;

    [SerializeField] private Item_Data[] _finalProducts;
    [SerializeField] private int _minAmount = 1;
    [SerializeField] private int _maxAmount = 3;

    public override void Interact(Player_Base _player)
    {
        if (_player.HasItem() && HasRecipeWithInput(_player.GiveItem().GetItemData())) // player has a final product item
        {

            Item_Data submittedItem = _player.GiveItem().GetItemData();
            _player.GiveItem().DestroySelf();

            int randomAmount = UnityEngine.Random.Range(_minAmount, _maxAmount + 1);
            for (int i = 0; i < randomAmount; i++)
            {
                InventoryManager.Instance.AddItemToInventory(submittedItem);
            }

            OnItemSubmit?.Invoke(this, EventArgs.Empty);

            // submit table interact audio oneshot
            Debug.Log($"✅ Added {randomAmount}x {submittedItem._objectName} to inventory.");
        }
    }

    private bool HasRecipeWithInput(Item_Data _inputItemData)
    {
        return CheckIfProductExists(_inputItemData) != null;
    }

    private Item_Data CheckIfProductExists(Item_Data _inputItemData)
    {
        foreach (Item_Data _recipe in _finalProducts)
        {
            if (_recipe == _inputItemData)
            {
                return _recipe;
            }
        }
        return null;
    }
}