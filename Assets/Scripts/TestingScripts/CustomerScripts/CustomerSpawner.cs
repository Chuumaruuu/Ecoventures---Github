using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Settings")]
    public GameObject[] customerPrefabs;   // 🔥 Multiple prefabs
    public float spawnInterval = 3f;
    public int maxCustomersInShop = 5;

    [Header("Environment")]
    public GameObject spawnFloor;
    public float spawnHeight = 0.5f;

    [Header("Spawn Area (Local to Floor)")]
    public Vector3 areaCenter = Vector3.zero;
    public Vector3 areaSize = new Vector3(4f, 0f, 4f);

    [Header("Scene References")]
    public ShopTable shopTable;
    public Transform exitPoint;

    [Header("Order System")]
    public OrderGenerator orderGenerator;

    [Header("UI")]
    public Canvas mainCanvas;
    public GameObject orderIconPrefab;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            TrySpawnCustomer();
            spawnTimer = spawnInterval;
        }
    }

    void TrySpawnCustomer()
    {
        if (CustomerTaskManager.Instance == null)
            return;

        if (!CustomerTaskManager.Instance.CanSpawn())
            return;

        if (customerPrefabs == null || customerPrefabs.Length == 0 || spawnFloor == null)
            return;

        if (orderGenerator == null || mainCanvas == null || orderIconPrefab == null)
        {
            Debug.LogError("Spawner missing references!");
            return;
        }

        int currentCustomers =
            FindObjectsByType<Customer>(FindObjectsSortMode.None).Length;

        if (currentCustomers >= maxCustomersInShop)
            return;

        Vector3 spawnPos = GetRandomPointInArea();
        if (spawnPos == Vector3.zero) return;

        // 🔥 RANDOM PREFAB
        GameObject selectedPrefab =
            customerPrefabs[Random.Range(0, customerPrefabs.Length)];

        GameObject obj =
            Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

        CustomerTaskManager.Instance.RegisterSpawn();

        Customer customer = obj.GetComponent<Customer>();
        if (customer == null) return;

        customer.shopTable = shopTable;
        customer.exitPoint = exitPoint;
        customer.desiredItem = orderGenerator.GetRandomItem();

        GameObject uiObj = Instantiate(orderIconPrefab, mainCanvas.transform);
        uiObj.SetActive(false);

        CustomerOrderUI orderUI = uiObj.GetComponent<CustomerOrderUI>();
        orderUI.Setup(customer);

        customer.orderUI = orderUI;
    }

    Vector3 GetRandomPointInArea()
    {
        Collider floorCollider = spawnFloor.GetComponent<Collider>();
        if (floorCollider == null)
        {
            Debug.LogError("Spawn Floor must have a Collider!");
            return Vector3.zero;
        }

        Vector3 worldCenter = spawnFloor.transform.position + areaCenter;
        Vector3 halfSize = areaSize / 2f;

        float x = Random.Range(worldCenter.x - halfSize.x, worldCenter.x + halfSize.x);
        float z = Random.Range(worldCenter.z - halfSize.z, worldCenter.z + halfSize.z);
        float y = floorCollider.bounds.min.y + spawnHeight;

        return new Vector3(x, y, z);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnFloor == null) return;

        Vector3 worldCenter = spawnFloor.transform.position + areaCenter;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(worldCenter, areaSize);
    }
}