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
    private AudioManager AudioSFX;
    [HideInInspector] public bool wasAccepted = false;
    [SerializeField] private AudioClip dragClip;

    private void Awake()
    {
        cam = Camera.main;
        AudioSFX = AudioManager.Instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        wasAccepted = false;
        startPos = transform.position;
        startParent = transform.parent;
        transform.SetParent(null);

        if (AudioSFX != null && dragClip != null)
            AudioSFX.PlaySFX(dragClip);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.up, new Vector3(0, startPos.y, 0));
        if (plane.Raycast(ray, out float dist))
        {
            Vector3 worldPos = ray.GetPoint(dist);
            transform.position = new Vector3(worldPos.x, startPos.y + 0.3f, worldPos.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!wasAccepted)
        {
            transform.SetParent(startParent);
            transform.position = startPos;

            if (sourceSlot != null && sourceSlot.IsEmpty)
            {
                sourceSlot.RestoreItem(this);
            }
        }
    }
}