// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;

public class Timer
{
    private float _timer;
    private float _secondsLimit;

    /// <summary>
    /// Tick each timerSeconds
    /// </summary>
    public Timer( float timerSeconds ) 
    { 
        _timer = timerSeconds;
        _secondsLimit = timerSeconds;
    }

    public void Restart() => _timer = _secondsLimit;
    public void Reset( float timerSeconds )
    {
        _secondsLimit = timerSeconds;
        Restart();
    }

    /// <summary>
    /// Only tick true once the seconds selected in constructors, needs manual time restart
    /// </summary>
    public bool HasTickOnce()
    {
        if ( _timer < 0 ) 
            return true;

        _timer -= Time.deltaTime;
        return _timer < 0;
    }

    /// <summary>
    /// Tick true each time the selected seconds in constructors, auto restart when tick
    /// </summary>
    public bool HasTickForever()
    {
        _timer -= Time.deltaTime;
        if ( _timer > 0 )
            return false;

        _timer += _secondsLimit;
        return true;
    }

    public float SecondsLeft() => _timer;
}
