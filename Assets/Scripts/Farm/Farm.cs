using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class Farm : MonoBehaviour
{
    public static Dictionary<string, Plant> Plants;
    public static Sprite SeedPlanted;

    public static List<PlowedSoil> PlowedSoils;

    // When loading a game
    public static bool Load(FarmData data)
    {
        try
        {
            Plants = new Dictionary<string, Plant>();
            GetPlants();

            SeedPlanted = Resources.Load<Sprite>("Crops/Seed planted");

            PlowedSoils = new List<PlowedSoil>();

            foreach (PlowedSoilData p in data.PlowedSoils)
            {
                p.Load(Instantiate(Resources.Load<GameObject>("Farm/Plowed soil"), p.Pos, Quaternion.Euler(0, 0, 0)));
            }
        }
        catch (Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "Farm", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Plants = new Dictionary<string, Plant>();
        GetPlants();

        SeedPlanted = Resources.Load<Sprite>("Crops/Seed planted");

        PlowedSoils = new List<PlowedSoil>();

        return true;
    }

    // When saving the game
    public static FarmData Save()
    {
        FarmData data = new FarmData(new List<PlowedSoilData>());
        foreach (PlowedSoil p in PlowedSoils)
        {
            data.PlowedSoils.Add(p.Save());
        }
        return data;
    }

    public static void GetPlants()
    {
        XmlDocument plantsDoc = new XmlDocument();
        plantsDoc.Load(Application.dataPath + "/Data/Plants.xml");
        XmlNodeList plants = plantsDoc.GetElementsByTagName("Plant");
        
        foreach (XmlNode plant in plants)
        {              
            Plant currentPlant = new Plant(plant["Name"].InnerText, int.Parse(plant["Levels"].InnerText), int.Parse(plant["MinAmount"].InnerText), int.Parse(plant["MaxAmount"].InnerText), int.Parse(plant["FertilizerExtra"].InnerText), int.Parse(plant["DaysUntilDry"].InnerText));
            Plants.Add(plant["Name"].InnerText, currentPlant);
        }
    }

    public static void UpdateFarm(GameObject farmFloor)
    {
        Vector3 tilePos = farmFloor.transform.position;
        string sprite = "";
        Collider2D currentTile;

        // Check left and right columns
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.5f, tilePos.y), 1 << LayerMask.NameToLayer("Plowed soil"));
        bool left = currentTile;
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.5f, tilePos.y), 1 << LayerMask.NameToLayer("Plowed soil"));
        bool right = currentTile;

        // Upper row
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y + 0.5f), 1 << LayerMask.NameToLayer("Plowed soil"));
        if (currentTile)
        {
            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.5f, tilePos.y + 0.5f), 1 << LayerMask.NameToLayer("Plowed soil"));
            if (currentTile && left) sprite += "1";
            else sprite += "0";

            sprite += "1";

            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f), 1 << LayerMask.NameToLayer("Plowed soil"));
            if (currentTile && right) sprite += "1";
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
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y - 0.5f), 1 << LayerMask.NameToLayer("Plowed soil"));
        if (currentTile)
        {
            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.5f, tilePos.y - 0.5f), 1 << LayerMask.NameToLayer("Plowed soil"));
            if (currentTile && left) sprite += "1";
            else sprite += "0";

            sprite += "1";

            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.5f, tilePos.y - 0.5f), 1 << LayerMask.NameToLayer("Plowed soil"));
            if (currentTile && right) sprite += "1";
            else sprite += "0";
        }
        else sprite += "000";
        
        farmFloor.GetComponent<PlowedSoil>().SetSprite(sprite);
    }
}