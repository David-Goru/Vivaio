using System.Collections.Generic;
using UnityEngine;

public class ObjectsHandler : MonoBehaviour
{
    public static Dictionary<string, ObjectInfo> ObjectInfo;
    public static ObjectsData Data;

    // When loading a game
    public static bool Load(ObjectsData data)
    {
        try
        {
            ObjectInfo = new Dictionary<string, ObjectInfo>();

            foreach (ObjectInfo oi in Resources.LoadAll("Objects info"))
            {
                ObjectInfo.Add(oi.Name, oi);
            }

            Data = data;
            foreach (IObject o in Data.Objects)
            {
                o.LoadObject();
            }
        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "ObjectsHandler", e));
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
        Bed bed = new Bed();
        bed.Placed = true;
        bed.Model = Instantiate(Resources.Load<GameObject>("Objects/Bed"), new Vector2(-48.25f, 7.75f), Quaternion.Euler(0, 0, 0));     
        bed.WorldPosition = bed.Model.transform.position;

        foreach (Transform t in bed.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(bed);
        TimeSystem.Bed = bed.Model;

        // Add cash register
        CashRegister cashRegister = new CashRegister();
        cashRegister.Placed = true;
        cashRegister.Model = Instantiate(Resources.Load<GameObject>("Objects/Cash register"), new Vector2(-13.75f, 9.25f), Quaternion.Euler(0, 0, 0));     
        cashRegister.WorldPosition = cashRegister.Model.transform.position;  

        CashRegisterHandler.CustomerPos = new List<Vector2>();
        foreach (Transform t in cashRegister.Model.transform.Find("Customer position"))
        {
            CashRegisterHandler.CustomerPos.Add(t.position);
        }

        foreach (Transform t in cashRegister.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(cashRegister);
        CashRegisterHandler.CashRegisterModel = cashRegister.Model;

        // Add house lamp
        BuildableObject lamp = new Lamp("House lamp");
        lamp.Placed = true;
        lamp.Model = Instantiate(Resources.Load<GameObject>("Objects/House lamp"), new Vector2(-51, 8), Quaternion.Euler(0, 0, 0));     
        lamp.WorldPosition = lamp.Model.transform.position;

        foreach (Transform t in lamp.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(lamp);

        return true;
    }
}