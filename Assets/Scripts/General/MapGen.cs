using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;

    void Start()
    {
        GameObject farmFloor = (GameObject)Resources.Load("Farm land");
        Transform ground = GameObject.Find("Farm").transform;

        for (float i = MinX; i <= MaxX; i += 0.5f)
        {
            for (float j = MinY; j <= MaxY; j += 0.5f)
            {
                GameObject tile = (GameObject)Instantiate(farmFloor, new Vector2(i, j), Quaternion.Euler(0, 0, 0));
                tile.transform.SetParent(ground);
                tile.name = "Grass";
            }
        }
    }
}