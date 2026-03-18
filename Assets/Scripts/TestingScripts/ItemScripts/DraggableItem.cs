using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Item_Data itemData;
    [HideInInspector] public TableSlot sourceSlot;

    private Vector3 startPos;
    private Transform startParent;
    private Camera cam;

    [HideInInspector] public bool wasAccepted = false;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        wasAccepted = false;
        startPos = transform.position;
        startParent = transform.parent;
        transform.SetParent(null);

        //Add sfx when item is dragged
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(eventData.position);
        // Use a horizontal plane at the item's original height
        Plane plane = new Plane(Vector3.up, new Vector3(0, startPos.y, 0));
        if (plane.Raycast(ray, out float dist))
        {
            Vector3 worldPos = ray.GetPoint(dist);
            // Keep item slightly above the table surface
            transform.position = new Vector3(worldPos.x, startPos.y + 0.3f, worldPos.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!wasAccepted)
        {
            // Snap back to original slot
            transform.SetParent(startParent);
            transform.position = startPos;

            // Re-register with source slot if it was cleared
            if (sourceSlot != null && sourceSlot.IsEmpty)
            {
                sourceSlot.RestoreItem(this);
            }
        }
    }
}