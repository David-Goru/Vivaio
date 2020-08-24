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
                    Master.Data.LastDayWaterUsage += 10 - wc.Remaining;
                    wc.Remaining = 10;
                    Inventory.ChangeObject();
                }
            }
        }
    }
}