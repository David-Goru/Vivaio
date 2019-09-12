using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public static string TimeState;
    public int MorningTime; // Time the player has until the shop opens. In seconds
    public int ShoppingTime; // Time the shop remains open. In seconds

    public GameObject DisplayTimeState;

    void Start()
    {
        TimeState = "Morning";
        DisplayTimeState.GetComponent<Text>().text = "Current time: " + TimeState;

        StartCoroutine(StateHandler());
    }

    IEnumerator StateHandler()
    {
        yield return new WaitForSeconds(MorningTime);

        // Open shop
        TimeState = "Shop open";
        DisplayTimeState.GetComponent<Text>().text = "Current time: " + TimeState;

        yield return new WaitForSeconds(ShoppingTime);

        // Close shop, start sunset and whatever
        TimeState = "Night"; // Night lasts until player decides to sleep
        DisplayTimeState.GetComponent<Text>().text = "Current time: " + TimeState;
    }

    public void OnMouseDown()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) < 1 && TimeState == "Night")
        {
            TimeState = "Sleepig";
            DisplayTimeState.GetComponent<Text>().text = "Current time: " + TimeState;
            // Sleep, black screen, etc
            TimeState = "Morning";
            DisplayTimeState.GetComponent<Text>().text = "Current time: " + TimeState;
            // Dawn, wake up and whatever
            NewDayCall();
            StartCoroutine(StateHandler());
        }
    }

    public void NewDayCall()
    {
        foreach (Farm.SimpleCrop crop in Farm.SimpleCrops.ToArray())
        {
            crop.NewDay();
        }
    }
}