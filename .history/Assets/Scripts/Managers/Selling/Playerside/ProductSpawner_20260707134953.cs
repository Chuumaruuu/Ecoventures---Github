using UnityEngine;

public class ProductSpawner : MonoBehaviour
{
    [SerializeField] private GameProduct_Randomizer_Data _randomizerData;
    [SerializeField] private Transform[] _spawnPoints = new Transform[3];
    [SerializeField] private GameInventory_Data _inventoryData;

    private void Start()
    {
        SpawnProducts();
    }

    public void SpawnProducts()
    {
        if (_randomizerData == null)
        {
            Debug.LogWarning("ProductSpawner is missing randomizer data.");
            return;
        }

        if (_randomizerData._allowedItems == null || _randomizerData._allowedItems.Count == 0)
        {
            Debug.LogWarning("ProductSpawner found no allowed items to spawn.");
            return;
        }

        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogWarning("ProductSpawner has no spawn points assigned.");
            return;
        }

        int spawnCount = Mathf.Min(_spawnPoints.Length, _randomizerData._allowedItems.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = _spawnPoints[i];
            Item_Data itemData = _randomizerData._allowedItems[i];

            Debug.Log("This is the item:" + _inventoryData._finalProducts[i]);

            if (spawnPoint == null || itemData == null || itemData._productGroupPrefab == null 
            || itemData.isUnlocked == false || _inventoryData._finalProducts[i] == null)
            {
                Debug.LogWarning($"ProductSpawner cannot spawn item at index {i} due to missing data or locked item.");
                return;
            }

            Transform spawnedGroup = Instantiate(itemData._productGroupPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
            spawnedGroup.localPosition = Vector3.zero;
            spawnedGroup.localRotation = Quaternion.identity;
        }
    }
}
