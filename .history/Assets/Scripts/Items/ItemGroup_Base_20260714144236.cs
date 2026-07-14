using UnityEngine;

// Attached to every groupedItems prefab. Holds a reference to the item this
// group represents and the array of child GameObjects used to represent its
// remaining-stock visual states (see ProductPrefabChanger).
public class ItemGroup_Base : MonoBehaviour
{
    [SerializeField] private Item_Data item_Data;
    [SerializeField] public GameObject[] itemState;

    public Item_Data GetItemData()
    {
        return item_Data;
    }
}