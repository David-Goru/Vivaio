using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Build : MonoBehaviour
{
    string objectName;
    GameObject objectBP; // Object blueprint
    Vector2 lastPos;

    void Start()
    {
        objectName = "None";
    }

    void Update()
    {
        if (objectName != "None")
        {
            Vector2 mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 1.0f) / 1.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.0f) / 1.0f);
            Vector2 pos = new Vector3(Mathf.Round(mousePos.x * 1.0f) / 1.0f, Mathf.Round(mousePos.y * 1.0f) / 1.0f); // Exact position

            if (Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Buildable area")) && !Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Object")))
            {
                if (objectBP == null)
                {
                    objectBP = (GameObject)Instantiate(Resources.Load("Objects/" + objectName), pos, Quaternion.Euler(0, 0, 0));
                    objectBP.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
                    objectBP.transform.position = pos;
                    lastPos = pos;
                }
                else if (lastPos != pos)
                {
                    objectBP.transform.position = pos;
                    lastPos = pos;
                }
            }
            else if (lastPos != pos && objectBP) Destroy(objectBP);

            if (Input.GetMouseButtonDown(0) && objectBP && !EventSystem.current.IsPointerOverGameObject())
            {
                objectBP.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                objectBP = null;
            }
        }
    }

    public void EndBuildMode()
    {
        if (objectBP) Destroy(objectBP);
        objectName = "None";
        gameObject.SetActive(false);
    }

    public void ChooseObject(string name)
    {
        objectName = name;
    }
}