using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Seed : IObject
{
    [SerializeField]
    public string Type;

    public Seed(string type, int stack,  int maxStack, string translationKey) : base(type + " seeds", stack, maxStack, translationKey)
    {
        Type = type;
        Name = type + " seeds";
    }

    public void UseSeed()
    {
        Stack--;
        if (Stack == 0) Inventory.RemoveObject();
        else Inventory.ChangeObject();
    }
}