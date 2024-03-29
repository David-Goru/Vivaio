﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Basket : Tool
{
    public Basket() : base("Basket") {}

    [SerializeField]
    public string ProductName;
    [System.NonSerialized]
    public Product Product;
    [SerializeField]
    public int Amount;

    public override string GetUIName()
    {
        if (Product != null) return string.Format(Localization.Translations["BasketWithProduct"], Localization.Translations[Product.TranslationKey].ToLower(), Amount, 20);
        return Localization.Translations[TranslationKey];
    }

    public override Sprite GetUISprite()
    {
        if (Product != null) return UI.Sprites[Name + " with " + Product.Name];
        return UI.Sprites[Name];
    }

    public override void LoadObjectCustom()
    {
        if (Amount > 0) Product = Products.ProductsList.Find(x => x.Name == ProductName);
    }
}