using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    public Sprite PlowedSoil;
    public Sprite WateredSoil;

    public static string CurrentTool;
    public static string CurrentSeed;

    // Related to the basket
    public static int CropsInBasket;
    public static string TypeInBasket;

    void Start()
    {
        CurrentTool = "None";
        CurrentSeed = "None";

        CropsInBasket = 0;
        TypeInBasket = "None";
    }

    void Update()
    {
        Vector2 mousePos;
        switch (CurrentTool)
        {
            case "Hoe":
                mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 1.0f) / 1.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.0f) / 1.0f);
                if (Input.GetMouseButtonDown(0) && Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Farmland")))
                {
                    GameObject farmFloor = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Farmland")).transform.gameObject;
                    if (Vector2.Distance(transform.position, farmFloor.transform.position) <= 1.5f && farmFloor.name == "Grass")
                    {
                        farmFloor.name = "Plowed soil";
                        updateFarmland(farmFloor);
                        foreach (Collider2D col in Physics2D.OverlapCircleAll(farmFloor.transform.position, 1.5f))
                        {
                            if (col.gameObject.name == "Plowed soil") updateFarmland(col.gameObject);
                        }
                    }
                }
                break;
            case "Watering can":
                mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 1.0f) / 1.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.0f) / 1.0f);
                if (Input.GetMouseButtonDown(0) && Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")))
                {
                    GameObject pot = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")).transform.parent.gameObject;
                    if (Vector2.Distance(transform.position, pot.transform.position) <= 1.5f)
                    {
                        Farm.SimpleCrop plant = Farm.SimpleCrops.Find(x => x.Pot == pot);
                        if (plant.Water()) Debug.Log("Was already watered");
                    }
                }
                break;
            case "Seed":
                mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 1.0f) / 1.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.0f) / 1.0f);
                if (Input.GetMouseButtonDown(0) && Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Farmland")) && !Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")))
                {
                    GameObject pot = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Farmland")).gameObject;
                    if (Vector2.Distance(transform.position, pot.transform.position) <= 1.5f && pot.name == "Plowed soil")
                    {
                        switch (CurrentSeed)
                        {
                            case "Tomato":
                                Farm.SimpleCrops.Add(new Farm.SimpleCrop(CurrentSeed, 2, 8, 5, pot));
                                break;
                            case "Carrot":
                                Farm.SimpleCrops.Add(new Farm.SimpleCrop(CurrentSeed, 2, 5, 3, pot));
                                break;
                            case "Potato":
                                Farm.SimpleCrops.Add(new Farm.SimpleCrop(CurrentSeed, 2, 4, 2, pot));
                                break;
                        }

                    }
                }
                break;
            case "Shovel":
                mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 1.0f) / 1.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.0f) / 1.0f);

                if (Input.GetMouseButtonDown(0) && Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")))
                {
                    GameObject pot = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")).transform.parent.gameObject;
                    if (Vector2.Distance(transform.position, pot.transform.position) <= 1.5f)
                    {
                        Farm.SimpleCrop plant = Farm.SimpleCrops.Find(x => x.Pot == pot);
                        plant.Delete();
                    }
                }
                break;
            case "Basket":
                mousePos = new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x * 1.0f) / 1.0f, Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.0f) / 1.0f);

                if (Input.GetMouseButtonDown(0) && Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")))
                {
                    GameObject pot = Physics2D.OverlapPoint(mousePos, 1 << LayerMask.NameToLayer("Plant")).transform.parent.gameObject;
                    if (Vector2.Distance(transform.position, pot.transform.position) <= 1.5f)
                    {
                        Farm.SimpleCrop plant = Farm.SimpleCrops.Find(x => x.Pot == pot);
                        plant.Harvest();
                    }
                }
                break;
        }   
    }

    public void TakeTool(string tool)
    {
        if (CurrentTool == "Basket") Inventory.ChangeSubobject("", "None");
        if (tool == "Build")
        {
            CurrentTool = tool;
            Inventory.ChangeObject("", "None");
            GameObject.Find("UI").transform.Find("Build").gameObject.SetActive(true);
        }
        else
        {
            if (CurrentTool == "Build") (GameObject.Find("UI").transform.Find("Build").gameObject.GetComponent("Build") as Build).EndBuildMode();
            CurrentTool = tool;
            Inventory.ChangeObject(tool, "Tool");
        }
    }

    public void RemoveTool()
    {
        CurrentTool = "None";
        Inventory.ChangeObject("", "None");
    }

    public void TakeSeed(string seed)
    {
        CurrentTool = "Seed";
        CurrentSeed = seed;
        Inventory.ChangeObject(seed, "Seed");
    }

    public void RemoveSeed()
    {
        CurrentTool = "None";
        CurrentSeed = "None";
        Inventory.ChangeObject("", "None");
    }

    void updateFarmland(GameObject farmFloor)
    {
        Vector3 tilePos = farmFloor.transform.position;
        string sprite = "";
        Collider2D currentTile;

        // Check left and right columns
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 1, tilePos.y), 1 << LayerMask.NameToLayer("Farmland"));
        bool left = currentTile && currentTile.gameObject.name == "Plowed soil";
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 1, tilePos.y), 1 << LayerMask.NameToLayer("Farmland"));
        bool right = currentTile && currentTile.gameObject.name == "Plowed soil";

        // Upper row
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y + 1), 1 << LayerMask.NameToLayer("Farmland"));
        if (currentTile && currentTile.gameObject.name == "Plowed soil")
        {
            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 1, tilePos.y + 1), 1 << LayerMask.NameToLayer("Farmland"));
            if (currentTile && currentTile.gameObject.name == "Plowed soil" && left) sprite += "1";
            else sprite += "0";

            sprite += "1";

            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 1, tilePos.y + 1), 1 << LayerMask.NameToLayer("Farmland"));
            if (currentTile && currentTile.gameObject.name == "Plowed soil" && right) sprite += "1";
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
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y - 1), 1 << LayerMask.NameToLayer("Farmland"));
        if (currentTile && currentTile.gameObject.name == "Plowed soil")
        {
            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 1, tilePos.y - 1), 1 << LayerMask.NameToLayer("Farmland"));
            if (currentTile && currentTile.gameObject.name == "Plowed soil" && left) sprite += "1";
            else sprite += "0";

            sprite += "1";

            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 1, tilePos.y - 1), 1 << LayerMask.NameToLayer("Farmland"));
            if (currentTile && currentTile.gameObject.name == "Plowed soil" && right) sprite += "1";
            else sprite += "0";
        }
        else sprite += "000";
        
        farmFloor.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Plowed soil/" + sprite);
    }
}