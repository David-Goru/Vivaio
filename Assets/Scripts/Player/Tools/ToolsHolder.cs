using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsHolder : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 1) return;
        if (PlayerTools.ToolOnHand != null && PlayerTools.ToolOnHand.Name != "Seed") PlayerTools.ToolOnHand.LetTool();
    }

    void OnMouseOver()
    {
        if (PlayerTools.ToolOnHand != null && PlayerTools.ToolOnHand.Name != "Seed")
        {
            if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 1)
                transform.Find("Let").gameObject.SetActive(false);
            else if (!transform.Find("Let").gameObject.activeSelf) transform.Find("Let").gameObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (transform.Find("Let").gameObject.activeSelf) transform.Find("Let").gameObject.SetActive(false);
    }
}