using UnityEngine;
using UnityEngine.UI;

public class Guide_Buttons : MonoBehaviour
{
    [SerializeField] private Button[] _recipeButtons;
    [SerializeField] private Image[] _recipeButtonImages;
    [SerializeField] private Item_Data[] _recipeData;

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
        if (_recipeButtons == null || _recipeButtonImages == null || _recipeData == null)
        {
            return;
        }

        int count = Mathf.Min(_recipeButtons.Length, _recipeButtonImages.Length, _recipeData.Length);

        for (int i = 0; i < count; i++)
        {
            Item_Data itemData = _recipeData[i];
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

        if (_recipeButtons.Length != _recipeButtonImages.Length || _recipeButtons.Length != _recipeData.Length)
        {
            Debug.LogWarning("Guide_Buttons arrays should have matching lengths.", this);
        }
    }
}
