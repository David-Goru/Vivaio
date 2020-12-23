using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Basket : Tool
{
    public Basket() : base("Basket") {}

    [System.NonSerialized]
    public Product Product;
    [SerializeField]
    public int Amount;

    public override string GetUIName()
    {
        return Localization.Translations[TranslationKey] + " (" + Amount + "/10)";
    }

    public override Sprite GetUISprite()
    {
        if (Product != null) return UI.Sprites[Name + " with " + Product.Name];
        return UI.Sprites[Name];
    }
}