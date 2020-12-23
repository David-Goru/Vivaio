using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneralData
{ 
    [SerializeField]
    public string PlayerName;
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
    public bool ShopInaugurated;
    [SerializeField]
    public List<int> ShopEarnings;
    [SerializeField]
    public List<Stand> Stands;
    [SerializeField]
    public List<CashRegister> CashRegisters;
    [SerializeField]
    public List<string> Log;

    public GeneralData(string playerName, int balance, int day, int debt, int lastDayDebt, int dailyDebt, int lastDayPaid, int lastDayWaterUsage, int lastDayEnergyUsage, bool shopInaugurated)
    {
        PlayerName = playerName;
        Balance = balance;
        Day = day;
        Debt = debt;
        LastDayDebt = lastDayDebt;
        DailyDebt = dailyDebt;
        LastDayPaid = lastDayPaid;
        LastDayWaterUsage = lastDayWaterUsage;
        LastDayEnergyUsage = lastDayEnergyUsage;
        ShopInaugurated = shopInaugurated;
        ShopEarnings = new List<int>();
        Stands = new List<Stand>();
        CashRegisters = new List<CashRegister>();
        Log = new List<string>();
    }
}