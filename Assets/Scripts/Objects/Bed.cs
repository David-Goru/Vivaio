using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bed : BuildableObject
{
    public Bed() : base("Bed", 1, 1) {}

    public override void LoadObjectCustom()
    {        
        TimeSystem.Bed = Model;
        TimeSystem.Sleep();
    }
}