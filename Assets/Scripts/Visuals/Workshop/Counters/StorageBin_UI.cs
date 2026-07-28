using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageBin_UI : MonoBehaviour
{
    [SerializeField] private Counter_Base _baseCounter;
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private Image _itemImage;
    [SerializeField ]private GameInventory_Data _mainInventory;


    void Start()
    {
        Player_Base.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        UpdateContainerUI();
        GameInventory_Data.OnInventoryDataChanged += UpdateContainerUI;
    }

    void LateUpdate()
    {
        _uiPanel.transform.forward = Camera.main.transform.forward;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player_Base.OnSelectedCounterChangedEventArgs e)
    {
        if (e._selectedCounter == _baseCounter) 
        {
            Show();
        } 
        else 
        {
            Hide();
        }
    }


    public void UpdateContainerUI()
    {
        if (_mainInventory._finalProducts.Count == 0)
        {
            NoImage();
        }
        else
        {
            Color color = _itemImage.color;
            color.a = 1f;
            _itemImage.sprite = _mainInventory._finalProducts[0].GetItemImage();
        }
       
    }

    private void Show()
    {
        _uiPanel.SetActive(true);
    }

    private void Hide()
    {
        _uiPanel.SetActive(false);
    }

    private void NoImage()
    {
        Color color = _itemImage.color;
        color.a = 0f;
    }
}
