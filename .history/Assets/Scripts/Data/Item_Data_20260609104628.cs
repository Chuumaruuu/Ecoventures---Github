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
}