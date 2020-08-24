using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FarmData
{ 
    [SerializeField]
    public List<PlowedSoilData> PlowedSoils;

    public FarmData(List<PlowedSoilData> plowedSoils)
    {
        PlowedSoils = plowedSoils;
    }
}