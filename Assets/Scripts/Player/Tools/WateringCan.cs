﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WateringCan : Tool
{
    [SerializeField]
    public int Remaining;

    public override void UseTool(int amount)
    {
        Remaining -= amount;
        Inventory.ChangeObject();
    }

    public override bool CheckTool()
    {
        if (Remaining > 0) return true;
        return false;
    }
}