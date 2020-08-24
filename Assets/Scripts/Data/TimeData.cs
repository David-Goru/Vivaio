using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TimeData
{ 
    [SerializeField]    
    public bool OnIndoors;
    [SerializeField]
    public bool Sleeping;
    [SerializeField]
    public TimeState TimeState;
    [SerializeField]
    public int CurrentMinute;
    [SerializeField]
    public int TimeSpeed;
    [SerializeField]
    public List<Timer> Timers;
    [SerializeField]
    public Vector2 SleepPosition;

    public TimeData(bool onIndoors, bool sleeping, TimeState timeState, int currentMinute, int timeSpeed, List<Timer> timers)
    {
        OnIndoors = onIndoors;
        Sleeping = sleeping;
        TimeState = timeState;
        CurrentMinute = currentMinute;
        TimeSpeed = timeSpeed;
        Timers = timers;
    }
}