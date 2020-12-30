using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tool : IObject
{
    [System.NonSerialized]
    public AudioClip[] Clips;
    [SerializeField]
    public bool OnStand;

    public Tool(string translationKey) : base("Tool", 1, 1, translationKey) {}

    public void ChangeVisual()
    {
        OnStand = !OnStand;
        Model.gameObject.GetComponent<SpriteRenderer>().enabled = OnStand;
    }

    public virtual void TakeTool()
    {
        if (OnStand)
        {
            Inventory.AddObject(this);
            ChangeVisual();
        }
    }

    public virtual void UseTool(int amount) {}

    public virtual bool CheckTool() 
    {
        return true;
    }

    public virtual void LetTool()
    {
        if (Inventory.Data.ObjectInHand == this)
        {
            Inventory.RemoveObject();
            ChangeVisual();
        }
    }
}