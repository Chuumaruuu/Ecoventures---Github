using UnityEngine;

public interface IItemParent 
{
    public Transform GetItemFollowTransform();
    public void SetItem(Item_Base _productionItem);
    public Item_Base GiveItem();
    public void ClearItem();
    public bool HasItem();

}
