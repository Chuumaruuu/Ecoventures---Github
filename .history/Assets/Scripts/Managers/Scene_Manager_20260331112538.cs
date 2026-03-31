using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
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
    [SerializeField] private Animator _fadeAnimator;
    public event System.Action OnSceneFadeComplete;

    private int _levelToLoad;
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
