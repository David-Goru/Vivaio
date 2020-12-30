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
        if (Input.GetKeyDown(KeyCode.R) && boInfo != null && physicalObject != null) boInfo.RotateObject();

        checkPosition(new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 4.0f) / 4.0f,     // Vertex position X
                                  Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 4.0f) / 4.0f));   // Vertex position Y

        if (Input.GetMouseButtonDown(0) && buildable && !EventSystem.current.IsPointerOverGameObject()) placeObject();  // Build/place object
    }

    void checkPosition(Vector2 pos)
    {
        // Object still not placed
        if (physicalObject == null)
        {
            physicalObject = Instantiate(Resources.Load<GameObject>("Objects/" + Inventory.Data.ObjectInHand.Name), pos, Quaternion.Euler(0, 0, 0));
            boInfo.Model = physicalObject;

            if (boInfo is Gate) physicalObject.transform.Find("R button").gameObject.SetActive(true);
        }

        // Check if object can be built at position
        if (Vector2.Distance(pos, GameObject.Find("Player").transform.position) < BuildRange)
        {
            buildable = true;            

            if (physicalObject.CompareTag("Water pump") && pos.x != 7) buildable = false;
            else if (boInfo.Name == "Water bottling machine" && pos.x != 7) buildable = false;
            else
            {                
                Transform tParent;
                tParent = physicalObject.transform.Find("Vertices");
                foreach (Transform t in tParent)
                {
                    Vertex v = VertexSystem.VertexFromPosition(t.transform.position);

                    if (v == null || v.State == VertexState.NotUsable) buildable = false;
                    else if (boInfo is Floor)
                    {
                        if (v.Floor != "None") buildable = false;
                    }
                    else if (v.State == VertexState.Occuppied || v.State == VertexState.Walkable) buildable = false;
                }
            }

            if (boInfo is Wall) ((Wall)boInfo).CheckRotation();
            else if (boInfo.Name == "Composite tile") ((Floor)boInfo).CheckRotation();
            else if (boInfo.Name == "Dirt tile") ((Floor)boInfo).CheckFullRotation();

            if (boInfo is Gate) physicalObject.transform.Find("Rotation " + boInfo.Rotation).Find(((Gate)boInfo).Opened ? "Open" : "Closed").gameObject.GetComponent<SpriteRenderer>().color = buildable ? Color.green : Color.red;
            else physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = buildable ? Color.green : Color.red;
        }
        else
        {
            buildable = false;
            
            if (boInfo is Gate) physicalObject.transform.Find("Rotation " + boInfo.Rotation).Find(((Gate)boInfo).Opened ? "Open" : "Closed").gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            else physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        
        // Update position values
        physicalObject.transform.position = pos;
    }

    void placeObject()
    {
        Master.RunSoundStatic(SoundsHandler.PlaceObjectStatic);

        // Update color
        if (boInfo is Gate) physicalObject.transform.Find("Rotation " + boInfo.Rotation).Find(((Gate)boInfo).Opened ? "Open" : "Closed").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        else physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        if (boInfo is Gate) physicalObject.transform.Find("Rotation " + boInfo.Rotation).Find(((Gate)boInfo).Opened ? "Open" : "Closed").Find("Obstacle").gameObject.SetActive(true);
        else if (physicalObject.transform.Find("Obstacle") != null) physicalObject.transform.Find("Obstacle").gameObject.SetActive(true);
        
        Transform tParent;
        tParent = physicalObject.transform.Find("Vertices");
        Vertex v = null;
        foreach (Transform t in tParent)
        {                            
            v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null)
            {
                if (boInfo is Floor)
                {
                    v.Floor = boInfo.Name;
                    v.FloorType = ((Floor)boInfo).SpriteType;
                }
                else if (boInfo is House)
                {
                    v.State = VertexState.Occuppied;
                    v.Floor = "House ground";
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
                if (boInfo.Name == "Composite tile") ((Floor)boInfo).CheckRotation(true);
                else if (boInfo.Name == "Dirt tile") ((Floor)boInfo).CheckFullRotation(true);
                
                boInfo.Stack--;
                if (boInfo.Stack > 0)
                {
                    Inventory.ChangeObject();
                    physicalObject = null;
                    return;
                }
            }

            if (boInfo.Name == "Fence gate") physicalObject.transform.Find("R button").gameObject.SetActive(false);

            boInfo.OnObjectPlaced(physicalObject);
            switch (boInfo.Name)
            {
                case "Small vegetables stand":
                case "Big vegetables stand":
                case "Food stand":
                case "Beverages stand": // Can be optimized
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
                case "Cash register":
                    boInfo.Model = physicalObject;
                    ObjectsHandler.Data.Objects.Add(boInfo);
                    boInfo.Placed = true;

                    CashRegister cr = (CashRegister)boInfo;                
                    cr.CustomerPos = new List<Vector2>();
                    foreach (Transform t in physicalObject.transform.Find("Customer position"))
                    {
                        cr.CustomerPos.Add(t.position);
                    }
                    Master.Data.CashRegisters.Add((CashRegister)boInfo);
                    break;
                case "Bottles recycler":
                    boInfo.Model = physicalObject;
                    ObjectsHandler.Data.Objects.Add(boInfo);
                    boInfo.Placed = true;

                    BottlesRecycler br = (BottlesRecycler)boInfo;                
                    br.CustomerPos = new List<Vector2>();
                    foreach (Transform t in physicalObject.transform.Find("Customer position"))
                    {
                        br.CustomerPos.Add(t.position);
                    }
                    Master.Data.BottlesRecyclers.Add((BottlesRecycler)boInfo);
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
                case "Water bottling machine":
                case "Garbage can":
                case "Outdoor light":
                case "House":
                    boInfo.Model = physicalObject;
                    ObjectsHandler.Data.Objects.Add(boInfo);
                    boInfo.Placed = true;
                    break;
                case "Fence":
                    Wall fence = new Wall("Fence", 1, 10, "Fence");
                    fence.Model = physicalObject;
                    fence.Placed = true;
                    ObjectsHandler.Data.Objects.Add(fence);
                    fence.WorldPosition = physicalObject.transform.position;
                    fence.CheckRotation(true);

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
                CashRegister cr = (CashRegister)boInfo;                
                cr.CustomerPos = new List<Vector2>();
                foreach (Transform t in physicalObject.transform.Find("Customer position"))
                {
                    cr.CustomerPos.Add(t.position);
                }
            }
            else if (boInfo is Wall)
            {
                Wall fence = (Wall)boInfo;
                List<Wall> wallsToCheck = fence.GetCollidingWalls();
                fence.WorldPosition = physicalObject.transform.position;
                fence.CheckRotation(true);
                foreach (Wall w in wallsToCheck)
                {
                    w.CheckRotation();
                }
            }
            else if (boInfo is Gate) physicalObject.transform.Find("R button").gameObject.SetActive(false);
        }

        if (boInfo != null)
        {
            boInfo.WorldPosition = physicalObject.transform.position;
            boInfo = null;
        }
        UI.Elements["Cancel build button"].SetActive(false);  
        physicalObject = null;
        this.enabled = false;
    }

    public void StartBuild(GameObject objectToMove)
    {
        Master.RunSoundStatic(SoundsHandler.MoveObjectStatic);
        physicalObject = objectToMove;
        savedPos = objectToMove.transform.position;        
        objectToMove.GetComponent<BoxCollider2D>().enabled = false;
        
        IObject bo = ObjectsHandler.Data.Objects.Find(x => x.Model == objectToMove);
        boInfo = (BuildableObject)bo;

        if (boInfo is Gate) 
        {
            physicalObject.transform.Find("Rotation " + boInfo.Rotation).Find(((Gate)boInfo).Opened ? "Open" : "Closed").Find("Obstacle").gameObject.SetActive(false);
            physicalObject.transform.Find("R button").gameObject.SetActive(true);
        }
        else if (physicalObject.transform.Find("Obstacle") != null) physicalObject.transform.Find("Obstacle").gameObject.SetActive(false);
        
        Transform tParent;
        tParent = physicalObject.transform.Find("Vertices");        
        foreach (Transform t in tParent)
        {                            
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null)
            {
                if (boInfo is Box && v.Floor == "House ground") continue;
                v.State = VertexState.Available;
            }
        }

        isMoving = true;
        this.enabled = true;
        UI.Elements["Cancel build button"].SetActive(true);
    }

    public void StartBuild(GameObject objectToBuild, BuildableObject bo)
    {
        physicalObject = objectToBuild;
        if (bo is Gate) physicalObject.transform.Find("R button").gameObject.SetActive(true);
        boInfo = bo;
        boInfo.Model = objectToBuild;
        isMoving = false;
        this.enabled = true;
        UI.Elements["Cancel build button"].SetActive(true);
        UI.Elements["Build object button"].SetActive(false);
    }

    public void CancelBuild()
    {
        UI.Elements["Cancel build button"].SetActive(false);

        if (isMoving)
        {
            if (boInfo is Gate)
            {
                physicalObject.transform.Find("Rotation " + boInfo.Rotation).Find(((Gate)boInfo).Opened ? "Open" : "Closed").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                physicalObject.transform.Find("R button").gameObject.SetActive(false);
            }
            else physicalObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            physicalObject.GetComponent<BoxCollider2D>().enabled = true;
            physicalObject.transform.position = savedPos;  

            if (boInfo is Gate) physicalObject.transform.Find("Rotation " + boInfo.Rotation).Find(((Gate)boInfo).Opened ? "Open" : "Closed").Find("Obstacle").gameObject.SetActive(true);
            else if (physicalObject.transform.Find("Obstacle") != null) physicalObject.transform.Find("Obstacle").gameObject.SetActive(true);

            Transform tParent;  
            tParent = physicalObject.transform.Find("Vertices");            
            foreach (Transform t in tParent)
            {                            
                Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
                if (v != null) v.State = VertexState.Occuppied;
            }
            physicalObject = null;
        }
        else
        {
            UI.Elements["Build object button"].SetActive(true);
            boInfo.Model = null;
            Destroy(physicalObject);
        }

        this.enabled = false;
    }
}