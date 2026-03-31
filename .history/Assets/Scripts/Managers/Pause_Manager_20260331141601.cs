using Unity.VisualScripting;
using UnityEngine;

public class Pause_Manager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject[] OtherUIElements;
    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        foreach (GameObject element in OtherUIElements)
        {
            element.SetActive(false);
        }
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        foreach (GameObject element in OtherUIElements)
        {
            element.SetActive(true);
        }
    }
}
