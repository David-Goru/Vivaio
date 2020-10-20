using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WateringCan : Tool
{
    public WateringCan() : base("WateringCan") {}

    [SerializeField]
    public int Remaining;

    public override void UseTool(int amount)
    {
        Remaining -= amount;
        if (Remaining == 0) Master.RunSoundStatic(Clips[1]);
        Inventory.ChangeObject();
    }

    public override bool CheckTool()
    {
        if (Remaining > 0) return true;
        return false;
    }

    public override void LoadObjectCustom()
    {
        if (Remaining == 0) Model.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Watering can empty");
    }
}