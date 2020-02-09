using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public string Name;
    public int Remaining;
    public bool OnHand;

    void OnMouseDown()
    {
        if (PlayerTools.ToolOnHand == null && Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= 1) TakeTool();
    }

    void OnMouseOver()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 1)
            transform.Find("Text").gameObject.SetActive(false);
        else if (!transform.Find("Text").gameObject.activeSelf && Inventory.InventoryText.text == "") transform.Find("Text").gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        if (transform.Find("Text").gameObject.activeSelf) transform.Find("Text").gameObject.SetActive(false);
    }

    public void ChangeVisual()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = OnHand;
        gameObject.GetComponent<SpriteRenderer>().enabled = OnHand;
        OnHand = !OnHand;
    }

    public virtual void TakeTool()
    {
        PlayerTools.ToolOnHand = this;
        Inventory.ChangeObject(Name, "Tool");
        ChangeVisual();
    }

    public virtual void UseTool() {}

    public virtual bool CheckTool() 
    {
        return true;
    }

    public virtual void LetTool()
    {
        PlayerTools.ToolOnHand = null;
        Inventory.ChangeObject("", "None");
        ChangeVisual();
    }
}