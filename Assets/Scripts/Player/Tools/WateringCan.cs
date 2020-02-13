using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : Tool
{
    public override void UseTool(int amount)
    {
        Remaining -= amount;
        Inventory.ChangeObject("Watering can", "Tool");
    }

    public override bool CheckTool()
    {
        if (Remaining > 0) return true;
        return false;
    }
}