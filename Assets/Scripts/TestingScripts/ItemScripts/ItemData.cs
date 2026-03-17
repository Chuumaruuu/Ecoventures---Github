using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public Sprite icon;
    public int sellprice;

    [Header("Optional Manual World Scale Override")]
    public bool useCustomWorldScale = false;
    public Vector3 customWorldScale = Vector3.one;
}