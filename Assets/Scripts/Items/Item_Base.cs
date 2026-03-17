using UnityEngine;

public class Item_Base : MonoBehaviour
{
    [SerializeField] private Item_Data _itemData;

    private IItemParent _itemParent;

    public Item_Data GetItemData() 
    {
        return _itemData;
    }

    public void SetItemParent(IItemParent _itemParent) 
    {
        if (this._itemParent != null) {
            this._itemParent.ClearItem();
        }

        this._itemParent = _itemParent;

        if (_itemParent.HasItem()) 
        {
            Debug.LogError("IItemParent already has an item!");
        }

        _itemParent.SetItem(this);

        transform.parent = _itemParent.GetItemFollowTransform();
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    public IItemParent GetItemParent() 
    {
        return _itemParent;
    }

    public void DestroySelf()
    {
        _itemParent.ClearItem();
        Destroy(gameObject);
    }

    public static Item_Base SpawnItem(Item_Data _itemData, IItemParent _itemParent) 
    {
        Transform _itemTransform = Instantiate(_itemData._itemPrefab);
        Item_Base _productionItem = _itemTransform.GetComponent<Item_Base>(); 
        _productionItem.SetItemParent(_itemParent);

        return _productionItem;
    }
}
