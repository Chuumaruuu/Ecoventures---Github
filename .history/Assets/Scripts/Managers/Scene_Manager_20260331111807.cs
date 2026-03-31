using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] private Animator _fadeAnimator;

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
        SceneManager.LoadScene(_levelToLoad);
    }

}
