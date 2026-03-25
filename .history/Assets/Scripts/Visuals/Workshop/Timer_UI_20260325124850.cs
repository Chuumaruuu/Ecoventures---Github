using System;
using UnityEngine;

public class Timer_UI : MonoBehaviour
{
    private float _remainingTime;

    [SerializeField] private float _timerMax;
    [SerializeField] private SpriteRenderer timerCircle;

    private void Start()
    {
        if (timerCircle == null)
        {
            timerCircle = GetComponent<SpriteRenderer>();
        }

        _remainingTime = _timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (_remainingTime > 0f)
        {
            _remainingTime -= Time.deltaTime;
        }

        _remainingTime = Mathf.Max(_remainingTime, 0f);

        if (timerCircle == null || _timerMax <= 0f)
        {
            return;
        }

        float remainingRatio = _remainingTime / _timerMax;

        if (remainingRatio > (2f / 3f))
        {
            timerCircle.color = Color.green;
        }
        else if (remainingRatio < (1f / 3f))
        {
            timerCircle.color = Color.red;
        }
        else
        {
            timerCircle.color = Color.yellow;
        }
    }
}
