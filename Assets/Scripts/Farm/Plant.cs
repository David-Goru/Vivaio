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
    public Sprite[] Watered;
    public Sprite[] Unwatered;
    public Sprite[] Dry;


    public Plant(string name, int levels, int minAmount, int maxAmount, int fertilizerExtra, int daysUntilDry)
    {
        Name = name;
        Levels = levels;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        FertilizerExtra = fertilizerExtra;
        DaysUntilDry = daysUntilDry;

        Watered = new Sprite[levels];
        Unwatered = new Sprite[levels];
        Dry = new Sprite[levels];

        for (int i = 0; i < levels; i++)
        {
            Watered[i] = Resources.Load<Sprite>("Crops/" + name + "/Watered/" + i);
            Unwatered[i] = Resources.Load<Sprite>("Crops/" + name + "/Unwatered/" + i);
            Dry[i] = Resources.Load<Sprite>("Crops/" + name + "/Dry/" + i);
        }
    }
}