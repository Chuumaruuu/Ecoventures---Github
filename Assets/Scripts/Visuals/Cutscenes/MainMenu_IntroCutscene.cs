using UnityEngine;
using UnityEngine.Video;

public class MainMenu_IntroCutscene : MonoBehaviour
{
    
    private Animator _cutsceneAnimator;
    [SerializeField] private GameObject _cutsceneCanvas;
    [SerializeField] private VideoPlayer _cutsceneVideoPlayer;
    void Start()
    {
        _cutsceneAnimator = GetComponent<Animator>();
        _cutsceneVideoPlayer.loopPointReached += FadeScreen;
    }

    private void FadeScreen(VideoPlayer _videoPlayer)
    {
        _cutsceneAnimator.SetTrigger("Fade");
    }

    public void HideIntroVideoCanvas()
    {
        _cutsceneCanvas.GetComponent<Canvas>().sortingOrder = 0;
        _cutsceneCanvas.SetActive(false);
    }

    public void SkipCutscene()
    {
        // Prevent loopPointReached from firing after stopping.
        _cutsceneVideoPlayer.loopPointReached -= FadeScreen;

        if (_cutsceneVideoPlayer.isPlaying)
        {
            _cutsceneVideoPlayer.Stop();
        }

        _cutsceneAnimator.SetTrigger("Fade");
    }
}
