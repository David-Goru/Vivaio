using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    // Master things
    public static string TimeState;
    public float MinuteValue; // Real seconds for in game minute
    public static bool OnIndoors;
    public static bool Sleeping;
    public static int CurrentMinute;

    // Time system things    
    public GameObject HouseSprite;
    public GameObject ClockHand;
    public static GameObject Night;
    public GameObject Player;
    public GameObject Bed;

    public Sprite ClockMorning;
    public Sprite ClockAfternoon;
    public Sprite ClockNight;

    // Sprites
    Sprite houseDay;
    Sprite houseNight;

    void Start()
    {
        OnIndoors = false;
        Sleeping = false;
        Night = GameObject.Find("Night");
        Night.SetActive(false);
        houseDay = Resources.Load<Sprite>("House/Indoor house day");
        houseNight = Resources.Load<Sprite>("House/Indoor house night");

        TimeState = "Morning";
        CurrentMinute = 180;

        Time.timeScale = MinuteValue;

        StartCoroutine(TimeTick());
    }

    public void StartSleeping()
    {
        StopAllCoroutines();

        Time.timeScale = 40;
        (Player.GetComponent("PlayerMovement") as PlayerMovement).enabled = false;
        Player.GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("UI").transform.Find("Bed").gameObject.SetActive(false);
        Bed.GetComponent<Animator>().SetTrigger("Sleep");
        Sleeping = true;

        StartCoroutine(TimeTick());
    }

    IEnumerator TimeTick()
    {
        if (Sleeping)
        {
            yield return new WaitForSeconds(0.25f);
            ClockHand.transform.eulerAngles = new Vector3(0, 0, -1f * CurrentMinute);
            CurrentMinute++;

            switch (CurrentMinute)
            {
                case 720:
                    CurrentMinute = 0;
                    NewDayCall();
                    break;
                case 180:
                    Bed.GetComponent<Animator>().SetTrigger("StopSleep");
                    (Player.GetComponent("PlayerMovement") as PlayerMovement).enabled = true;
                    Player.GetComponent<SpriteRenderer>().enabled = true;
                    Sleeping = false;
                    Time.timeScale = MinuteValue;
                    TimeState = "Morning";
                    ClockHand.transform.parent.gameObject.GetComponent<Image>().sprite = ClockMorning;
                    HouseSprite.GetComponent<SpriteRenderer>().sprite = houseDay;
                    break;
                case 270:
                    TimeState = "Shop open";
                    ClockHand.transform.parent.gameObject.GetComponent<Image>().sprite = ClockAfternoon;
                    break;
                case 630:
                    TimeState = "Night";
                    ClockHand.transform.parent.gameObject.GetComponent<Image>().sprite = ClockNight;
                    HouseSprite.GetComponent<SpriteRenderer>().sprite = houseNight;
                    break;
            }

            StartCoroutine(TimeTick());
        }
        else if (CurrentMinute < 720)
        {
            yield return new WaitForSeconds(1);
            ClockHand.transform.eulerAngles = new Vector3(0, 0, -1f * CurrentMinute);
            CurrentMinute++;

            StartCoroutine(TimeTick());
        }
    }

    public void NewDayCall()
    {
        Master.Day++;
        Master.LastDayPaid = 0;
        Master.LastDayPaid += WaterSource.WaterUsage;
        Master.LastDayWaterUsage = WaterSource.WaterUsage;
        WaterSource.WaterUsage = 0;
        Master.LastDayPaid += 1;
        if (Master.Debt > Master.DailyDebt)
        { 
            if (Master.DailyDebt + Master.LastDayPaid < Master.Balance)
            {
                Master.Debt -= Master.DailyDebt;
                Master.LastDayDebt = Master.DailyDebt;
                Master.LastDayPaid += Master.DailyDebt;
            }
            else Master.LastDayDebt = 0;
        }
        else if (Master.Debt > 0)
        {
            if (Master.Debt + Master.LastDayPaid < Master.Balance)
            {
                Master.LastDayDebt = Master.Debt;
                Master.LastDayPaid += Master.Debt;
                Master.Debt = 0;
            }
            else Master.LastDayDebt = 0;
        }
        else Master.LastDayDebt = 0;
        Master.UpdateBalance(-Master.LastDayPaid);

        (gameObject.GetComponent("AI") as AI).NewDay();
        GameObject.Find("Structure").transform.Find("Entrance").Find("Mailbox").gameObject.GetComponent<PostBox>().UpdateLetters();

        foreach (Crop crop in Farm.Crops.ToArray())
        {
            crop.NewDay();
        }
        (GameObject.Find("Farm handler").GetComponent("DeliverySystem") as DeliverySystem).UpdatePackages();
    }
}