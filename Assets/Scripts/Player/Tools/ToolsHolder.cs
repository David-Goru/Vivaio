using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsHolder : MonoBehaviour
{
    public static Hoe Hoe;
    public static Shovel Shovel;
    public static WateringCan WateringCan;
    public static Basket Basket;

    void OnMouseDown()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 1.1f) return;
        if (Inventory.ObjectInHand is Tool) ((Tool)Inventory.ObjectInHand).LetTool();
    }

    void OnMouseOver()
    {
        if (Inventory.ObjectInHand is Tool)
        {
            if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 1.1f)
                transform.Find("Let").gameObject.SetActive(false);
            else if (!transform.Find("Let").gameObject.activeSelf) transform.Find("Let").gameObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (transform.Find("Let").gameObject.activeSelf) transform.Find("Let").gameObject.SetActive(false);
    }
}