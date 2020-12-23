using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DripIrrigationKit : IObject
{
    public DripIrrigationKit(string translationKey) : base("Drip irrigation kit", "", 1, 1, translationKey) { }
}