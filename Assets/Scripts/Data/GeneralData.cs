using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneralData
{ 
    [SerializeField]
    public int Balance;
    [SerializeField]
    public int Day;
    [SerializeField]
    public int Debt;
    [SerializeField]
    public int LastDayDebt;
    [SerializeField]
    public int DailyDebt;
    [SerializeField]
    public int LastDayPaid;
    [SerializeField]
    public int LastDayWaterUsage;
    [SerializeField]
    public int LastDayEnergyUsage;
    [SerializeField]
    public List<Stand> Stands;

    public GeneralData(int balance, int day, int debt, int lastDayDebt, int dailyDebt, int lastDayPaid, int lastDayWaterUsage, int lastDayEnergyUsage)
    {
        Balance = balance;
        Day = day;
        Debt = debt;
        LastDayDebt = lastDayDebt;
        DailyDebt = dailyDebt;
        LastDayPaid = lastDayPaid;
        LastDayWaterUsage = lastDayWaterUsage;
        LastDayEnergyUsage = lastDayEnergyUsage;
        Stands = new List<Stand>();
    }
}