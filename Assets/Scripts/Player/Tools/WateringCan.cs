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
        if (Remaining == 0) Model.GetComponent<SpriteRenderer>().sprite = UI.Sprites["Watering can empty"];
    }

    public override string GetUIName()
    {
        return Localization.Translations[TranslationKey] + " (" + Remaining + "/10)";
    }

    public override Sprite GetUISprite()
    {
        if (Remaining == 0) return UI.Sprites[Name + " empty"];
        return UI.Sprites[Name];
    }
}