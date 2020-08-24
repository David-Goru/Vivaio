using System;
using UnityEngine;

[Serializable]
public class Timer
{
    [SerializeField]    
    OnTimeReached function;
    [SerializeField] 
    public int Time;

    public Timer(OnTimeReached function, int time)
    {
        this.function = function;
        Time = time;
    }

    public void Cycle()
    {
        Time -= 2 * TimeSystem.Data.TimeSpeed;
        if (Time <= 0)
        {
            Time = 0;
            function();
            TimeSystem.Data.Timers.Remove(this);
        }
    }
}