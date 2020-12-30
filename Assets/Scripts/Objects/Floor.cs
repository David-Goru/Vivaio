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

    public void CheckFullRotation(bool propagate = false)
    {        
        string sprite = "";
        Vector3 tilePos = Model.transform.position;
        GameObject checkFloor;
        Transform checkT;
        Vertex checkV;

        // Left
        bool left = false;
        if (Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y), 1 << LayerMask.NameToLayer("Floor")))
        {
            checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y), 1 << LayerMask.NameToLayer("Floor")).gameObject;
            checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
            checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
            if (checkV != null && checkV.Floor == Name) left = true;
        }

        bool right = false;
        if (Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y), 1 << LayerMask.NameToLayer("Floor")))
        {
            checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y), 1 << LayerMask.NameToLayer("Floor")).gameObject;
            checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
            checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
            if (checkV != null && checkV.Floor == Name) right = true;
        }

        bool up = false;
        if (Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
        {
            checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x , tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
            checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
            checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
            if (checkV != null && checkV.Floor == Name) up = true;
        }

        bool down = false;
        if (Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
        {
            checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
            checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
            checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
            if (checkV != null && checkV.Floor == Name) down = true;
        }

        // Upper row
        bool upLeft = false;
        bool upRight = false;
        if (up)
        {
            if (left)
            {
                if (Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
                {
                    checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                    checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                    checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                    if (checkV != null && checkV.Floor == Name) upLeft = true;
                }
                if (upLeft) sprite += "1";
                else sprite += "0";
            }
            else sprite += "0";

            sprite += "1";

            if (right)
            {
                if (Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
                {
                    checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                    checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                    checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                    if (checkV != null && checkV.Floor == Name) upRight = true;
                }
                if (upRight) sprite += "1";
                else sprite += "0";
            }
            else sprite += "0";
        }
        else sprite += "000";

        // Middle row
        if (left) sprite += "1";
        else sprite += "0";
        // This is for the current farm tile
        sprite += "1";
        if (right) sprite += "1";
        else sprite += "0";

        // Lower row
        bool downLeft = false;
        bool downRight = false;
        if (down)
        {
            if (left)
            {
                if (Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
                {
                    checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                    checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                    checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                    if (checkV != null && checkV.Floor == Name) downLeft = true;
                }
                if (downLeft) sprite += "1";
                else sprite += "0";
            }
            else sprite += "0";

            sprite += "1";

            if (right)
            {
                if (Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
                {
                    checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                    checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                    checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                    if (checkV != null && checkV.Floor == Name) downRight = true;
                }
                if (downRight) sprite += "1";
                else sprite += "0";
            }
            else sprite += "0";
        }
        else sprite += "000";

        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/" + Name + "/" + sprite);
        SpriteType = sprite;

        if (propagate)
        {
            if (left) UpdateFloorSprite8Positions(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (right) UpdateFloorSprite8Positions(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (down) UpdateFloorSprite8Positions(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x, Model.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (up) UpdateFloorSprite8Positions(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x, Model.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (upLeft) UpdateFloorSprite8Positions(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (upRight) UpdateFloorSprite8Positions(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (downLeft) UpdateFloorSprite8Positions(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x - 0.25f, Model.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
            if (downRight) UpdateFloorSprite8Positions(Physics2D.OverlapPoint(new Vector2(Model.transform.position.x + 0.25f, Model.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject, Name);
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

    public static List<GameObject> GetCollidingFloorsFull(GameObject floor)
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

        if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - 0.25f, floor.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            floors.Add(Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - 0.25f, floor.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject);

        if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - +.25f, floor.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            floors.Add(Physics2D.OverlapPoint(new Vector2(floor.transform.position.x + 0.25f, floor.transform.position.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject);

        if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - 0.25f, floor.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            floors.Add(Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - 0.25f, floor.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject);

        if (Physics2D.OverlapPoint(new Vector2(floor.transform.position.x - +.25f, floor.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            floors.Add(Physics2D.OverlapPoint(new Vector2(floor.transform.position.x + 0.25f, floor.transform.position.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject);

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
    
    public static void UpdateFloorSprite8Positions(GameObject floor, string floorName)
    {
        Transform t = floor.transform.Find("Vertices").Find("Vertex");
        Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
        if (v != null && v.Floor == floorName)
        {
            string sprite = "";
            Vector3 tilePos = floor.transform.position;
            GameObject checkFloor;
            Transform checkT;
            Vertex checkV;

            // Left
            bool left = false;
            if (Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y), 1 << LayerMask.NameToLayer("Floor")))
            {
                checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                if (checkV != null && checkV.Floor == floorName) left = true;
            }

            bool right = false;
            if (Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y), 1 << LayerMask.NameToLayer("Floor")))
            {
                checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                if (checkV != null && checkV.Floor == floorName) right = true;
            }

            bool up = false;
            if (Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            {
                checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x , tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                if (checkV != null && checkV.Floor == floorName) up = true;
            }

            bool down = false;
            if (Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
            {
                checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                if (checkV != null && checkV.Floor == floorName) down = true;
            }

            // Upper row
            if (up)
            {
                if (left)
                {
                    bool upLeft = false;
                    if (Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
                    {
                        checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                        checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                        checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                        if (checkV != null && checkV.Floor == floorName) upLeft = true;
                    }
                    if (upLeft) sprite += "1";
                    else sprite += "0";
                }
                else sprite += "0";

                sprite += "1";

                if (right)
                {
                    bool upRight = false;
                    if (Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")))
                    {
                        checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y + 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                        checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                        checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                        if (checkV != null && checkV.Floor == floorName) upRight = true;
                    }
                    if (upRight) sprite += "1";
                    else sprite += "0";
                }
                else sprite += "0";
            }
            else sprite += "000";

            // Middle row
            if (left) sprite += "1";
            else sprite += "0";
            // This is for the current farm tile
            sprite += "1";
            if (right) sprite += "1";
            else sprite += "0";

            // Lower row
            if (down)
            {
                if (left)
                {
                    bool downLeft = false;
                    if (Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
                    {
                        checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.25f, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                        checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                        checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                        if (checkV != null && checkV.Floor == floorName) downLeft = true;
                    }
                    if (downLeft) sprite += "1";
                    else sprite += "0";
                }
                else sprite += "0";

                sprite += "1";

                if (right)
                {
                    bool downRight = false;
                    if (Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")))
                    {
                        checkFloor = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.25f, tilePos.y - 0.25f), 1 << LayerMask.NameToLayer("Floor")).gameObject;
                        checkT = checkFloor.transform.Find("Vertices").Find("Vertex");
                        checkV = VertexSystem.VertexFromPosition(checkT.transform.position);
                        if (checkV != null && checkV.Floor == floorName) downRight = true;
                    }
                    if (downRight) sprite += "1";
                    else sprite += "0";
                }
                else sprite += "0";
            }
            else sprite += "000";

            floor.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/" + floorName + "/" + sprite);
            v.FloorType = sprite;
        }
    }
}