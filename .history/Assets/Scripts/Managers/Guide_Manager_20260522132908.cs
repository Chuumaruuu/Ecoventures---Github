using UnityEngine;

public class Guide_Manager : MonoBehaviour
{
    private int _currentGuideIndex = 0;
    [SerializeField] private GameObject[] _pages;
    private void setGuideIndex(int index) 
    {
        _currentGuideIndex = index;
    }

    public void NextPage() 
    {
        if (_currentGuideIndex < _pages.Length - 1)
        {
            _pages[_currentGuideIndex].SetActive(false);
            _currentGuideIndex++;
            _pages[_currentGuideIndex].SetActive(true);
        }
    }
}
