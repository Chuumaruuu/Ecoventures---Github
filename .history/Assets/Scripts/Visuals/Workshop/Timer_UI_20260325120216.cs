using System;
using UnityEngine;

public class Timer_UI : MonoBehaviour
{
    private float _remainingTime;
    private TimerState _currentTimeState;

    [SerializeField] private float _timerMax;

    SpriteRenderer timer;
    public enum TimerState
    {
        Green,
        Yellow,
        Red
    }

    private void Start()
    {
        _currentTimeState = TimerState.Green;
        timer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        switch(_currentTimeState)
        {
            case TimerState.Green:
            timer.color = Color.green;
            if(_remainingTime > ((2/3)*_timerMax))
                {
                    _remainingTime -= Time.deltaTime;
                }
            else
                {
                    _currentTimeState = TimerState.Yellow;
                }
            break;
            case TimerState.Yellow:
            timer.color = Color.yellow;
            if(_remainingTime > ((1/3)*_timerMax))
                {
                    _remainingTime -= Time.deltaTime;
                }
            else
                {
                    _currentTimeState = TimerState.Red;
                }

            break;
            case TimerState.Red:
            timer.color = Color.red;

            break;
        }
    }
}
