using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private GameInventory_Data gameInventoryData;
    [SerializeField] private GameProduct_Randomizer_Data randomizerData;
}
