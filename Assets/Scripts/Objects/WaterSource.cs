using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaterSource : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Inventory.Data.ObjectInHand is WateringCan && !EventSystem.current.IsPointerOverGameObject())
        {
            WateringCan wc = (WateringCan)Inventory.Data.ObjectInHand;
            if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
            {
                if (wc.Remaining < 10)
                {
                    GetComponent<Animator>().SetTrigger("UseSource");
                    GameObject waterPump = GameObject.FindGameObjectWithTag("Water pump");
                    if (waterPump != null) ((WaterPump)ObjectsHandler.Data.Objects.Find(x => x.Model == waterPump)).GetWater(10 - wc.Remaining);
                    else Master.Data.LastDayWaterUsage += 10 - wc.Remaining;

                    wc.Remaining = 10;
                    Inventory.ChangeObject();
                }
            }
        }
    }
}