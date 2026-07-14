using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] public GameInventory_Data gameInventoryData;
    [SerializeField] public GameProduct_Randomizer_Data randomizerData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
