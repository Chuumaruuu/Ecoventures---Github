using Unity.VisualScripting;
using UnityEngine;

public class Pause_Manager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject UIs;
    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        UIs.SetActive(false);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        UIs.SetActive(true);
    }
}
