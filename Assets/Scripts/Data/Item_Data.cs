using UnityEngine;

[CreateAssetMenu(fileName = "Item_Data", menuName = "Scriptable Objects/Item_Data")]
public class Item_Data : ScriptableObject
{
    public Transform _productGroupPrefab;
    public Transform _itemPrefab;
    public Sprite _itemSprite;
    public string _objectName;
    public bool isUnlocked = false;
    public int sellprice;
    [Header("Optional Manual World Scale Override")]
    public bool useCustomWorldScale = false;
    public Vector3 customWorldScale = Vector3.one;
}