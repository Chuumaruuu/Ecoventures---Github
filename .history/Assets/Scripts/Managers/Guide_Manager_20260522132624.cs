using UnityEngine;
using UnityEngine.UI;

public class Guide_Manager : MonoBehaviour
{
    [SerializeField] private Toggle[] _toggles;
    [SerializeField] private GameObject[] _pages;
    [SerializeField] private int _defaultPageIndex = 0;

    private void Awake()
    {
        if (_toggles == null || _toggles.Length == 0)
        {
            _toggles = GetComponentsInChildren<Toggle>(true);
        }
    }

    private void Start()
    {
        for (int i = 0; i < _toggles.Length; i++)
        {
            int pageIndex = i;
            Toggle toggle = _toggles[i];

            if (toggle == null)
            {
                continue;
            }

            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    ShowPage(pageIndex);
                }
            });
        }

        ShowPage(_defaultPageIndex);
    }

    public void ShowPage(int pageIndex)
    {
        if (_pages == null || _pages.Length == 0)
        {
            return;
        }

        if (pageIndex < 0 || pageIndex >= _pages.Length)
        {
            return;
        }

        for (int i = 0; i < _pages.Length; i++)
        {
            if (_pages[i] == null)
            {
                continue;
            }

            _pages[i].SetActive(i == pageIndex);
        }
    }
}
