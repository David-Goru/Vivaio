using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int MinX;
    public int MaxX;
    public int MinY;
    public int MaxY;

    void Start()
    {
        GameObject farmFloor = (GameObject)Resources.Load("Farm land");
        Transform ground = GameObject.Find("Farm").transform;

        for (int i = MinX; i < MaxX; i++)
        {
            for (int j = MinY; j < MaxY; j++)
            {
                GameObject tile = (GameObject)Instantiate(farmFloor, new Vector2(i, j), Quaternion.Euler(0, 0, 0));
                tile.transform.SetParent(ground);
                tile.name = "Grass";
            }
        }
    }
}