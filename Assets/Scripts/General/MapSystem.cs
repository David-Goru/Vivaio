using System;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    public static MapData Data;

    // When loading a game
    public static bool Load(MapData data)
    {
        try
        {
            Data = data;

            GameObject farmFloor = Resources.Load<GameObject>("Farm/Farm land");
            Transform ground = GameObject.Find("Farm").transform;
            foreach (Vector2 pos in data.FarmLands)
            {
                GameObject tile = (GameObject)Instantiate(farmFloor, pos, Quaternion.Euler(0, 0, 0));
                tile.transform.SetParent(ground);
            }
        }
        catch (Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "MapSystem", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new MapData(new List<Vector2>());

        Transform verticesHandler = GameObject.Find("Initializer").transform.Find("Vertices");   
      
        Vector3 firstVfarm = verticesHandler.Find("First vertex").position;
        Vector3 lastVfarm = verticesHandler.Find("Last vertex").position;
        GameObject farmFloor = Resources.Load<GameObject>("Farm/Farm land");
        Transform ground = GameObject.Find("Farm").transform;
        Vector3 farmMin = new Vector3(firstVfarm.x + 0.125f, firstVfarm.y + 0.125f, 0);
        Vector3 farmMax = new Vector3(lastVfarm.x - 0.125f, lastVfarm.y - 0.125f, 0);

        for (float i = farmMin.x; i <= farmMax.x; i += 0.5f)
        {
            for (float j = farmMin.y; j <= farmMax.y; j += 0.5f)
            {
                GameObject tile = (GameObject)Instantiate(farmFloor, new Vector2(i, j), Quaternion.Euler(0, 0, 0));
                tile.transform.SetParent(ground);
                Data.FarmLands.Add(tile.transform.position);
            }
        }

        return true;
    }

    public static void ExpandFarm(Vector2 firstV, Vector2 lastV)
    {
        GameObject farmFloor = Resources.Load<GameObject>("Farm/Farm land");
        Transform ground = GameObject.Find("Farm").transform;
        Vector3 farmMin = new Vector3(firstV.x + 0.125f, firstV.y + 0.125f, 0);
        Vector3 farmMax = new Vector3(lastV.x - 0.125f, lastV.y - 0.125f, 0);

        for (float i = farmMin.x; i <= farmMax.x; i += 0.5f)
        {
            for (float j = farmMin.y; j <= farmMax.y; j += 0.5f)
            {
                GameObject tile = (GameObject)Instantiate(farmFloor, new Vector2(i, j), Quaternion.Euler(0, 0, 0));
                tile.transform.SetParent(ground);
                Data.FarmLands.Add(tile.transform.position);
            }
        }
    }
}