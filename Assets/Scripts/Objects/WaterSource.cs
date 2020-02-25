using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaterSource : MonoBehaviour
{
    public static int WaterUsage;

    void OnMouseDown()
    {
        if (Inventory.ObjectInHand is WateringCan && !EventSystem.current.IsPointerOverGameObject())
        {
            WateringCan wc = (WateringCan)Inventory.ObjectInHand;
            if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
            {
                if (wc.Remaining < 10)
                {
                    GetComponent<Animator>().SetTrigger("UseSource");
                    WaterUsage += 10 - wc.Remaining;
                    wc.Remaining = 10;
                    Inventory.ChangeObject();
                }
            }
        }
    }
}