using UnityEngine;

public class ShopTable : MonoBehaviour
{
    [Header("Grid Settings")]
    public int rows = 2;
    public int columns = 5;

    [Header("Slot Spacing")]
    public float slotSpacingX = 1f;
    public float slotSpacingZ = 1f;

    public GameObject slotPrefab;
    [HideInInspector] public TableSlot[] slots;
    public int maxItems => rows * columns;

    private void Awake()
    {
        GenerateSlots();
    }

    void GenerateSlots()
    {
        slots = new TableSlot[rows * columns];
        float totalWidth = (columns - 1) * slotSpacingX;
        float totalDepth = (rows - 1) * slotSpacingZ;
        Vector3 startPos = transform.position - new Vector3(totalWidth / 2f, 0f, -totalDepth / 2f);

        int index = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 slotPos = startPos + new Vector3(c * slotSpacingX, 0f, -r * slotSpacingZ);
                GameObject newSlot = Instantiate(slotPrefab, slotPos, Quaternion.identity, transform);
                newSlot.name = $"Slot_{index}";
                TableSlot slot = newSlot.GetComponent<TableSlot>();
                slot.ClearSlot();
                slots[index] = slot;
                index++;
            }
        }
    }

    public bool PlaceItemOnTable(Item_Data item)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.IsEmpty)
            {
                slot.PlaceItem(item);
                return true;
            }
        }
        return false;
    }

    public Item_Data TakeItem(Item_Data wantedItem)
    {
        if (wantedItem == null) return null;
        foreach (var slot in slots)
        {
            if (slot != null && !slot.IsEmpty && slot.item == wantedItem)
            {
                return slot.TakeItem();
            }
        }
        return null;
    }
}