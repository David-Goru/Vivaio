using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wall : BuildableObject
{
    [SerializeField]
    public string SpriteType = "None";

    public Wall(string name, int stack, int maxStack, string translationKey) : base(name, stack, maxStack, translationKey) { }

    public override void ActionTwo()
    {
        if (Inventory.AddObject(this) == 0) return;

        ObjectsHandler.Data.Objects.Remove(this);
        Placed = false;
        SpriteType = "None";
        foreach (Transform t in Model.transform.Find("Vertices"))
        {                            
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null) v.State = VertexState.Available;
        }
        foreach (Wall w in GetCollidingWalls())
        {
            w.CheckRotation();
        }
        MonoBehaviour.Destroy(Model);
    }

    public void CheckRotation(bool propagate = false)
    {
        string sprite = Name;

        // Check left
        bool left = false;
        if (WorldPosition != new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y))
            left = ObjectsHandler.Data.Objects.Exists(x => x.WorldPosition == new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y) && x.Name == Name);

        bool right = false;
        if (WorldPosition != new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y))
            right = ObjectsHandler.Data.Objects.Exists(x => x.WorldPosition == new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y) && x.Name == Name);

        bool down = false;
        if (WorldPosition != new Vector2(Model.transform.position.x, Model.transform.position.y - 0.25f))
            down = ObjectsHandler.Data.Objects.Exists(x => x.WorldPosition == new Vector2(Model.transform.position.x, Model.transform.position.y - 0.25f) && x.Name == Name);

        bool up = false;
        if (WorldPosition != new Vector2(Model.transform.position.x, Model.transform.position.y + 0.25f))
            up = ObjectsHandler.Data.Objects.Exists(x => x.WorldPosition == new Vector2(Model.transform.position.x, Model.transform.position.y + 0.25f) && x.Name == Name);

        if (left) sprite += " Left";
        if (right) sprite += " Right";
        if (down) sprite += " Down";
        if (up) sprite += " Up";

        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/" + Name + "/" + sprite);
        SpriteType = sprite;

        if (propagate)
        {
            if (left) ((Wall)ObjectsHandler.Data.Objects.Find(x => x.WorldPosition == new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y))).CheckRotation();
            if (right) ((Wall)ObjectsHandler.Data.Objects.Find(x => x.WorldPosition == new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y))).CheckRotation();
            if (down) ((Wall)ObjectsHandler.Data.Objects.Find(x => x.WorldPosition == new Vector2(Model.transform.position.x, Model.transform.position.y - 0.25f))).CheckRotation();
            if (up) ((Wall)ObjectsHandler.Data.Objects.Find(x => x.WorldPosition == new Vector2(Model.transform.position.x, Model.transform.position.y + 0.25f))).CheckRotation();
        }
    }

    public List<Wall> GetCollidingWalls()
    {
        List<Wall> walls = new List<Wall>();

        if (ObjectsHandler.Data.Objects.Exists(x => x.WorldPosition == new Vector2(WorldPosition.x - 0.25f, WorldPosition.y) && x.Name == Name))
            walls.Add((Wall)ObjectsHandler.Data.Objects.Find(x => x.WorldPosition == new Vector2(WorldPosition.x - 0.25f, WorldPosition.y)));

        if (ObjectsHandler.Data.Objects.Exists(x => x.WorldPosition == new Vector2(WorldPosition.x + 0.25f, WorldPosition.y) && x.Name == Name))
            walls.Add((Wall)ObjectsHandler.Data.Objects.Find(x => x.WorldPosition == new Vector2(WorldPosition.x + 0.25f, WorldPosition.y)));

        if (ObjectsHandler.Data.Objects.Exists(x => x.WorldPosition == new Vector2(WorldPosition.x, WorldPosition.y - 0.25f) && x.Name == Name))
            walls.Add((Wall)ObjectsHandler.Data.Objects.Find(x => x.WorldPosition == new Vector2(WorldPosition.x, WorldPosition.y - 0.25f)));

        if (ObjectsHandler.Data.Objects.Exists(x => x.WorldPosition == new Vector2(WorldPosition.x, WorldPosition.y + 0.25f) && x.Name == Name))
            walls.Add((Wall)ObjectsHandler.Data.Objects.Find(x => x.WorldPosition == new Vector2(WorldPosition.x, WorldPosition.y + 0.25f)));

        return walls;
    }

    public override void LoadObjectCustom()
    {
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/" + Name + "/" + SpriteType);
    }
}