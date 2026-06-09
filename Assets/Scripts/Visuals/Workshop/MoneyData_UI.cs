using TMPro;
using UnityEngine;
public class MoneyData_UI : MonoBehaviour
{
    [SerializeField] private GameInventory_Data _mainInventory;
    [SerializeField] private TextMeshProUGUI _moneyText;
    void Start()
    {
        UpdateMoneyUI();
        GameInventory_Data.OnMoneyValueChanged += UpdateMoneyUI;
    }

    public void UpdateMoneyUI()
    {
        _moneyText.text = _mainInventory._playerMoney.ToString();
    }
}
