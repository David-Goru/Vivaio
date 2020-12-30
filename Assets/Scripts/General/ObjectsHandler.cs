using System.Collections.Generic;
using UnityEngine;

public class ObjectsHandler : MonoBehaviour
{
    public static ObjectsData Data;

    // When loading a game
    public static bool Load(ObjectsData data)
    {        
        Data = data;
        foreach (IObject o in Data.Objects)
        {
            try
            {
                o.LoadObject();
            }
            catch (System.Exception e)
            {
                Master.AddLog("Error loading data for " + o.Name + " (ObjectsHandler): " + e);
            }
        }        

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new ObjectsData();
        Data.Objects = new List<IObject>();

        // Add bed
        Bed bed = new Bed("Bed");
        bed.Placed = true;
        bed.Model = Instantiate(Resources.Load<GameObject>("Objects/Bed"), new Vector2(-17.75f, -2.25f), Quaternion.Euler(0, 0, 0));     
        bed.WorldPosition = bed.Model.transform.position;

        foreach (Transform t in bed.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(bed);
        TimeSystem.Bed = bed.Model;

        // Add house lamp
        BuildableObject lamp = new Lamp("House lamp", "HouseLamp");
        lamp.Placed = true;
        lamp.Model = Instantiate(Resources.Load<GameObject>("Objects/House lamp"), new Vector2(-20.5f, -2), Quaternion.Euler(0, 0, 0));     
        lamp.WorldPosition = lamp.Model.transform.position;

        foreach (Transform t in lamp.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(lamp);

        return true;
    }
}