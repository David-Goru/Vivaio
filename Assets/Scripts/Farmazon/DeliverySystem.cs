using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public static DeliverySystemData Data;
    public static Vector2[] PointPositions = new Vector2[] { new Vector2(-13.75f, 10.75f), 
                                                             new Vector2(-12.9f, 10.75f), 
                                                             new Vector2(-13.75f, 10.25f), 
                                                             new Vector2(-12.9f, 10.25f) };

    // When loading a game
    public static bool Load(DeliverySystemData data)
    {
        try
        {
            Data = data; 
        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "DeliverySystem", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {        
        Data = new DeliverySystemData();
        Data.DeliveryList = new List<Box>();
        Data.DeliveryPoints = new List<DeliveryPoint>();

        for (int i = 0; i < PointPositions.Length; i++)
        {
            Data.DeliveryPoints.Add(new DeliveryPoint(PointPositions[i]));
        }

        // First grandma present
        Box box = new Box("Present box", "PresentBox");
        DeliveryPoint point = Data.DeliveryPoints[1];
        point.Available = false;
        box.Point = point;
        box.Model = Instantiate(Resources.Load<GameObject>("Objects/Present box"), point.Pos, Quaternion.Euler(0, 0, 0));
        box.Model.name = "Present box";
        string type = "Family";
        string title = "I'm glad";
        string body = "Dear grandson, \n\nI'm glad you finally decided to make the big step. Now, you're all on your own, and will have the power to build whatever future you want. \n\nI want you to make your best, so here is my bit: carrot seeds. Treat them well, and make them glad of their children, just as you make me glad. Good luck!";
        string signature = "Grandma";
        box.Items[0] = new Letter(type, title, body, signature);
        box.Items[1] = new Seed("Carrot", 10, 10, "CarrotSeeds");
        box.Placed = true;
        box.WorldPosition = point.Pos;
        ObjectsHandler.Data.Objects.Add(box);
        
        return true;
    }

    public static void UpdatePackages()
    {
        foreach (Box box in Data.DeliveryList.ToArray())
        {
            DeliveryPoint point = null;
            List<DeliveryPoint> pointsChecked = new List<DeliveryPoint>();

            while (point == null && pointsChecked.Count < Data.DeliveryPoints.Count)
            {
                DeliveryPoint pointToCheck = null;
                while (pointToCheck == null)
                {
                    pointToCheck = Data.DeliveryPoints[Random.Range(0, Data.DeliveryPoints.Count)];
                    if (pointsChecked.Contains(pointToCheck)) pointToCheck = null;
                    else pointsChecked.Add(pointToCheck);
                }

                if (pointToCheck.Available) point = pointToCheck;
            }

            if (point != null)
            {
                point.Available = false;
                box.Point = point;
                box.Model = Instantiate(Resources.Load<GameObject>("Objects/Delivery box"), point.Pos, Quaternion.Euler(0, 0, 0));
                box.Model.name = "Delivery box";
                Data.DeliveryList.Remove(box);
                box.Placed = true;
                box.WorldPosition = point.Pos;
                ObjectsHandler.Data.Objects.Add(box);
            }
            else break;
        }
    }
}