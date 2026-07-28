using UnityEngine;
using UnityEngine.UI;
public class RecyclingTable_UI : MonoBehaviour
{
    [SerializeField] private GameInventory_Data _mainInventory;
    [SerializeField] private Image _itemImage;
    private Animator _recyclingAnimator;


    void Start()
    {
        _recyclingAnimator = GetComponent<Animator>();
    }
    
    public void NotifyItemAdd(Item_Data _itemData)
    {
        _itemImage.sprite = _itemData.GetItemImage();
        _recyclingAnimator.SetTrigger("PopUp");
    }

    public void NextItem()
    {
         
    }

}
