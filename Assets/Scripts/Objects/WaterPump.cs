using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class WaterPump : BuildableObject
{
    [SerializeField]
    public int WaterUsage;

    public WaterPump(string translationKey) : base("Water pump", 1, 1, translationKey) 
    {
        WaterUsage = 0;
    }

    public void GetWater(int amount)
    {
        WaterUsage += amount;
        if (WaterUsage >= 20)
        {
            WaterUsage -= 20;
            Master.Data.LastDayEnergyUsage += 1;
        }
    }

    public override void ActionTwo()
    {
        if (Inventory.Data.ObjectInHand is WateringCan && !EventSystem.current.IsPointerOverGameObject())
        {
            WateringCan wc = (WateringCan)Inventory.Data.ObjectInHand;
            if (Vector2.Distance(GameObject.Find("Player").transform.position, Model.transform.position) <= 1.5f)
            {
                if (wc.Remaining < 10)
                {
                    WaterUsage += (10 - wc.Remaining);
                    wc.Remaining = 10;

                    if (WaterUsage >= 20)
                    {
                        WaterUsage -= 20;
                        Master.Data.LastDayEnergyUsage += 1;
                    }
                    Inventory.ChangeObject();
                }
            }
        }
    }

    public override void LoadObjectCustom()
    {        
        foreach (PlowedSoil p in Farm.PlowedSoils)
        {
            p.CheckDripIrrigationWarning(false);
        }
    }
}