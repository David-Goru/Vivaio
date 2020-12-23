using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DripBottle : IObject
{    
    [SerializeField]
    public int WaterUnits;

    public DripBottle(int waterUnits, string translationKey) : base("Drip bottle", "", 1, 1, translationKey)
    {
        WaterUnits = waterUnits;
    }

    public override string GetUIName()
    {
        return Localization.Translations[TranslationKey] + " (" + WaterUnits + "u)";
    }

    public override Sprite GetUISprite()
    {
        return UI.Sprites[Name + " " + WaterUnits];
    }
}