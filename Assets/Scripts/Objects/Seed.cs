using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : IObject
{
    public string Type;
    public int Amount;

    public Seed(string type, int amount)
    {
        Type = type;
        Amount = amount;
    }

    public void UseSeed()
    {
        Amount--;
        if (Amount == 0) Inventory.RemoveObject();
        else Inventory.ChangeObject();
    }
}