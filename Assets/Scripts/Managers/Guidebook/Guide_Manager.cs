using UnityEngine;

public class Guide_Manager : MonoBehaviour
{
    [System.Serializable]
    private class MainPageGroup
    {
        public GameObject mainPage;
        public Recipe_Data[] recipeData;
    }

    [SerializeField] private MainPageGroup[] _mainPages;
    [SerializeField] private Guide_RecipePage _recipePage;

    private int _currentMainPageIndex = -1;

    private void Start()
    {
        setCurrentMainPage(0);
    }

    public void setCurrentPage(int index)
    {
        setCurrentMainPage(index);
    }

    public void setCurrentMainPage(int index)
    {
        if (_mainPages == null || index < 0 || index >= _mainPages.Length)
        {
            Debug.LogWarning("Index out of range: " + index);
            return;
        }

        for (int i = 0; i < _mainPages.Length; i++)
        {
            if (_mainPages[i].mainPage != null)
            {
                _mainPages[i].mainPage.SetActive(i == index);
            }
        }

        _currentMainPageIndex = index;

        if (_recipePage != null)
        {
            _recipePage.ClearRecipe();
        }

        Recipe_Data[] recipeData = _mainPages[index].recipeData;

        if (recipeData != null && recipeData.Length > 0)
        {
            setCurrentRecipePage(0);
        }
    }

    public void setCurrentRecipePage(int index)
    {
        if (_currentMainPageIndex < 0 || _currentMainPageIndex >= _mainPages.Length)
        {
            Debug.LogWarning("No main page is currently selected.");
            return;
        }

        if (_recipePage == null)
        {
            Debug.LogWarning("No recipe page view is assigned.");
            return;
        }

        Recipe_Data[] recipeData = _mainPages[_currentMainPageIndex].recipeData;

        if (recipeData == null || recipeData.Length == 0)
        {
            Debug.LogWarning("Current main page has no recipe data assigned.");
            return;
        }

        if (index < 0 || index >= recipeData.Length)
        {
            Debug.LogWarning("Recipe index out of range: " + index);
            return;
        }

        SetMainPagesActive(false);
        _recipePage.ShowRecipe(recipeData[index]);
    }

    public void setCurrentRecipe(Recipe_Data recipeData)
    {
        if (_recipePage == null)
        {
            Debug.LogWarning("No recipe page view is assigned.");
            return;
        }

        SetMainPagesActive(false);
        _recipePage.ShowRecipe(recipeData);
    }

    private void SetMainPagesActive(bool isActive)
    {
        if (_mainPages == null)
        {
            return;
        }

        for (int i = 0; i < _mainPages.Length; i++)
        {
            if (_mainPages[i].mainPage != null)
            {
                _mainPages[i].mainPage.SetActive(isActive);
            }
        }
    }
}
