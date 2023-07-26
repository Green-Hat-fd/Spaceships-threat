using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomTimer
{
    private float _maxTime;
    private float _timeElapsed;

    private bool _isActive = true;   //Used to pause/resume the timer
    private bool _isOver = false;    //Changed when the timer will be over
    
    public UnityEvent OnTimerDone_event = new UnityEvent();


    #region // Constructors \\

    public CustomTimer() { }

    public CustomTimer(float newMaxTime)
    {
        maxTime = newMaxTime;
    }

    #endregion


    /// <summary>
    /// The max time to wait in seconds
    /// </summary>
    public float maxTime
    {
        get => _maxTime;
        set { _maxTime = value; }
    }

    public float timeElapsed
    {
        get => _timeElapsed;
    }


    /// <summary>
    /// This function adds <i>Time.deltaTime</i> (of Unity) to the timer,  
    /// <br></br>when it's over, it calls OnTimerDone, resets the timer and deactivates it
    /// </summary>
    public void AddTimeToTimer()
    {
        if (_timeElapsed >= maxTime)
        {
            OnTimerDone_event.Invoke();

            _timeElapsed = 0;     //Reset the timer
            _isActive = false;    //Block the timer
            _isOver = true;
        }
        else
        {
            if(_isActive)
                _timeElapsed += Time.deltaTime;   //Increases the elapsed time
        }
    }
    /// <summary>
    /// This function adds <b><i>timeToAdd</i></b> to the timer,
    /// <br></br>when it's over, it calls OnTimerDone and resets the timer and deactivates it
    /// </summary>
    /// <param name="timeToAdd">Other time to add</param>
    public void AddTimeToTimer(float timeToAdd)
    {
        if (_timeElapsed >= maxTime)
        {
            OnTimerDone_event.Invoke();

            _timeElapsed = 0;     //Reset the timer
            _isActive = false;    //Block the timer
            _isOver = true;
        }
        else
        {
            if(_isActive)
                _timeElapsed += timeToAdd;   //Increases the elapsed time
        }
    }

    /// <summary>
    /// Restart the timer from the start and activates it
    /// </summary>
    public void Restart()
    {
        _timeElapsed = 0;     //Reset the timer
        _isActive = true;     //Activates the timer
        _isOver = false;
    }

    /// <summary>
    /// Activates the timer if it's not active
    /// </summary>
    public void Activate()
    {
        _isActive = true;
    }
    /// <summary>
    /// De-activates the timer if it's not active
    /// </summary>
    public void Pause()
    {
        _isActive = false;
    }

    /// <summary>
    /// Returns if the timer is over
    /// </summary>
    public bool CheckIsOver() => _isOver;

    /// <summary>
    /// Returns the elapsed time in percentage
    /// </summary>
    public float PercentElapsedTime() => timeElapsed / maxTime;

    #region UNUSED
    /// <summary>
    /// This function will be executed when the timer is done
    /// </summary>
    [System.Obsolete("This UnityEvent is not used anymore, use the function OnTimerDone() instead", true)]
    public /*abstract*/ void OnTimerDone() { }
    #endregion
}
