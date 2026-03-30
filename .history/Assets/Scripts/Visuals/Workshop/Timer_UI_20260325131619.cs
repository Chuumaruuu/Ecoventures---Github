using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer_UI : MonoBehaviour
{
    private float _remainingTime;

    [SerializeField] private float _timerMax;
    [SerializeField] private Image timerImage;
    [SerializeField, Range(0f, 0.5f)] private float transitionWidth = 0.1f;

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
        if (_timerMax <= 0f)
        {
            return;
        }

        if (_remainingTime > 0f)
        {
            _remainingTime -= Time.deltaTime;
            if (_remainingTime < 0f)
            {
                _remainingTime = 0f;
            }
        }

        UpdateVisuals();
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

        timerImage.color = GetSmoothTimerColor(timeRatio);
    }

    private Color GetSmoothTimerColor(float timeRatio)
    {
        float upperThreshold = 2f / 3f;
        float lowerThreshold = 1f / 3f;
        float halfWindow = transitionWidth * 0.5f;

        // Blend near each threshold to avoid abrupt color changes.
        if (timeRatio >= upperThreshold + halfWindow)
        {
            return Color.green;
        }

        if (timeRatio > upperThreshold - halfWindow)
        {
            float t = Mathf.InverseLerp(upperThreshold - halfWindow, upperThreshold + halfWindow, timeRatio);
            return Color.Lerp(Color.yellow, Color.green, t);
        }

        if (timeRatio >= lowerThreshold + halfWindow)
        {
            return Color.yellow;
        }

        if (timeRatio > lowerThreshold - halfWindow)
        {
            float t = Mathf.InverseLerp(lowerThreshold - halfWindow, lowerThreshold + halfWindow, timeRatio);
            return Color.Lerp(Color.red, Color.yellow, t);
        }

        return Color.red;
    }
}
