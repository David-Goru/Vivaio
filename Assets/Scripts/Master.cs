using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviour
{
    public static int Balance;
    public static int Day;
    public static int Debt;
    public static int LastDayDebt;
    public static int DailyDebt;
    public static int LastDayPaid;
    public static int LastDayWaterUsage;

    void Start()
    {
        Balance = 2000;
        Day = 0;
        Debt = 3000;
        DailyDebt = 100;
        LastDayDebt = 0;
        LastDayPaid = 0;
        LastDayWaterUsage = 0;

        Application.targetFrameRate = 60;
    }

    public static void UpdateBalance(int money)
    {
        Balance += money;
        GameObject.Find("UI").transform.Find("Money").transform.Find("Text").gameObject.GetComponent<Text>().text = Balance + "€";
    }
}