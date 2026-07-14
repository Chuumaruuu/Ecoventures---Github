using UnityEngine;

public class SellingProductsUIManager : MonoBehaviour
{
    [SerializeField] private GameProduct_Randomizer_Data _randomizerData;
    [SerializeField] private SellingProductButton[] sellingButtons = new SellingProductButton[3];

    private void Start()
    {
        AssignProductsToButtons();
    }

    private void AssignProductsToButtons()
    {
        if (_randomizerData == null)
        {
            Debug.LogWarning("SellingProductsUIManager: randomizer data not assigned.");
            return;
        }

        if (sellingButtons == null || sellingButtons.Length == 0)
        {
            Debug.LogWarning("SellingProductsUIManager: no selling buttons assigned.");
            return;
        }

        int count = Mathf.Min(sellingButtons.Length, _randomizerData._allowedItems.Count);

        // Assign items in the same order as the allowed items list so button positions remain consistent.
        for (int i = 0; i < sellingButtons.Length; i++)
        {
            if (i < count)
            {
                sellingButtons[i].SetProduct(_randomizerData._allowedItems[i]);
                sellingButtons[i].gameObject.SetActive(true);
            }
            else
            {
                sellingButtons[i].ClearProduct();
                sellingButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
