using TMPro;
using UnityEngine;
using System.Linq;

public class ContainerCounter_UI : MonoBehaviour
{
    [SerializeField] private Counter_Base _baseCounter;
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private TextMeshProUGUI _itemAmountText;
    [SerializeField ]private GameInventory_Data _mainInventory;

    private Item_Data _itemData;

    void Start()
    {
        Player_Base.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        _itemData = GetComponent<Counter_Container>().GetStorageItem();
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
        if (_mainInventory.GetExtraMaterials().Contains(_itemData))
        {
            _itemAmountText.text = _mainInventory.GetExtraMaterials().Count(item => item == _itemData).ToString();        
        }
        else
        {
            _itemAmountText.text = "₱" + _itemData.sellprice.ToString();
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
}
