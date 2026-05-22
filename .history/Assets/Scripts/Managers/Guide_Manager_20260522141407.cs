using UnityEngine;

public class Guide_Manager : MonoBehaviour
{
    [SerializeField] private GameObject[] _pages;

    public void setCurrentPage(int index) 
    {
        if (index < 0 || index >= _pages.Length)
        {
            Debug.LogWarning("Index out of range: " + index);
            return;
        }

        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].SetActive(i == index);
        }
    }
}
