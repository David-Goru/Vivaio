using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Build : MonoBehaviour
{
    GameObject objectBP; // Object blueprint
    Vector2 lastPos;
    bool buildable;

    void Update()
    {
        if (Inventory.ObjectInHand is BuildableObject)
        {
            BuildableObject bo = (BuildableObject)Inventory.ObjectInHand;
            Vector2 mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 2.0f) / 2.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 2.0f) / 2.0f);
            Vector2 pos = new Vector3(Mathf.Round(mousePos.x * 2.0f) / 2.0f, Mathf.Round(mousePos.y * 2.0f) / 2.0f); // Exact position

            if (lastPos != pos)
            {
                if (objectBP == null) objectBP = (GameObject)Instantiate(Resources.Load("Objects/" + Inventory.ObjectInHand.Name), pos, Quaternion.Euler(0, 0, 0));

                if (Vector2.Distance(pos, GameObject.Find("Player").transform.position) < 2
                    && Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Buildable area")) 
                    && !Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Pickable")))
                {
                    objectBP.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
                    buildable = true;
                }
                else
                {
                    objectBP.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                    buildable = false;
                }
                objectBP.transform.position = pos;
                lastPos = pos;
            }

            if (Input.GetMouseButtonDown(0) && buildable && !EventSystem.current.IsPointerOverGameObject())
            {
                objectBP.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

                if (bo.Name == "Shop tile")
                {
                    VertexSystem.Vertex v = new VertexSystem.Vertex(pos);
                    VertexSystem.Vertices.Add(v);
                    v.UpdateCons();
                    objectBP.name = "Shop tile";
                    objectBP.GetComponent<BoxCollider2D>().enabled = true;
                    buildable = false;
                    bo.Amount--;
                    if (bo.Amount > 0)
                    {
                        Inventory.ChangeObject();
                        objectBP = null;
                        return;
                    }
                }
                else if (bo.Name == "Shop table")
                {
                    Stands.StandsList.Add(new Stand(objectBP, 10));
                    objectBP.name = "Shop table";
                    objectBP.GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (bo.Name == "Storage box")
                {
                    objectBP.name = "Storage box";
                    objectBP.GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (bo.Name == "Product box")
                {
                    ProductStorages.PBList.Add(new ProductBox(objectBP, 50));
                    objectBP.name = "Product box";
                    objectBP.GetComponent<BoxCollider2D>().enabled = true;
                }
                objectBP = null;
                lastPos = new Vector2(-1000, -1000);
                Inventory.RemoveObject();
                GameObject.Find("UI").transform.Find("Cancel build button").gameObject.SetActive(false);
                this.enabled = false;
            }
        }
    }

    public void CancelBuild()
    {
        GameObject.Find("UI").transform.Find("Cancel build button").gameObject.SetActive(false);
        GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(true);
        Destroy(objectBP);
        lastPos = new Vector2(-1000, -1000);
        this.enabled = false;
    }
}