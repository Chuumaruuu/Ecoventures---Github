using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    public static OrderGenerator Instance;

    [Header("Available Items")]
    public Item_Data[] availableItems;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Item_Data GetRandomItem()
    {
        if (availableItems == null || availableItems.Length == 0)
        {
            Debug.LogWarning("OrderGenerator has no items assigned!");
            return null;
        }
        return availableItems[Random.Range(0, availableItems.Length)];
    }
}