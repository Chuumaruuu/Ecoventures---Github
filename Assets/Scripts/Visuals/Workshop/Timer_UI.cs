using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer_UI : MonoBehaviour
{
    private float _remainingTime;
    private bool _hasTimerEnded;
    private bool _timerRunning = false;

    private const int MAP_SCENE_INDEX = 2;

    [SerializeField] private float _timerMax;
    [SerializeField] private Image _timerImage;
    [SerializeField] private TextMeshProUGUI _timerTxt;
    [SerializeField] private Scene_Manager _sceneManager;
        

    public enum TimerState
    {
        Green,
        Yellow,
        Red
    }

    private void Start()
    {
        _remainingTime = _timerMax;
        _timerRunning = true;
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_timerRunning)
        {
            return;
        }
        else
        {
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
        
    }

    public void TimerSwitch(int value)
    {
        if (value == 1)
        {
            _timerRunning = true;
        }
        else if (value == 0)
        {
            _timerRunning = false;
        }
        else
        {
            Debug.LogError("Timer switch value not set. Set to 1 or 0");
        }
    }

    private void OnTimerEnded()
    {
        if (_hasTimerEnded)
        {
            return;
        }

        _hasTimerEnded = true;
        TransitionToMapScene();
    }

    private void TransitionToMapScene()
    {
        _sceneManager.FadeToScene(MAP_SCENE_INDEX);
    }

    private void UpdateVisuals()
    {
        if (_timerImage == null)
        {
            return;
        }

        float timeRatio = Mathf.Clamp01(_remainingTime / _timerMax);

        // For radial fill images, this drives the visible portion from full (1) to empty (0).
        _timerImage.fillAmount = timeRatio;

        if (timeRatio > (2f / 3f))
        {
            _timerImage.color = Color.green;
        }
        else if (timeRatio > (1f / 3f))
        {
            _timerImage.color = Color.yellow;
        }
        else
        {
            _timerImage.color = Color.red;
        }

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (_timerTxt == null)
        {
            return;
        }

        int minutes = Mathf.FloorToInt(_remainingTime / 60f);
        int seconds = Mathf.FloorToInt(_remainingTime % 60f);
        _timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
