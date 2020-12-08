using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Floor : BuildableObject
{
    [SerializeField]
    public string SpriteType = "None";

    public Floor(string name, int stack, int maxStack, string translationKey) : base(name, stack, maxStack, translationKey) { }

    public void CheckRotation(bool propagate = false)
    {
        string sprite = Name;

        // Check left
        bool left = false;
        if (Physics2D.OverlapPoint(new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y), 1 << LayerMask.NameToLayer("Floor")))
        {
            GameObject floor = Physics2D.OverlapPoint(new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject;
            Transform t = floor.transform.Find("Vertices").Find("Vertex");
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null && v.Floor == Name) left = true;
        }

        bool right = false;
        if (Physics2D.OverlapPoint(new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y), 1 << LayerMask.NameToLayer("Floor")))
        {
            GameObject floor = Physics2D.OverlapPoint(new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject;
            Transform t = floor.transform.Find("Vertices").Find("Vertex");
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null && v.Floor == Name) right = true;
        }

        bool down = false;
        if (Physics2D.OverlapPoint(new Vector2(Model.transform.position.x, Model.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
        {
            GameObject floor = Physics2D.OverlapPoint(new Vector2(Model.transform.position.x, Model.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
            Transform t = floor.transform.Find("Vertices").Find("Vertex");
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null && v.Floor == Name) down = true;
        }

        bool up = false;
        if (Physics2D.OverlapPoint(new Vector2(Model.transform.position.x, Model.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
        {
            GameObject floor = Physics2D.OverlapPoint(new Vector2(Model.transform.position.x, Model.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
            Transform t = floor.transform.Find("Vertices").Find("Vertex");
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null && v.Floor == Name) up = true;
        }

        if (left) sprite += " Left";
        if (right) sprite += " Right";
        if (down) sprite += " Down";
        if (up) sprite += " Up";

        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/" + Name + "/" + sprite);
        SpriteType = sprite;

        if (propagate)
        {
            if (left) UpdateFloorSprite(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (right) UpdateFloorSprite(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (down) UpdateFloorSprite(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x, Model.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (up) UpdateFloorSprite(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x, Model.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
        }
    }

    public static List<GameObject> GetCollidingFloors(GameObject floor)
    {
        List<GameObject> floors = new List<GameObject>();

        if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - 0.25f, floor.transform.position.y), 1 << LayerMask.NameToLayer("Floor")))
            floors.Add(Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - 0.25f, floor.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject);

        if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x + 0.25f, floor.transform.position.y), 1 << LayerMask.NameToLayer("Floor")))
            floors.Add(Physics2D.OverlapPoint(new Vector2(floor.transform.position.x + 0.25f, floor.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject);

        if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x, floor.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            floors.Add(Physics2D.OverlapPoint(new Vector2(floor.transform.position.x, floor.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject);

        if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x, floor.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            floors.Add(Physics2D.OverlapPoint(new Vector2(floor.transform.position.x, floor.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject);

        return floors;
    }

    public static void UpdateFloorSprite(GameObject floor, string floorName)
    {
        Transform t = floor.transform.Find("Vertices").Find("Vertex");
        Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
        if (v != null && v.Floor == floorName)
        {
            string sprite = floorName;

            // Check left
            bool left = false;
            if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - 0.25f, floor.transform.position.y), 1 << LayerMask.NameToLayer("Floor")))
            {
                GameObject checkFloor = Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - 0.25f, floor.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                Transform checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                Vertex checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                if (checkV != null && checkV.Floor == floorName) left = true;
            }

            bool right = false;
            if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x + 0.25f, floor.transform.position.y), 1 << LayerMask.NameToLayer("Floor")))
            {
                GameObject checkFloor = Physics2D.OverlapPoint(new Vector2(floor.transform.position.x + 0.25f, floor.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                Transform checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                Vertex checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                if (checkV != null && checkV.Floor == floorName) right = true;
            }

            bool down = false;
            if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x, floor.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            {
                GameObject checkFloor = Physics2D.OverlapPoint(new Vector2(floor.transform.position.x, floor.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                Transform checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                Vertex checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                if (checkV != null && checkV.Floor == floorName) down = true;
            }

            bool up = false;
            if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x, floor.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            {
                GameObject checkFloor = Physics2D.OverlapPoint(new Vector2(floor.transform.position.x , floor.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                Transform checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                Vertex checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                if (checkV != null && checkV.Floor == floorName) up = true;
            }

            if (left) sprite += " Left";
            if (right) sprite += " Right";
            if (down) sprite += " Down";
            if (up) sprite += " Up";

            floor.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/" + floorName + "/" + sprite);
            v.FloorType = sprite;
        }
    }
}