using UnityEngine;

public class TableSlot : MonoBehaviour
{
    public Item_Data item;
    private GameObject spawnedItem;

    [Header("Slot Visual (Editor Only)")]
    public Vector3 slotSize = new Vector3(0.8f, 0.2f, 0.8f);
    public Color emptyColor = Color.green;
    public Color filledColor = Color.red;

    [Header("Item Placement")]
    public float itemHeightOffset = 0.5f;

    [Header("Item Scaling")]
    public bool scaleToFitSlot = true;
    [Range(0.1f, 1f)]
    public float scalePadding = 0.9f;

    [Header("Scale Limits")]
    public float minScale = 0.5f;
    public float maxScale = 2f;

    public bool IsEmpty => item == null;

    public void PlaceItem(Item_Data newItem)
    {
        if (!IsEmpty)
            return;

        item = newItem;
        Vector3 spawnPos = transform.position + Vector3.up * itemHeightOffset;
        spawnedItem = Instantiate(
            newItem._itemPrefab.gameObject,
            spawnPos,
            transform.rotation,
            transform
        );

        spawnedItem.transform.localScale = Vector3.one;

        if (newItem.useCustomWorldScale)
        {
            spawnedItem.transform.localScale = newItem.customWorldScale;
        }
        else if (scaleToFitSlot)
        {
            ScaleItemToSlot();
        }

        DraggableItem drag = spawnedItem.GetComponent<DraggableItem>();
        if (drag != null)
        {
            drag.itemData = newItem;
            drag.sourceSlot = this;
        }
    }

    private void ScaleItemToSlot()
    {
        if (spawnedItem == null) return;
        Renderer renderer = spawnedItem.GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        Bounds bounds = renderer.bounds;
        Vector3 itemSize = bounds.size;

        if (itemSize.x <= 0f || itemSize.z <= 0f)
            return;

        float scaleX = (slotSize.x * scalePadding) / itemSize.x;
        float scaleZ = (slotSize.z * scalePadding) / itemSize.z;
        float uniformScale = Mathf.Min(scaleX, scaleZ);
        uniformScale = Mathf.Clamp(uniformScale, minScale, maxScale);

        spawnedItem.transform.localScale = Vector3.one * uniformScale;
    }

    public Item_Data TakeItem()
    {
        if (IsEmpty)
            return null;

        if (spawnedItem != null)
            Destroy(spawnedItem);

        Item_Data taken = item;
        item = null;
        spawnedItem = null;
        return taken;
    }

    public void ClearSlot()
    {
        item = null;
        if (spawnedItem != null)
            Destroy(spawnedItem);
        spawnedItem = null;
    }

    public void RestoreItem(DraggableItem drag)
    {
        item = drag.itemData;
        spawnedItem = drag.gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsEmpty ? emptyColor : filledColor;
        Gizmos.DrawWireCube(transform.position, slotSize);
    }
}