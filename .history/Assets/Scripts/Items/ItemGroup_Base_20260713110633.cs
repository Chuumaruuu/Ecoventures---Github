using UnityEngine;

public class ItemGroup_Base : MonoBehaviour
{
    [SerializeField] private Item_Data item_Data;
    [SerializeField] private GameObject[] itemState;

    public Item_Data GetItemData() 
    {
        return item_Data;
    }

}
