using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableObject : IObject
{
    public int Amount;

    public BuildableObject(int amount) 
    { 
        Amount = amount;
    }
}