using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer_UI : MonoBehaviour
{
    private float _remainingTime;

    [SerializeField] private float _timerMax;
    [SerializeField] private Image timerImage;

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
