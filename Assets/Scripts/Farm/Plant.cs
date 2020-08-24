using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant
{
    // Info
    public string Name;
    public int Levels;
    public int MinAmount;
    public int MaxAmount;
    public int FertilizerExtra;
    public int DaysUntilDry;

    // Sprites
    public Sprite[] Normal;
    public Sprite[] Dry;
    public Sprite Harvested;


    public Plant(string name, int levels, int minAmount, int maxAmount, int fertilizerExtra, int daysUntilDry)
    {
        Name = name;
        Levels = levels;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        FertilizerExtra = fertilizerExtra;
        DaysUntilDry = daysUntilDry;
        
        Normal = new Sprite[levels];
        Dry = new Sprite[levels];        
        Harvested = Resources.Load<Sprite>("Crops/" + name + "/Harvested");

        for (int i = 0; i < levels; i++)
        {
            Normal[i] = Resources.Load<Sprite>("Crops/" + name + "/Normal/" + i);
            Dry[i] = Resources.Load<Sprite>("Crops/" + name + "/Dry/" + i);
        }
    }
}