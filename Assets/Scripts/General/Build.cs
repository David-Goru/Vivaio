using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Build : MonoBehaviour
{
    public float BuildRange;
    GameObject physicalObject;
    BuildableObject boInfo; // Only for buildingg objects
    Vector2 savedPos; // Only for moving objects
    bool buildable;
    bool isMoving;

    void Update()
    {
        checkPosition(new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 4.0f) / 4.0f,     // Vertex position X
                                  Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 4.0f) / 4.0f));   // Vertex position Y

        if (Input.GetMouseButtonDown(0) && buildable && !EventSystem.current.IsPointerOverGameObject()) placeObject();  // Build/place object

        if (Input.GetKeyDown(KeyCode.R) && boInfo != null) updateRotation();
    }

    void updateRotation()
    {
        if (physicalObject == null || !boInfo.CanRot) return;
        
        boInfo.Rotation++;
        ObjectInfo oi = Resources.Load<ObjectInfo>("Objects info/" + boInfo.Name);
        if (boInfo.Rotation == oi.RotPositions) boInfo.Rotation = 0;
        
        physicalObject.GetComponent<BoxCollider2D>().offset = oi.CollOffset[boInfo.Rotation + (boInfo is Gate ? (((Gate)boInfo).Opened ? 4 : 0) : 0)];
        physicalObject.GetComponent<BoxCollider2D>().size = oi.CollSize[boInfo.Rotation + (boInfo is Gate ? (((Gate)boInfo).Opened ? 4 : 0) : 0)];

        if (boInfo is Gate) physicalObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = oi.Sprites[boInfo.Rotation + (((Gate)boInfo).Opened ? 4 : 0)];
        else physicalObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = oi.Sprites[boInfo.Rotation];
    }

    void checkPosition(Vector2 pos)
    {
        // Object still not placed
        if (physicalObject == null)
        {
            physicalObject = Instantiate(Resources.Load<GameObject>("Objects/" + Inventory.Data.ObjectInHand.Name), pos, Quaternion.Euler(0, 0, 0));
            boInfo.Model = physicalObject;
            if (boInfo is Gate) physicalObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<ObjectInfo>("Objects info/" + boInfo.Name).Sprites[boInfo.Rotation + (((Gate)boInfo).Opened ? 4 : 0)];
            else physicalObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<ObjectInfo>("Objects info/" + boInfo.Name).Sprites[boInfo.Rotation];
        }

        // Check if object can be built at position
        if (Vector2.Distance(pos, GameObject.Find("Player").transform.position) < BuildRange)
        {
            buildable = true;            

            if (physicalObject.CompareTag("Cash register") && pos.x > -9.7f) buildable = false;
            else if (physicalObject.CompareTag("Water pump") && pos.x != -3.25f) buildable = false;
            else
            {                
                Transform tParent;
                if (boInfo.CanRot && physicalObject.transform.Find("Vertices " + boInfo.Rotation)) tParent = physicalObject.transform.Find("Vertices " + boInfo.Rotation);
                else tParent = physicalObject.transform.Find("Vertices");
                foreach (Transform t in tParent)
                {
                    Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));

                    if (v == null) buildable = false;
                    else if (boInfo is Floor)
                    {
                        if (v.Floor != "None") buildable = false;
                    }
                    else if (v.State == VertexState.Occuppied || v.State == VertexState.Walkable) buildable = false;
                }
            }

            if (buildable) physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            else physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            buildable = false;
            physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        
        // Update position values
        physicalObject.transform.position = pos;
    }

    void placeObject()
    {
        // Update color
        physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        
        if (physicalObject.transform.Find("Obstacle 0") != null) physicalObject.transform.Find("Obstacle " + (boInfo.Rotation + (boInfo is Gate ? (((Gate)boInfo).Opened ? 4 : 0) : 0))).gameObject.SetActive(true);
        else if (physicalObject.transform.Find("Obstacle") != null) physicalObject.transform.Find("Obstacle").gameObject.SetActive(true);

        Transform tParent;
        if (boInfo.CanRot && physicalObject.transform.Find("Vertices " + boInfo.Rotation)) tParent = physicalObject.transform.Find("Vertices " + (boInfo.Rotation + (boInfo is Gate ? (((Gate)boInfo).Opened ? 4 : 0) : 0)));
        else tParent = physicalObject.transform.Find("Vertices");
        foreach (Transform t in tParent)
        {                            
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            if (v != null)
            {
                if (boInfo is Floor)
                {
                    v.Floor = boInfo.Name;
                    v.Rot = boInfo.Rotation;
                }
                else v.State = VertexState.Occuppied;
            }
        }

        if (!isMoving)
        {
            physicalObject.name = boInfo.Name;
            physicalObject.GetComponent<BoxCollider2D>().enabled = true;

            if (boInfo is Floor)
            {
                boInfo.Stack--;
                if (boInfo.Stack > 0)
                {
                    Inventory.ChangeObject();
                    physicalObject = null;
                    return;
                }
            }

            switch (boInfo.Name)
            {
                case "Shop table":
                case "Shop box": // Can be optimized
                    boInfo.Model = physicalObject;
                    ObjectsHandler.Data.Objects.Add(boInfo);
                    boInfo.Placed = true;

                    Stand s = (Stand)boInfo;
                    s.CustomerPos = new List<Vector2>();
                    foreach (Transform t in physicalObject.transform.Find("Customer position"))
                    {
                        s.CustomerPos.Add(t.position);
                    }

                    Master.Data.Stands.Add((Stand)boInfo);
                    break;                
                case "Water pump": // Very optimizable
                    boInfo.Model = physicalObject;
                    ObjectsHandler.Data.Objects.Add(boInfo);
                    boInfo.Placed = true;

                    foreach (PlowedSoil p in Farm.PlowedSoils)
                    {
                        p.CheckDripIrrigationWarning(false);
                    }
                    break;
                case "Product box":
                case "Composter":
                case "Storage box":
                case "Seed box":
                case "Deseeding machine":
                case "Flour machine":
                case "Bread machine":
                case "Furnace":
                case "Sign":
                case "Fence gate":
                    boInfo.Model = physicalObject;
                    ObjectsHandler.Data.Objects.Add(boInfo);
                    boInfo.Placed = true;
                    break;
                case "Fence":
                    Wall fence = new Wall("Fence", 1, 10);
                    fence.Model = physicalObject;
                    fence.Placed = true;
                    fence.Rotation = boInfo.Rotation;
                    ObjectsHandler.Data.Objects.Add(fence);
                    fence.WorldPosition = physicalObject.transform.position;

                    boInfo.Stack--;
                    if (boInfo.Stack > 0)
                    {
                        Inventory.ChangeObject();
                        physicalObject = null;
                        return;
                    }
                    break;
            }

            Inventory.RemoveObject();          
        }
        else 
        {
            physicalObject.GetComponent<BoxCollider2D>().enabled = true;
            if (boInfo.Name == "Delivery box" || boInfo.Name == "Present box") ((Box)ObjectsHandler.Data.Objects.Find(x => x.Model == physicalObject)).UpdatePoint();
            else if (boInfo is Stand) 
            {
                Stand s = (Stand)boInfo;                
                s.CustomerPos = new List<Vector2>();
                foreach (Transform t in physicalObject.transform.Find("Customer position"))
                {
                    s.CustomerPos.Add(t.position);
                }
            }
            else if (boInfo.Name == "Cash register")
            {                             
                CashRegister.CustomerPos = new List<Vector2>();
                foreach (Transform t in physicalObject.transform.Find("Customer position"))
                {
                    CashRegister.CustomerPos.Add(t.position);
                }
            }            
        }

        if (boInfo != null)
        {
            boInfo.WorldPosition = physicalObject.transform.position;
            boInfo = null;
        }
        GameObject.Find("UI").transform.Find("Cancel build button").gameObject.SetActive(false);  
        physicalObject = null;
        this.enabled = false;
    }

    public void StartBuild(GameObject objectToMove)
    {
        physicalObject = objectToMove;
        savedPos = objectToMove.transform.position;        
        objectToMove.GetComponent<BoxCollider2D>().enabled = false;
        
        IObject bo = ObjectsHandler.Data.Objects.Find(x => x.Model == objectToMove);
        boInfo = (BuildableObject)bo;

        if (physicalObject.transform.Find("Obstacle 0") != null) physicalObject.transform.Find("Obstacle " + (boInfo.Rotation + (boInfo is Gate ? (((Gate)boInfo).Opened ? 4 : 0) : 0))).gameObject.SetActive(false);
        else if (physicalObject.transform.Find("Obstacle") != null) physicalObject.transform.Find("Obstacle").gameObject.SetActive(false);
        
        Transform tParent;
        if (boInfo.CanRot && physicalObject.transform.Find("Vertices " + boInfo.Rotation)) tParent = physicalObject.transform.Find("Vertices " + (boInfo.Rotation + (boInfo is Gate ? (((Gate)boInfo).Opened ? 4 : 0) : 0)));
        else tParent = physicalObject.transform.Find("Vertices");
        foreach (Transform t in tParent)
        {                            
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            if (v != null) v.State = VertexState.Available;
        }

        isMoving = true;
        this.enabled = true;
        GameObject.Find("UI").transform.Find("Cancel build button").gameObject.SetActive(true);
    }

    public void StartBuild(GameObject objectToBuild, BuildableObject bo)
    {
        physicalObject = objectToBuild;
        boInfo = bo;
        isMoving = false;
        this.enabled = true;
        GameObject.Find("UI").transform.Find("Cancel build button").gameObject.SetActive(true);
        GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(false);
    }

    public void CancelBuild()
    {
        GameObject.Find("UI").transform.Find("Cancel build button").gameObject.SetActive(false);

        if (isMoving)
        {
            physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            physicalObject.GetComponent<BoxCollider2D>().enabled = true;
            
            if (physicalObject.transform.Find("Obstacle 0") != null) physicalObject.transform.Find("Obstacle " + (boInfo.Rotation + (boInfo is Gate ? (((Gate)boInfo).Opened ? 4 : 0) : 0))).gameObject.SetActive(true);  
            else if (physicalObject.transform.Find("Obstacle") != null) physicalObject.transform.Find("Obstacle").gameObject.SetActive(true);  

            physicalObject.transform.position = savedPos;  

            Transform tParent;
            if (boInfo.CanRot && physicalObject.transform.Find("Vertices " + boInfo.Rotation)) tParent = physicalObject.transform.Find("Vertices " + (boInfo.Rotation + (boInfo is Gate ? (((Gate)boInfo).Opened ? 4 : 0) : 0)));
            else tParent = physicalObject.transform.Find("Vertices");
            foreach (Transform t in tParent)
            {                            
                Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
                if (v != null) v.State = VertexState.Occuppied;
            }
            physicalObject = null;
        }
        else
        {
            GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(true);
            boInfo.Model = null;
            Destroy(physicalObject);
        }

        this.enabled = false;
    }
}