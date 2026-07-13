using UnityEngine;
using System.Linq;
using TMPro;

public class ProductSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints = new Transform[3];
    private InventoryManager _inventoryManager;

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
            int countInInventory = _inventoryData._finalProducts.Count(item => item == itemData);

            if (spawnPoint == null || itemData == null || itemData._productGroupPrefab == null 
            || itemData.isUnlocked == false || countInInventory == 0)
            {
                // Debug.LogWarning($"ProductSpawner cannot spawn item at index {i} due to missing data or locked item.");
                Debug.Log("Spawn Point: " + (spawnPoint != null ? spawnPoint.name : "null"));
                Debug.Log("Item Data: " + (itemData != null ? itemData.name : "null"));
                Debug.Log("Count in Inventory: " + countInInventory);
                continue;
            }

            Debug.Log($"Spawning product: {itemData.name} at spawn point: {spawnPoint.name}");
            Transform spawnedGroup = Instantiate(itemData._productGroupPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
            spawnedGroup.localPosition = Vector3.zero;
            spawnedGroup.localRotation = Quaternion.identity;
        }
    }

    private void UpdateProductCountTexts()
    {
        if (_productCountTexts == null || _productCountTexts.Length == 0)
        {
            Debug.LogWarning("ProductSpawner has no product count texts assigned.");
            return;
        }

        for (int i = 0; i < _productCountTexts.Length; i++)
        {
            if (i >= _randomizerData._allowedItems.Count)
            {
                _productCountTexts[i].text = "0";
                continue;
            }

            Item_Data itemData = _randomizerData._allowedItems[i];
            int countInInventory = _inventoryData._finalProducts.Count(item => item == itemData);
            _productCountTexts[i].text = countInInventory.ToString();
        }
    }
}
