using UnityEngine;
using UnityEngine.UI;

public class OnHandIndicator_UI : MonoBehaviour
{
    [SerializeField] private GameObject _onHandIndicatorUI;
    [SerializeField] private Image _heldItemSprite;

    private Item_Data _heldItem;
    void Start()
    {
        Player_Base.Instance.OnObjectPickup += SetIndicatorSprite;
        Player_Base.Instance.OnObjectDrop += SetIndicatorSprite;
    }

    private void SetIndicatorSprite()
    {
        if (Player_Base.Instance.HasItem())
        {
            _heldItem = Player_Base.Instance.GiveItem().GetItemData();
            _heldItemSprite.sprite = _heldItem._itemSprite;
        }

        _onHandIndicatorUI.SetActive(Player_Base.Instance.HasItem());
    }
}
