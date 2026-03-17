using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    public ItemData GetItemData()
    {
        return itemData;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}