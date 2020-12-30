using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public static DeliverySystemData Data;
    public static Vector2[] PointPositions;

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
                if (Master.GameEdition == "Christmas") box.Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = VersionHandlerGame.DeliveryBoxChristmas;
                Data.DeliveryList.Remove(box);
                box.Placed = true;
                box.WorldPosition = point.Pos;
                ObjectsHandler.Data.Objects.Add(box);
            }
            else break;
        }
    }
}