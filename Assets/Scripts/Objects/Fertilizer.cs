using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fertilizer : IObject
{
    public Fertilizer(int stack, int maxStack) : base("Fertilizer", stack, maxStack)
    {
        Name = "Fertilizer";
    }

    public void UseFertilizer()
    {
        Stack--;
        if (Stack == 0) Inventory.RemoveObject();
        else Inventory.ChangeObject();
    }
}