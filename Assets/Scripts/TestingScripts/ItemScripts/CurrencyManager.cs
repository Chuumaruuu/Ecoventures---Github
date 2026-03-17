using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static int CurrentCurrency = 0;

    public TMP_Text moneyText; // Assign in Inspector
    public static CurrencyManager Instance;

    private void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public static void AddMoney(int amount)
    {
        CurrentCurrency += amount;

        if (Instance != null)
            Instance.UpdateUI();
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = "₱ " + CurrentCurrency.ToString("N0");
    }
}
