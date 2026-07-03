using Unity.VisualScripting;
using UnityEngine;

public class Pause_Manager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _guideUI;
    [SerializeField] private GameObject _moneyUI;
    [SerializeField] private Guide_Manager _guideManager;
    public void Pause()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
        _gameUI.SetActive(false);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
        _gameUI.SetActive(true);
        _guideUI.SetActive(false);
        _moneyUI.SetActive(true);
    }

    public void OpenGuide()
    {
        Time.timeScale = 0f;
        _guideUI.SetActive(true);
        _pauseMenu.SetActive(false);
        _gameUI.SetActive(false);
        _moneyUI.SetActive(false);
        _guideManager.ResetToFirstPage();
    }
}
