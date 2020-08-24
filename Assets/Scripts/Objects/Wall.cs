using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wall : BuildableObject
{
    public Wall(string name, int stack, int maxStack) : base(name, stack, maxStack, true) { }

    public override void ActionTwo()
    {
        if (Inventory.Data.ObjectInHand == null) Inventory.AddObject(this);
        else if (Inventory.Data.ObjectInHand is Wall && Inventory.Data.ObjectInHand.Stack < MaxStack)
        {
            Inventory.Data.ObjectInHand.Stack++;
            Inventory.ChangeObject();
        }
        else return;

        ObjectsHandler.Data.Objects.Remove(this);
        Placed = false;
        foreach (Transform t in Model.transform.Find("Vertices"))
        {                            
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            if (v != null) v.State = VertexState.Available;
        }
        MonoBehaviour.Destroy(Model);
    }
}