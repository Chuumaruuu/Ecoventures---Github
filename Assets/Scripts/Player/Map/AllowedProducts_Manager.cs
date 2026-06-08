using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AllowedProducts_Manager : MonoBehaviour
{
    [SerializeField] private GameProduct_Randomizer_Data _randomizerData;

    [SerializeField] private Item_Data[] _allowedItem1;
    [SerializeField] private Item_Data[] _allowedItem2;
    [SerializeField] private Item_Data[] _allowedItem3;
    
    [SerializeField] private Image _item1, _item2, _item3;
    private List<Item_Data> _finalAllowedItems = new List<Item_Data>();

    void Start()
    {
        
        _item1.sprite = SetItem1();
        _item2.sprite = SetItem2();
        _item3.sprite = SetItem3();
    }

    public void SetItems()
    {
        if(_finalAllowedItems.Count == 0)
        {
            Debug.LogError("_finalAllowedItems being set while it is empty");
        }

        _randomizerData.ClearAllowedProduct();
        foreach (Item_Data i in _finalAllowedItems)
        {
            _randomizerData.AddAllowedProduct(i);
        }
    }

    private Sprite SetItem1()
    {
        _item1.color = Color.white;
        int _randomItemIndex = Random.Range(0,_allowedItem1.Length);

        while (_finalAllowedItems.Contains(_allowedItem1[_randomItemIndex]))
        {
            _randomItemIndex  = Random.Range(0,_allowedItem1.Length);
        }

        _finalAllowedItems.Add(_allowedItem1[_randomItemIndex]);
        Debug.Log(_allowedItem1[_randomItemIndex]+" Added");

        if (!_allowedItem1[_randomItemIndex].isUnlocked)
        {
            _item1.color = Color.black;
        }
        return _allowedItem1[_randomItemIndex]._itemSprite;
    }

    private Sprite SetItem2()
    {
        _item2.color = Color.white;
        int _randomItemIndex = Random.Range(0,_allowedItem2.Length);

        while (_finalAllowedItems.Contains(_allowedItem2[_randomItemIndex]))
        {
            _randomItemIndex  = Random.Range(0,_allowedItem2.Length);
        }

        _finalAllowedItems.Add(_allowedItem2[_randomItemIndex]);
        Debug.Log(_allowedItem2[_randomItemIndex]+" Added");

        if (!_allowedItem2[_randomItemIndex].isUnlocked)
        {
            _item2.color = Color.black;
        }
        return _allowedItem2[_randomItemIndex]._itemSprite;
    }

    private Sprite SetItem3()
    {
        _item3.color = Color.white;
        int _randomItemIndex = Random.Range(0,_allowedItem3.Length);

        while (_finalAllowedItems.Contains(_allowedItem3[_randomItemIndex]))
        {
            _randomItemIndex  = Random.Range(0,_allowedItem3.Length);
        }

        _finalAllowedItems.Add(_allowedItem3[_randomItemIndex]);
        Debug.Log(_allowedItem3[_randomItemIndex]+" Added");

        if (!_allowedItem3[_randomItemIndex].isUnlocked)
        {
            _item3.color = Color.black;
        }
        return _allowedItem3[_randomItemIndex]._itemSprite;
    }
}
