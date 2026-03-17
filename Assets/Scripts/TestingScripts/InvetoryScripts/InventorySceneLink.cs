using UnityEngine;

/// <summary>
/// Assigns the inventory grid to InventoryManager automatically
/// </summary>
public class InventorySceneLink : MonoBehaviour
{
    private void Awake()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.SetGridParent(transform);
            InventoryManager.Instance.PopulateInventoryUI();
        }
    }
}