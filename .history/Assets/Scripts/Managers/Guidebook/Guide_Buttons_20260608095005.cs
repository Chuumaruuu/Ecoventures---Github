using UnityEngine;
using UnityEngine.UI;

public class Guide_Buttons : MonoBehaviour
{
    [SerializeField] private Button[] _recipeButtons;
    [SerializeField] private Image[] _recipeButtonImages;
    [SerializeField] private Item_Data[] _itemData;

    private void Start()
    {
        RefreshButtons();
    }

    private void OnValidate()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        if (_recipeButtons == null || _recipeButtonImages == null || _itemData == null)
        {
            return;
        }

        int count = Mathf.Min(_recipeButtons.Length, _recipeButtonImages.Length, _itemData.Length);

        for (int i = 0; i < count; i++)
        {
            Item_Data itemData = _recipeData[i];

            // Update the image sprite from the item data first
            if (_recipeButtonImages[i] != null && itemData != null && itemData._itemSprite != null)
            {
                _recipeButtonImages[i].sprite = itemData._itemSprite;
            }

            bool isUnlocked = itemData != null && itemData.isUnlocked;

            if (_recipeButtons[i] != null)
            {
                _recipeButtons[i].interactable = isUnlocked;
            }

            if (_recipeButtonImages[i] != null)
            {
                _recipeButtonImages[i].color = isUnlocked ? Color.white : Color.black;
            }
        }

        if (_recipeButtons.Length != _recipeButtonImages.Length || _recipeButtons.Length != _itemData.Length)
        {
            Debug.LogWarning("Guide_Buttons arrays should have matching lengths.", this);
        }
    }
}
