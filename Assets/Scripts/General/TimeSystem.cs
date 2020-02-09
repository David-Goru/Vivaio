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
        Night = GameObject.Find("Night");
        Night.SetActive(false);
        houseDay = Resources.Load<Sprite>("House/Indoor house day");
        houseNight = Resources.Load<Sprite>("House/Indoor house night");

        TimeState = "Morning";

        StartCoroutine(StateHandler());
    }

    IEnumerator StateHandler()
    {
        Night.SetActive(false);
        ClockHand.transform.parent.gameObject.GetComponent<Image>().sprite = ClockMorning;

        for (int i = 0; i < 90; i++)
        {
            yield return new WaitForSeconds(MinuteValue);
            ClockHand.transform.Rotate(0, 0, -1);
        }

        // Open shop
        TimeState = "Shop open";
        ClockHand.transform.parent.gameObject.GetComponent<Image>().sprite = ClockAfternoon;

        for (int i = 0; i < 360; i++)
        {
            yield return new WaitForSeconds(MinuteValue);
            ClockHand.transform.Rotate(0, 0, -1);
        }

        // Close shop, start sunset and whatever
        TimeState = "Night"; // Night lasts until player decides to sleep
        ClockHand.transform.parent.gameObject.GetComponent<Image>().sprite = ClockNight;
        if (!OnIndoors) Night.SetActive(true);
        else if (Vector2.Distance(Player.transform.position, Bed.transform.position) < 1) GameObject.Find("UI").transform.Find("Bed").gameObject.SetActive(true);
        HouseSprite.GetComponent<SpriteRenderer>().sprite = houseNight;
    }

    public void NewDayCall()
    {
        StartCoroutine(goNight());
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
    }

    IEnumerator goNight()
    {
        TimeState = "Sleeping";
        Bed.GetComponent<Animator>().SetTrigger("Sleep");
        (Player.GetComponent("PlayerMovement") as PlayerMovement).enabled = false;
        Player.GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("UI").transform.Find("Bed").gameObject.SetActive(false);

        Color colorBasement = HouseSprite.GetComponent<SpriteRenderer>().color;

        while (colorBasement.a > 0.2)
        {
            colorBasement = new Color(colorBasement.r, colorBasement.g, colorBasement.b, colorBasement.a - 0.1f);
            HouseSprite.GetComponent<SpriteRenderer>().color = colorBasement;
            Bed.GetComponent<SpriteRenderer>().color = colorBasement;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(goDay());
    }

    IEnumerator goDay()
    {
        ClockHand.transform.rotation = Quaternion.Euler(0, 0, -180);
        TimeState = "Morning";
        HouseSprite.GetComponent<SpriteRenderer>().sprite = houseDay;
        foreach (Crop crop in Farm.Crops.ToArray())
        {
            crop.NewDay();
        }
        (gameObject.GetComponent("AI") as AI).NewDay();

        (GameObject.Find("Farm handler").GetComponent("DeliverySystem") as DeliverySystem).UpdatePackages();

        Color colorBasement = HouseSprite.GetComponent<SpriteRenderer>().color;

        while (colorBasement.a < 1)
        {
            colorBasement = new Color(colorBasement.r, colorBasement.g, colorBasement.b, colorBasement.a + 0.1f);
            HouseSprite.GetComponent<SpriteRenderer>().color = colorBasement;
            Bed.GetComponent<SpriteRenderer>().color = colorBasement;
            yield return new WaitForSeconds(0.05f);
        }
        Bed.GetComponent<Animator>().SetTrigger("StopSleep");
        (Player.GetComponent("PlayerMovement") as PlayerMovement).enabled = true;
        Player.GetComponent<SpriteRenderer>().enabled = true;

        StartCoroutine(StateHandler());
    }
}