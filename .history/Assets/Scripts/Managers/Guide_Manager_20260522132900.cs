using UnityEngine;

public class Guide_Manager : MonoBehaviour
{
    private int _currentGuideIndex = 0;
    [SerializeField] private GameObject[] _pages;
    private void setGuideIndex(int index) 
    {
        _currentGuideIndex = index;
    }
}
