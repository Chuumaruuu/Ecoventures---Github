using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] private Animator _fadeAnimator;
    public event Action OnSceneFadeComplete;
    private int _levelToLoad;
    public static Scene_Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void Start()
    {
        _fadeAnimator.SetTrigger("Fade");
    }

    public void FadeToScene(int _sceneIndex)
    {
        _fadeAnimator.SetTrigger("Fade");
        _levelToLoad = _sceneIndex;
    }

    public void OnFadeComplete()
    {
        OnSceneFadeComplete?.Invoke();
        SceneManager.LoadScene(_levelToLoad);
    }

}
