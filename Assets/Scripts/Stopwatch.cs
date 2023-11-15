using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    public float Duration;

    private float _duration;
    
    private delegate void StopwatchComplete();

    private StopwatchComplete _stopwatchComplete;

    private bool _isRunning;

    public void Play() => _isRunning = true;

    public void Pause() => _isRunning = false;
    public void Reset() => _duration = Duration;

    private void Awake()
    {
        _duration = Duration;
    }

    private void Update()
    {
        if (_isRunning)
        {
            Duration -= Time.deltaTime;
        }
        if (_isRunning && Duration <= 0)
        {
            _stopwatchComplete.Invoke();
        }
    }
}
