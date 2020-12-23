using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public static TimeData Data;

    // Game objects
    public static GameObject Background;
    public static GameObject House;
    public static GameObject Bed;

    // Sprites
    public Sprite HouseDay;
    public Sprite HouseNoon;
    public Sprite HouseNight;
    public Sprite ClockMorning;
    public Sprite ClockDay;
    public Sprite ClockEvening;
    public Sprite ClockNight;

    // Colors
    public Color EarlyBackground;
    public Color MorningBackground;
    public Color ShopOpenBackground;
    public Color EveningBackground;
    public Color NightBackground;

    // When loading a game
    public static bool Load(TimeData data)
    {
        try
        {
            Data = data;
            
            Background = GameObject.Find("Background");
            House = GameObject.FindGameObjectWithTag("House");

            Background.GetComponent<SpriteRenderer>().color = GameObject.Find("Farm handler").GetComponent<TimeSystem>().EarlyBackground;
            UI.Elements["Clock"].GetComponent<Image>().sprite = GameObject.Find("Farm handler").GetComponent<TimeSystem>().ClockNight;
            House.GetComponent<SpriteRenderer>().sprite = GameObject.Find("Farm handler").GetComponent<TimeSystem>().HouseNight;
            UI.Elements["Day display"].GetComponent<Text>().text = string.Format(Localization.Translations["Day"], Master.Data.Day);

            Master.Player.transform.position = Data.SleepPosition;

            MusicHandler.StartTransition(SongType.Night);
        }
        catch (Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "TimeSystem", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new TimeData(false, false, TimeState.MORNING, 180, 1, new List<Timer>());        
            
        Background = GameObject.Find("Background");
        House = GameObject.FindGameObjectWithTag("House");
        UI.Elements["Day display"].GetComponent<Text>().text = string.Format(Localization.Translations["Day"], Master.Data.Day);

        MusicHandler.StartTransition(SongType.Morning);

        return true;
    }

    public void GoToBed()
    {
        Data.SleepPosition = Master.Player.transform.position;
        Data.Sleeping = true;
        Data.TimeSpeed = 200;
        Sleep();
    }

    public static void Sleep()
    {
        (Master.Player.GetComponent("PlayerMovement") as PlayerMovement).enabled = false;
        Master.Player.GetComponent<SpriteRenderer>().enabled = false;
        Master.Player.transform.Find("Character shadow").gameObject.SetActive(false);
        UI.Elements["Bed"].SetActive(false);
        Bed.transform.Find("Sprite").GetComponent<Animator>().SetTrigger("Sleep");
        if (Data.CurrentMinute == 719) GameObject.Find("Farm handler").GetComponent<TimeSystem>().StartCoroutine(GameObject.Find("Farm handler").GetComponent<TimeSystem>().TimeTick());
        MusicHandler.StartTransition(SongType.Early);
    }

    public IEnumerator TimeTick()
    {
        yield return new WaitForSeconds(Data.Sleeping ? 0.005f : Options.Data.MinuteValue);

        // Add minute
        Data.CurrentMinute++;

        // Day/night system
        switch (Data.CurrentMinute)
        {
            case 180:
                setMorning();
                break;
            case 270:
                setShopOpen();
                break;
            case 510:     
                setEvening();
                break;
            case 630:
                setNight();
                break;
            case 720:
                setEarly();
                break;
        }

        // Clock
        UI.Elements["Clock hand"].transform.eulerAngles = new Vector3(0, 0, -1f * Data.CurrentMinute);      

        // Objects
        foreach (Timer t in Data.Timers.ToArray())
        {
            t.Cycle();
        }

        // New tick
        if (Data.Sleeping || Data.CurrentMinute < 719) StartCoroutine(TimeTick());
    }

    IEnumerator ColorTransition(Color a, Color b)
    {
        float i = 0;
        while (i < 1)
        {
            if (Data.Sleeping) i = 1;
            else
            {
                i += 0.005f;
                Background.GetComponent<SpriteRenderer>().color = Color.Lerp(a, b, i);
                yield return new WaitForSeconds(Options.Data.MinuteValue / 10);
            }
        }
    }

    public void NewDayCall()
    {
        Master.Data.ShopEarnings.Add(Shop.TodayEarnings);
        Shop.TodayEarnings = 0;

        Master.Data.LastDayPaid += Master.Data.LastDayWaterUsage;
        Master.Data.LastDayPaid += Master.Data.LastDayEnergyUsage;

        if (Master.Data.Debt > Master.Data.DailyDebt)
        { 
            if (Master.Data.DailyDebt + Master.Data.LastDayPaid <= Master.Data.Balance)
            {
                Master.Data.Debt -= Master.Data.DailyDebt;
                Master.Data.LastDayDebt = Master.Data.DailyDebt;
                Master.Data.LastDayPaid += Master.Data.DailyDebt;
            }
        }
        else if (Master.Data.Debt > 0)
        {
            if (Master.Data.Debt + Master.Data.LastDayPaid <= Master.Data.Balance)
            {
                Master.Data.LastDayDebt = Master.Data.Debt;
                Master.Data.LastDayPaid += Master.Data.Debt;
                Master.Data.Debt = 0;
            }
        }
        Master.UpdateBalance(-Master.Data.LastDayPaid);

        AI.NewDay();
        GameObject.FindGameObjectWithTag("Mailbox").GetComponent<Mailbox>().UpdateLetters();        
        if (Management.Data.ExpandField) Management.ExpandField();

        Master.Data.Day++;
        Master.Data.LastDayDebt = 0;
        Master.Data.LastDayPaid = 0;
        Master.Data.LastDayWaterUsage = 0;
        Master.Data.LastDayEnergyUsage = 1; // The house needs light...

        Management.UpdateDebt();

        foreach (PlowedSoil p in Farm.PlowedSoils)
        {
            p.NewDay();
        }
        DeliverySystem.UpdatePackages();
        UI.Elements["Day display"].GetComponent<Text>().text = string.Format(Localization.Translations["Day"], Master.Data.Day);

        foreach (IObject gc in ObjectsHandler.Data.Objects.FindAll(x => x.Name == "Garbage can"))
        {
            ((GarbageCan)gc).Items = new List<IObject>();
        }

        GameSaver.SaveGame();
    }

    void setEarly()
    {
        Data.TimeState = TimeState.EARLY;
        Background.GetComponent<SpriteRenderer>().color = EarlyBackground;

        Data.CurrentMinute = 0;
        NewDayCall();
    }

    void setMorning()
    {
        Data.TimeState = TimeState.MORNING;
        Background.GetComponent<SpriteRenderer>().color = MorningBackground;
        
        Data.CurrentMinute = 180;
        Data.TimeSpeed = 1;
        Data.Sleeping = false;

        Bed.transform.Find("Sprite").GetComponent<Animator>().SetTrigger("StopSleep");
        Master.Player.transform.position = Bed.transform.Find("Player position").position;
        (Master.Player.GetComponent("PlayerMovement") as PlayerMovement).enabled = true;
        Master.Player.GetComponent<SpriteRenderer>().enabled = true;
        Master.Player.transform.Find("Character shadow").gameObject.SetActive(true);
        UI.Elements["Clock"].GetComponent<Image>().sprite = ClockMorning;
        House.GetComponent<SpriteRenderer>().sprite = HouseDay;

        if (!Data.Sleeping) MusicHandler.StartTransition(SongType.Morning);
    }

    void setShopOpen()
    {
        Data.TimeState = TimeState.SHOP_OPEN;
        if (Data.Sleeping) Background.GetComponent<SpriteRenderer>().color = ShopOpenBackground;
        else StartCoroutine(ColorTransition(MorningBackground, ShopOpenBackground));

        UI.Elements["Clock"].GetComponent<Image>().sprite = ClockDay;
        
        if (!Data.Sleeping) MusicHandler.StartTransition(SongType.ShopOpen);
    }

    void setEvening()
    {
        Data.TimeState = TimeState.EVENING;
        if (Data.Sleeping) Background.GetComponent<SpriteRenderer>().color = EveningBackground;
        else StartCoroutine(ColorTransition(ShopOpenBackground, EveningBackground));
        
        UI.Elements["Clock"].GetComponent<Image>().sprite = ClockEvening;
        House.GetComponent<SpriteRenderer>().sprite = HouseNoon;
        
        if (!Data.Sleeping) MusicHandler.StartTransition(SongType.Evening);
    }

    void setNight()
    {
        Data.TimeState = TimeState.NIGHT;
        if (Data.Sleeping) Background.GetComponent<SpriteRenderer>().color = NightBackground;
        else StartCoroutine(ColorTransition(EveningBackground, NightBackground));
        
        UI.Elements["Clock"].GetComponent<Image>().sprite = ClockNight;
        House.GetComponent<SpriteRenderer>().sprite = HouseNight;

        if (GameObject.FindGameObjectWithTag("Water pump") != null)
        {
            foreach (PlowedSoil p in Farm.PlowedSoils)
            {
                p.AutoWaterKit();
            }
        }
        
        if (!Data.Sleeping) MusicHandler.StartTransition(SongType.Night);
    }
}

public enum TimeState
{
    EARLY,
    MORNING,
    SHOP_OPEN,
    EVENING,
    NIGHT
}

public delegate void OnTimeReached();