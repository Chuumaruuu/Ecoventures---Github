using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance {get; private set;}
    [SerializeField]private bool DEBUG_MODE;
    
    public event Action<string> OnAchievement;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public bool DebugModeOn()
    {
        return DEBUG_MODE == true; 
    }

    public void AchievementAccomplished(string data)
    {
        OnAchievement?.Invoke(data);
    }

}
