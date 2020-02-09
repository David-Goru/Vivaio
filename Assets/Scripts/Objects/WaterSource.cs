using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaterSource : MonoBehaviour
{
    public static int WaterUsage;

    void OnMouseDown()
    {
        if (PlayerTools.ToolOnHand != null && PlayerTools.ToolOnHand.Name == "Watering can" && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
            {
                if (PlayerTools.ToolOnHand.Remaining < 10)
                {
                    GetComponent<Animator>().SetTrigger("UseSource");
                    WaterUsage += 10 - PlayerTools.ToolOnHand.Remaining;
                    PlayerTools.ToolOnHand.Remaining = 10;
                    Inventory.ChangeObject("Watering can", "Tool");
                }
            }
        }
    }
}