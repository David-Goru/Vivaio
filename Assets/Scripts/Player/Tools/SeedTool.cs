using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedTool : Tool
{
    public string SeedName;

    public override void TakeTool()
    {
        PlayerTools.ToolOnHand = this;
        Inventory.ChangeObject(SeedName, "Seed");
        GameObject.Find("UI").transform.Find("Throw seeds").gameObject.SetActive(true);
    }

    public override void UseTool(int none)
    {
        Remaining--;
        Inventory.ChangeObject(SeedName, "Seed");

        if (Remaining == 0) LetTool();
    }

    public override void LetTool()
    {
        GameObject.Find("UI").transform.Find("Throw seeds").gameObject.SetActive(false);
        PlayerTools.ToolOnHand = null;
        Inventory.ChangeObject("", "None");
    }
}