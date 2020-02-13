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
        Balance = 0;
        Day = 0;
        Debt = 3000;
        DailyDebt = 100;
        LastDayDebt = 0;
        LastDayPaid = 0;
        LastDayWaterUsage = 0;

        Application.targetFrameRate = 60;
        UpdateBalance(2000);
    }

    public static void UpdateBalance(int money)
    {
        Balance += money;
        GameObject.Find("UI").transform.Find("Money").transform.Find("Text").gameObject.GetComponent<Text>().text = Balance + "€";

        Transform MoneyHandler = GameObject.Find("UI").transform.Find("Money").Find("Money updates").Find("Viewport").Find("Content");

        GameObject uiElement = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Money " + (money < 0 ? "removed" : "added")), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        uiElement.GetComponent<Text>().text = money + "€";
        uiElement.transform.SetParent(MoneyHandler);
    }
}