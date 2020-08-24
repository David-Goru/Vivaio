using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DripBottle : IObject
{    
    [SerializeField]
    public int WaterUnits;

    public DripBottle(int waterUnits) : base("Drip bottle", 1, 1)
    {
        WaterUnits = waterUnits;
    }
}