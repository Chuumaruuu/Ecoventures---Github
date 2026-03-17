using UnityEngine;

public class Counter_Base : MonoBehaviour, IItemParent
{
    [SerializeField] private Transform _counterTop;

    private Item_Base _baseItem;

    public virtual void Interact(Player_Base _player)
    {
        Debug.LogError("BaseCounter not overwritten"); // interact must always be overwritten depending on the countertype
    }

    public virtual void InteractAlternate(Player_Base _player)
    {
        // nothing happens
    }
    
    public Transform GetItemFollowTransform() 
    {
        return _counterTop;
    }
    public void SetItem(Item_Base _baseItem) 
    {
        this._baseItem = _baseItem;
    }
    public Item_Base GiveItem() 
    {
        return _baseItem;
    }
    public void ClearItem() 
    {
        _baseItem = null;
    }
    public bool HasItem() 
    {
        return _baseItem != null;
    }
}
