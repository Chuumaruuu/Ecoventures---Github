using UnityEngine;
using UnityEngine.UI;

public class Timer_UI : MonoBehaviour
{
    private float _remainingTime;
    private bool _hasTimerEnded;
    private bool _awaitingMapSceneInput;
    private bool _hasStartedSceneTransition;

    [SerializeField] private float _timerMax;
    [SerializeField] private Image timerImage;
    [SerializeField] private Scene_Manager _sceneManager;
    [SerializeField] private int _mapSceneIndex = 2;

    public enum TimerState
    {
        Green,
        Yellow,
        Red
    }

    private void Start()
    {
        if (timerImage == null)
        {
            timerImage = GetComponent<Image>();
        }

        _remainingTime = _timerMax;
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        if (_awaitingMapSceneInput)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                TransitionToMapScene();
            }

            return;
        }

        if (_timerMax <= 0f)
        {
            return;
        }

        if (!_hasTimerEnded && _remainingTime > 0f)
        {
            _remainingTime -= Time.deltaTime;
            if (_remainingTime < 0f)
            {
                _remainingTime = 0f;
            }

            if (_remainingTime <= 0f)
            {
                OnTimerEnded();
            }
        }

        UpdateVisuals();
    }

    private void OnTimerEnded()
    {
        if (_hasTimerEnded)
        {
            return;
        }

        _hasTimerEnded = true;
        ShowMapTransitionNotification();
    }

    private void ShowMapTransitionNotification()
    {
        _awaitingMapSceneInput = true;
        Debug.Log("Timer ended. Press Enter to proceed to the map scene.");
    }

    private void TransitionToMapScene()
    {
        if (_hasStartedSceneTransition)
        {
            return;
        }

        _hasStartedSceneTransition = true;
        _awaitingMapSceneInput = false;

        if (_sceneManager == null)
        {
            _sceneManager = FindObjectOfType<Scene_Manager>();
        }

        if (_sceneManager == null)
        {
            Debug.LogError("Scene_Manager reference is missing. Cannot transition to map scene.");
            _hasStartedSceneTransition = false;
            _awaitingMapSceneInput = true;
            return;
        }

        _sceneManager.FadeToScene(_mapSceneIndex);
    }

    private void UpdateVisuals()
    {
        if (timerImage == null)
        {
            return;
        }

        float timeRatio = Mathf.Clamp01(_remainingTime / _timerMax);

        // For radial fill images, this drives the visible portion from full (1) to empty (0).
        timerImage.fillAmount = timeRatio;

        if (timeRatio > (2f / 3f))
        {
            timerImage.color = Color.green;
        }
        else if (timeRatio > (1f / 3f))
        {
            timerImage.color = Color.yellow;
        }
        else
        {
            timerImage.color = Color.red;
        }
    }
}
