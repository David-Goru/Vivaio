using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeTools;
using UnityEngine.EventSystems;


public class PlayerTools : MonoBehaviour
{
    public static Tool ToolOnHand;
    public static bool DoingAnim;
    bool cancelPaused;

    // Pickable object
    GameObject pickableObject;
    Vector3 firstPos;

    // Tiles
    public static int TilesAmount;

    void Update()
    {        
        if (Input.GetKeyDown("t"))
        {
            if (GameObject.Find("UI").transform.Find("Tutorial").gameObject.activeSelf) GameObject.Find("UI").transform.Find("Tutorial").gameObject.SetActive(false);
            else GameObject.Find("UI").transform.Find("Tutorial").gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos;
            if (pickableObject != null)
            {
                mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 2.0f) / 2.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 2.0f) / 2.0f);

                if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Buildable area"))
                    && Vector2.Distance(transform.position, pickableObject.transform.position) <= 1.5f
                    && !Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
                {
                    pickableObject.transform.position = mousePos;

                    SpriteRenderer objectSprite;
                    if (pickableObject.GetComponent<SpriteRenderer>() != null) objectSprite = pickableObject.GetComponent<SpriteRenderer>();
                    else objectSprite = pickableObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
                    objectSprite.color = Color.white;
                    pickableObject.GetComponent<BoxCollider2D>().enabled = true;

                    if (pickableObject.name == "Delivery box") DeliverySystem.DeliveryList.Find(x => x.Box == pickableObject).Point.Available = true;

                    pickableObject = null;
                }
            }
            else if (!Inventory.InventorySlot.activeSelf)
            {
                mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 2.0f) / 2.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 2.0f) / 2.0f);
                
                if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable"))
                 && Vector2.Distance(transform.position, mousePos) <= 1.5f)
                {
                    pickableObject = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")).gameObject;
                    if (Vector2.Distance(transform.position, pickableObject.transform.position) <= 1.5f)
                    {
                        firstPos = pickableObject.transform.position;
                        pickableObject.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    else pickableObject = null;
                }
            } 
            else if (ToolOnHand != null && ToolOnHand.Name == "Shovel")
            {
                mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 2.0f) / 2.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 2.0f) / 2.0f);
                
                if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
                {
                    GameObject possibleTile = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")).gameObject;
                    if (possibleTile.name == "Shop tile")
                    {
                        VertexSystem.Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(possibleTile.transform.position.x, possibleTile.transform.position.y));
                        foreach (VertexSystem.Vertex u in v.Conns)
                        {
                            u.Conns.Remove(v);
                        }                        
                        VertexSystem.Vertices.Remove(v);
                        Destroy(possibleTile);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 2.0f) / 2.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 2.0f) / 2.0f);
                    
            if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")))
            {
                GameObject pot = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")).transform.parent.gameObject;
                if (Vector2.Distance(transform.position, pot.transform.position) <= 1.5f)
                {
                    Debug.Log("Pot info");
                }
            }
            else if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
            {
                GameObject objectSelected = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")).gameObject;
                if (Vector2.Distance(transform.position, objectSelected.transform.position) <= 1.5f)
                {
                    if (objectSelected.name == "Shop table")
                    {
                        Stand standObject = Stands.StandsList.Find(x => x.Model == objectSelected);

                        if (standObject.Item != null)
                        {
                            StandUI.StandObject = standObject;
                            GameObject.Find("UI").transform.Find("Stand UI").gameObject.SetActive(true);
                            (GameObject.Find("UI").transform.Find("Stand UI").GetComponent("StandUI") as StandUI).ReActivate();
                        }
                        else
                        {
                            if (Inventory.InventorySlot.activeSelf) return;

                            if (standObject.IsEmpty())
                            {
                                Stands.StandsList.Remove(standObject);
                                (GameObject.Find("Farm handler").GetComponent("Build") as Build).ObjectName = "Shop table";
                                GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(true);
                                Inventory.ChangeObject("Shop table", "Object");
                                Destroy(objectSelected);
                            }
                        }
                    }
                    else if (objectSelected.name == "Product box")
                    {                        
                        if (Inventory.InventorySlot.activeSelf) return;
                        ProductBox pb = ProductStorages.PBList.Find(x => x.Model == objectSelected);

                        if (pb.IsEmpty())
                        {
                            ProductStorages.PBList.Remove(pb);
                            (GameObject.Find("Farm handler").GetComponent("Build") as Build).ObjectName = "Product box";
                            GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(true);
                            Inventory.ChangeObject("Product box", "Object");
                            Destroy(objectSelected);
                        }
                    }
                }
            }
        }
        else if (pickableObject != null && !EventSystem.current.IsPointerOverGameObject()) 
        {
            Vector2 mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 2.0f) / 2.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 2.0f) / 2.0f);
            pickableObject.transform.position = mousePos;

            SpriteRenderer objectSprite;
            if (pickableObject.GetComponent<SpriteRenderer>() != null) objectSprite = pickableObject.GetComponent<SpriteRenderer>();
            else objectSprite = pickableObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();

            if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Buildable area"))
                && Vector2.Distance(transform.position, pickableObject.transform.position) <= 1.5f
                && !Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
                objectSprite.color = Color.green;
            else objectSprite.color = Color.red;
        }

        if (Input.GetAxisRaw("Cancel") != 0)
        {
            if (!cancelPaused)
            {
                cancelPaused = true;
                if (pickableObject != null)
                {
                    SpriteRenderer objectSprite;
                    if (pickableObject.GetComponent<SpriteRenderer>() != null) objectSprite = pickableObject.GetComponent<SpriteRenderer>();
                    else objectSprite = pickableObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();

                    pickableObject.transform.position = firstPos;
                    objectSprite.color = Color.white;
                    pickableObject.GetComponent<BoxCollider2D>().enabled = true;
                    pickableObject = null;
                }
                else
                {
                    if (Time.timeScale == 0)
                    {
                        GameObject.Find("UI").transform.Find("Pause").gameObject.SetActive(false);
                        Time.timeScale = 1;
                    }
                    else
                    {
                        Time.timeScale = 0;
                        GameObject.Find("UI").transform.Find("Pause").gameObject.SetActive(true);
                    }
                }
            }
        }
        
        if (Input.GetAxisRaw("Cancel") == 0) cancelPaused = false;

        if (Input.GetAxis("Speed") != 0) PlayerMovement.IsRunning = true;
        else PlayerMovement.IsRunning = false;
    }

    public bool IsMovingOrBuilding()
    {
        if (pickableObject != null || (GameObject.Find("Farm handler").GetComponent("Build") as Build).enabled) return true;
        return false;
    }

    public static IEnumerator DoAnim(string anim, Vector2 pos)
    {
        DoingAnim = true;

        GameObject player = GameObject.Find("Player");

        Vector2 distance = new Vector2(pos.x - player.transform.position.x,
                                        pos.y - player.transform.position.y);
        
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {   
            if (distance.x < 0)
            {
                anim += " left";
                PlayerMovement.LastDirection = "Anim left";
            }
            else
            {
                anim += " right";
                PlayerMovement.LastDirection = "Anim right";
            }
        }
        else
        {
            if (distance.y < 0)
            {
                anim += " down";
                PlayerMovement.LastDirection = "Anim down";
            }
            else 
            {
                anim += " up";
                PlayerMovement.LastDirection = "Anim up";
            }
        }

        player.GetComponent<Animator>().SetTrigger(anim);
        yield return new WaitForSeconds(0.25f);
        player.GetComponent<Animator>().ResetTrigger(anim);
        DoingAnim = false;
    }

    public void ThrowSeeds()
    {
        if (ToolOnHand.name == "Seed") ToolOnHand.LetTool();
        GameObject.Find("UI").transform.Find("Throw seeds").gameObject.SetActive(false);
    }

    public void OpenLetter()
    {
        GameObject letterUI = GameObject.Find("UI").transform.Find("Letter").gameObject;
        Letter letter = (Letter)Inventory.ObjectInHand;
        letter.Read = true;
        Inventory.ChangeObject("Letter", "Letter");
        letterUI.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + letter.Type);
        letterUI.transform.Find("Title").gameObject.GetComponent<Text>().text = letter.Title;
        letterUI.transform.Find("Body").gameObject.GetComponent<Text>().text = letter.Body;
        letterUI.SetActive(true);
    }

    public void ThrowLetter()
    {
        GameObject.Find("UI").transform.Find("Letter").gameObject.SetActive(false);
        GameObject.Find("UI").transform.Find("Open letter").gameObject.SetActive(false);
        Inventory.ChangeObject("", "None");
        Inventory.ObjectInHand = null;
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}