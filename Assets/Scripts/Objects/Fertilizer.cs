using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : IObject
{
    public int Amount;

    public Fertilizer(int amount)
    {
        Amount = amount;
    }

    public void UseFertilizer()
    {
        Amount--;
        if (Amount == 0) Inventory.RemoveObject();
        else Inventory.ChangeObject();
    }
}