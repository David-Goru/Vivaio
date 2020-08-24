using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsHolder : MonoBehaviour
{
    public static ToolsData Data;

    // When loading a game
    public static bool Load(ToolsData data)
    {
        try
        {
            Data = data;

            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Tool"))
            {
                g.GetComponent<ToolPhysical>().Load();
            }

        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "ToolsHolder", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new ToolsData();

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Tool"))
        {
            g.GetComponent<ToolPhysical>().New();
        }

        return true;
    }
}