using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Toggle[] _mainPageToggles;
    [SerializeField] private GameObject _backBtn;

    private int _currentMainPageIndex = -1;

    private void Start()
    {
        setCurrentMainPage(0);
    }

    public void ResetToFirstPage()
    {
        if (_mainPageToggles != null && _mainPageToggles.Length > 0 && _mainPageToggles[0] != null)
        {
            _mainPageToggles[0].isOn = true; // Toggle Group handles turning the rest off
        }

        setCurrentMainPage(0);
    }

    public void GoBack()
    {
        if (_currentMainPageIndex < 0 || _currentMainPageIndex >= _mainPages.Length)
        {
            return;
        }

        // Restore the main page
        SetMainPagesActive(false);
        _mainPages[_currentMainPageIndex].mainPage.SetActive(true);

        // Restore the toggle
        if (_mainPageToggles != null && _currentMainPageIndex < _mainPageToggles.Length && _mainPageToggles[_currentMainPageIndex] != null)
        {
            _mainPageToggles[_currentMainPageIndex].isOn = true;
        }

        // Hide recipe page and back button
        if (_recipePage != null)
        {
            _recipePage.ClearRecipe();
        }

        SetBackButtonActive(false);
    }

    private void SetBackButtonActive(bool isActive)
    {
        if (_backBtn != null)
        {
            _backBtn.SetActive(isActive);
        }
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

        SetBackButtonActive(false);
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
        SetBackButtonActive(true);
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
        SetBackButtonActive(true);
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
