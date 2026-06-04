using UnityEngine;
using UnityEngine.UI;

public class Counter_Base : MonoBehaviour, IItemParent
{
    [SerializeField] private Transform _counterTop;
    [SerializeField] private Sprite _counterSprite;
    [SerializeField] protected Counter_Audio _counterAudio;


    private Item_Base _baseItem;

    public virtual void Interact(Player_Base _player) //needs to be overwritten
    {
        Debug.LogError("BaseCounter not overwritten"); // interact must always be overwritten depending on the countertype
    }

    public virtual void InteractAlternate(Player_Base _player) // not all counters have an altinteract
    {
        // nothing happens
    }
    
    public Sprite GetSprite()
    {
        return _counterSprite;
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
