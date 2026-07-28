using UnityEngine;
using UnityEngine.Video;

public class EndingCutscene : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void OnEnable()
    {
        if (ObjectivesTracker.Instance != null)
        {
            ObjectivesTracker.Instance.OnObjectivesCompleted += PlayCutscene;

            // In case objectives were already completed before this
            // script subscribed (e.g. scene load order), check immediately.
            if (ObjectivesTracker.Instance.AreObjectivesMet())
            {
                PlayCutscene();
            }
        }
    }

    void OnDisable()
    {
        if (ObjectivesTracker.Instance != null)
        {
            ObjectivesTracker.Instance.OnObjectivesCompleted -= PlayCutscene;
        }
    }

    void PlayCutscene()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Cutscene finished!");
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}