using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public static OrderGenerator Instance;

    [SerializeField] private GameProduct_Randomizer_Data _randomizerData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Item_Data GetRandomItem()
    {
        if (_randomizerData == null || _randomizerData._allowedItems == null || _randomizerData._allowedItems.Count == 0)
        {
            Debug.LogWarning("OrderGenerator has no allowed items assigned!");
            return null;
        }

        return _randomizerData._allowedItems[Random.Range(0, _randomizerData._allowedItems.Count)];
    }
}