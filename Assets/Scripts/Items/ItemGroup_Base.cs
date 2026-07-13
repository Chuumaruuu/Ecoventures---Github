using UnityEngine;

public class ItemGroup_Base : MonoBehaviour
{
    public static ItemGroup_Base Instance;
    [SerializeField] private Item_Data item_Data;
    [SerializeField] public GameObject[] itemState;

    public Item_Data GetItemData() 
    {
        return item_Data;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

}
