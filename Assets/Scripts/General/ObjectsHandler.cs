using System.Collections.Generic;
using UnityEngine;

public class ObjectsHandler : MonoBehaviour
{
    public static Dictionary<string, ObjectInfo> ObjectInfo;
    public static ObjectsData Data;

    // When loading a game
    public static bool Load(ObjectsData data)
    {
        ObjectInfo = new Dictionary<string, ObjectInfo>();
        
        foreach (ObjectInfo oi in Resources.LoadAll("Objects info"))
        {
            try
            {
                ObjectInfo.Add(oi.Name, oi);
            }
            catch (System.Exception e)
            {
                Master.AddLog("Error adding ObjectInfo (ObjectsHandler): " + e);
            }
        }

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
        ObjectInfo = new Dictionary<string, ObjectInfo>();

        foreach (ObjectInfo oi in Resources.LoadAll("Objects info"))
        {
            ObjectInfo.Add(oi.Name, oi);
        }

        Data = new ObjectsData();
        Data.Objects = new List<IObject>();

        // Add bed
        Bed bed = new Bed("Bed");
        bed.Placed = true;
        bed.Model = Instantiate(Resources.Load<GameObject>("Objects/Bed"), new Vector2(-17.75f, -2.5f), Quaternion.Euler(0, 0, 0));     
        bed.WorldPosition = bed.Model.transform.position;

        foreach (Transform t in bed.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(bed);
        TimeSystem.Bed = bed.Model;

        // Add cash register
        CashRegister cashRegister = new CashRegister("CashRegister");
        cashRegister.Placed = true;
        cashRegister.Model = Instantiate(Resources.Load<GameObject>("Objects/Cash register"), new Vector2(-3.5f, -1), Quaternion.Euler(0, 0, 0));     
        cashRegister.WorldPosition = cashRegister.Model.transform.position;  

        CashRegisterHandler.CustomerPos = new List<Vector2>();
        foreach (Transform t in cashRegister.Model.transform.Find("Customer position"))
        {
            CashRegisterHandler.CustomerPos.Add(t.position);
        }

        foreach (Transform t in cashRegister.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(cashRegister);
        CashRegisterHandler.CashRegisterModel = cashRegister.Model;

        // Add house lamp
        BuildableObject lamp = new Lamp("House lamp", "HouseLamp");
        lamp.Placed = true;
        lamp.Model = Instantiate(Resources.Load<GameObject>("Objects/House lamp"), new Vector2(-20.75f, -2), Quaternion.Euler(0, 0, 0));     
        lamp.WorldPosition = lamp.Model.transform.position;

        foreach (Transform t in lamp.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(lamp);

        // Add garbage can
        GarbageCan garbageCan = new GarbageCan("GarbageCan");
        garbageCan.Placed = true;
        garbageCan.Model = Instantiate(Resources.Load<GameObject>("Objects/Garbage can"), new Vector2(-1.75f, -0.75f), Quaternion.Euler(0, 0, 0));     
        garbageCan.WorldPosition = garbageCan.Model.transform.position;  

        foreach (Transform t in garbageCan.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(garbageCan);

        return true;
    }
}