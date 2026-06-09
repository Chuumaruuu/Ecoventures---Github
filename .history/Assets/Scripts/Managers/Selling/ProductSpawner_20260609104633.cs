using UnityEngine;

public class ProductSpawner : MonoBehaviour
{
    [SerializeField] private GameProduct_Randomizer_Data _randomizerData;
    [SerializeField] private Transform[] _spawnPoints = new Transform[3];

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

            if (spawnPoint == null || itemData == null || itemData._productGroupPrefab == null)
                continue;

            Transform spawnedGroup = Instantiate(itemData._productGroupPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
            spawnedGroup.localPosition = Vector3.zero;
            spawnedGroup.localRotation = Quaternion.identity;

            if (itemData.useCustomWorldScale)
                spawnedGroup.localScale = itemData.customWorldScale;
            else
                spawnedGroup.localScale = Vector3.one;
        }
    }
}
