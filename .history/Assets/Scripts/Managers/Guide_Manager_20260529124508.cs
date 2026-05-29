using UnityEngine;

public class Guide_Manager : MonoBehaviour
{
    [System.Serializable]
    private class MainPageGroup
    {
        public GameObject mainPage;
        public GameObject[] recipePages;
    }

    [SerializeField] private MainPageGroup[] _mainPages;
    private int _currentMainPageIndex = -1;
    private int _currentRecipePageIndex = -1;

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

            SetRecipePagesActive(_mainPages[i].recipePages, false);
        }

        _currentMainPageIndex = index;
        _currentRecipePageIndex = -1;

        if (_mainPages[index].recipePages != null && _mainPages[index].recipePages.Length > 0)
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

        GameObject[] recipePages = _mainPages[_currentMainPageIndex].recipePages;

        if (recipePages == null || recipePages.Length == 0)
        {
            Debug.LogWarning("Current main page has no recipe pages assigned.");
            return;
        }

        if (index < 0 || index >= recipePages.Length)
        {
            Debug.LogWarning("Recipe index out of range: " + index);
            return;
        }

        SetRecipePagesActive(recipePages, false);
        recipePages[index].SetActive(true);
        _currentRecipePageIndex = index;
    }

    private void SetRecipePagesActive(GameObject[] recipePages, bool isActive)
    {
        if (recipePages == null)
        {
            return;
        }

        for (int i = 0; i < recipePages.Length; i++)
        {
            if (recipePages[i] != null)
            {
                recipePages[i].SetActive(isActive);
            }
        }
    }
}
