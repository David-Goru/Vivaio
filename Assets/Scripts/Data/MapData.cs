using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MapData
{ 
    [SerializeField]
    public List<Vector2> FarmLands;

    public MapData(List<Vector2> farmLands)
    {
        FarmLands = farmLands;
    }
}