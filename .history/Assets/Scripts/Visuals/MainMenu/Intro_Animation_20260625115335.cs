using UnityEngine;
using UnityEngine.Video;

public class Intro_Animation : MonoBehaviour
{
    private Animator _cutsceneAnimator;
    [SerializeField]private GameObject _videoCanvas;
    [SerializeField] private VideoPlayer _videoPlayer;


    private void Start()
    {
        _cutsceneAnimator = GetComponent<Animator>();
        _videoPlayer.loopPointReached += FadeOut;
    }

    private void FadeOut(VideoPlayer vp)
    {
        Debug.Log("Before Fade");
        _cutsceneAnimator.SetTrigger("Fade");
        Debug.Log("Fade");
        _videoPlayer.loopPointReached -= FadeOut;
        Debug.Log("After Fade");
        HideIntroVideoCanvas();
    }

    public void HideIntroVideoCanvas()
    {
        Debug.Log("Before Hide");
        _videoCanvas.SetActive(false);
        Debug.Log("After Hide");
    }
}
