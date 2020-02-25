using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : IObject
{
    public GameObject Model;
    public bool OnHand;

    public void ChangeVisual()
    {
        Model.gameObject.GetComponent<BoxCollider2D>().enabled = OnHand;
        Model.gameObject.GetComponent<SpriteRenderer>().enabled = OnHand;
        OnHand = !OnHand;
    }

    public virtual void TakeTool()
    {
        Inventory.ObjectInHand = this;
        Inventory.ChangeObject();
        ChangeVisual();
    }

    public virtual void UseTool(int amount) {}

    public virtual bool CheckTool() 
    {
        return true;
    }

    public virtual void LetTool()
    {
        Inventory.RemoveObject();
        ChangeVisual();
    }
}