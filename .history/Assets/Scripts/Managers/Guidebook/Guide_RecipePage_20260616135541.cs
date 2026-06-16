using UnityEngine;
using UnityEngine.UI;

public class Guide_RecipePage : MonoBehaviour
{
    [SerializeField] private Image _leftPageImage;
    [SerializeField] private Image _rightPageImage;
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _nextButton;

    private Recipe_Data _currentRecipeData;
    private int _currentSpriteIndex;

    private void Awake()
    {
        ClearRecipe();
    }

    public void ShowRecipe(Recipe_Data recipeData)
    {
        _currentRecipeData = recipeData;
        _currentSpriteIndex = 0;

        if (_currentRecipeData == null || _currentRecipeData._recipeSprite == null || _currentRecipeData._recipeSprite.Length == 0)
        {
            ClearRecipe();
            return;
        }

        gameObject.SetActive(true);
        RefreshPage();
        Debug.Log("Showing recipe: " + recipeData.name);
    }

    public void ClearRecipe()
    {
        Debug.Log("Clearing recipe page.");
        _currentRecipeData = null;
        _currentSpriteIndex = 0;

        if (_leftPageImage != null)
        {
            _leftPageImage.gameObject.SetActive(false);
            _leftPageImage.sprite = null;
        }

        if (_rightPageImage != null)
        {
            _rightPageImage.gameObject.SetActive(false);
            _rightPageImage.sprite = null;
        }

        SetButtonActive(_previousButton, false);
        SetButtonActive(_nextButton, false);

        gameObject.SetActive(false);
    }

    public void NextPage()
    {
        if (!CanGoNext())
        {
            return;
        }

        _currentSpriteIndex += 2;
        RefreshPage();
    }

    public void PreviousPage()
    {
        if (!CanGoPrevious())
        {
            return;
        }

        _currentSpriteIndex -= 2;
        RefreshPage();
    }

    private void RefreshPage()
    {
        Sprite[] sprites = _currentRecipeData != null ? _currentRecipeData._recipeSprite : null;

        if (sprites == null || sprites.Length == 0)
        {
            ClearRecipe();
            return;
        }

        bool hasLeftSprite = _currentSpriteIndex >= 0 && _currentSpriteIndex < sprites.Length && sprites[_currentSpriteIndex] != null;
        bool hasRightSprite = _currentSpriteIndex + 1 < sprites.Length && sprites[_currentSpriteIndex + 1] != null;

        if (_leftPageImage != null)
        {
            _leftPageImage.gameObject.SetActive(hasLeftSprite);

            if (hasLeftSprite)
            {
                _leftPageImage.sprite = sprites[_currentSpriteIndex];
            }
        }

        if (_rightPageImage != null)
        {
            _rightPageImage.gameObject.SetActive(hasRightSprite);

            if (hasRightSprite)
            {
                _rightPageImage.sprite = sprites[_currentSpriteIndex + 1];
            }
        }

        SetButtonActive(_previousButton, CanGoPrevious());
        SetButtonActive(_nextButton, CanGoNext());
    }

    private bool CanGoPrevious()
    {
        return _currentRecipeData != null && _currentSpriteIndex > 0;
    }

    private bool CanGoNext()
    {
        Sprite[] sprites = _currentRecipeData != null ? _currentRecipeData._recipeSprite : null;
        return sprites != null && _currentSpriteIndex + 2 < sprites.Length;
    }

    private void SetButtonActive(Button button, bool isActive)
    {
        if (button != null)
        {
            button.gameObject.SetActive(isActive);
        }
    }
}
