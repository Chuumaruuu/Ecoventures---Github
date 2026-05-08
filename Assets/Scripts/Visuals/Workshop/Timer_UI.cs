using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer_UI : MonoBehaviour
{
    private float _remainingTime;
    private bool _hasTimerEnded;

    [SerializeField] private float _timerMax;
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI timerTxt;
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

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            _mapSceneIndex = 1;
        }

        _remainingTime = _timerMax;
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
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
        if (_sceneManager == null)
        {
            _sceneManager = FindFirstObjectByType<Scene_Manager>();
        }

        if (_sceneManager == null)
        {
            Debug.LogError("Scene_Manager reference is missing. Cannot transition to map scene.");
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

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (timerTxt == null)
        {
            return;
        }

        int minutes = Mathf.FloorToInt(_remainingTime / 60f);
        int seconds = Mathf.FloorToInt(_remainingTime % 60f);
        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
