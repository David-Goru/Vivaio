using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopTicket
{
    [SerializeField]
    public string Consumer;
    [SerializeField]
    public int Day;
    [SerializeField]
    public int Total;
    [SerializeField]
    public string ItemsBought;
    [SerializeField]
    public int TotalLines;

    public ShopTicket(string consumer, int total, string itemsBought, int itemsLines)
    {
        Consumer = consumer;
        Day = Master.Data.Day;
        Total = total;
        ItemsBought = itemsBought;
        TotalLines = 2 + itemsLines;
    }
}