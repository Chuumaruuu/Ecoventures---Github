using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndingCutscene : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Cutscene finished!");
        // Do whatever you need next, e.g.:
        // SceneManager.LoadScene("MainMenu");
        // Or show credits, fade out, etc.
    }

    void OnDestroy()
    {
        // Avoid lingering event subscriptions
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}