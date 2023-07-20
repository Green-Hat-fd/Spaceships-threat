using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CustomTimer : MonoBehaviour
{
    private float _maxTime;
    private float _timeElapsed;
    #region UNUSED
    [System.Obsolete("This UnityEvent is not used anymore, use the function OnTimerDone() instead", true)]
    public UnityEvent OnTimerDone_event; 
    #endregion

    public
    float maxTime
    {
        get => _maxTime;
        set { _maxTime = value; }
    }

    public float timeElapsed
    {
        get => _timeElapsed;
    }

    /// <summary>
    /// This function adds Time.deltaTime (of Unity) to the timer,  
    /// <br></br>when it's over, it calls OnTimerDone and resets the timer
    /// </summary>
    public void AddTimeToTimer()
    {
        if (_timeElapsed >= maxTime)
        {
            OnTimerDone();

            _timeElapsed = 0;     //Reset the timer
        }
        else
        {
            _timeElapsed += Time.deltaTime;   //Increases the elapsed time
        }
    }

    /// <summary>
    /// This function adds <b><i>timeToAdd</i></b> to the timer,
    /// <br></br>when it's over, it calls OnTimerDone and resets the timer
    /// </summary>
    /// <param name="timeToAdd">Other time to add</param>
    public void AddTimeToTimer(float timeToAdd)
    {
        if (_timeElapsed >= maxTime)
        {
            OnTimerDone();

            _timeElapsed = 0;     //Reset the timer
        }
        else
        {
            _timeElapsed += timeToAdd;   //Increases the elapsed time
        }
    }

    /// <summary>
    /// This function will be executed when the timer is done
    /// </summary>
    public abstract void OnTimerDone();
}
