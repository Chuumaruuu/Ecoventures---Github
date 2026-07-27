using System;
using UnityEngine;

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
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    
    public void ResumeGame()
    {
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
