using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class Farm : MonoBehaviour
{
    public static Dictionary<string, Plant> Plants;

    public static List<Crop> Crops;

    public static Sprite UnwateredSeed;
    public static Sprite WateredSeed;

    void Start()
    {
        Plants = new Dictionary<string, Plant>();
        GetPlants();

        UnwateredSeed = Resources.Load<Sprite>("Crops/Unwatered seed planted");
        WateredSeed = Resources.Load<Sprite>("Crops/Watered seed planted");

        Crops = new List<Crop>();
    }

    public void GetPlants()
    {
        XmlDocument plantsDoc = new XmlDocument();
        plantsDoc.Load(Application.dataPath + "/Data/Plants.xml");
        XmlNodeList plants = plantsDoc.GetElementsByTagName("Plant");
        
        foreach (XmlNode plant in plants)
        {              
            Plant currentPlant = new Plant(plant["Name"].InnerText, int.Parse(plant["Levels"].InnerText), int.Parse(plant["MinAmount"].InnerText), int.Parse(plant["MaxAmount"].InnerText), int.Parse(plant["DaysUntilDry"].InnerText));
            Plants.Add(plant["Name"].InnerText, currentPlant);
        }
    }

    public static void UpdateFarm(GameObject farmFloor)
    {
        Vector3 tilePos = farmFloor.transform.position;
        string sprite = "";
        Collider2D currentTile;

        // Check left and right columns
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.5f, tilePos.y), 1 << LayerMask.NameToLayer("Farmland"));
        bool left = currentTile && currentTile.gameObject.name == "Plowed soil";
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.5f, tilePos.y), 1 << LayerMask.NameToLayer("Farmland"));
        bool right = currentTile && currentTile.gameObject.name == "Plowed soil";

        // Upper row
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y + 0.5f), 1 << LayerMask.NameToLayer("Farmland"));
        if (currentTile && currentTile.gameObject.name == "Plowed soil")
        {
            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.5f, tilePos.y + 0.5f), 1 << LayerMask.NameToLayer("Farmland"));
            if (currentTile && currentTile.gameObject.name == "Plowed soil" && left) sprite += "1";
            else sprite += "0";

            sprite += "1";

            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f), 1 << LayerMask.NameToLayer("Farmland"));
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
        currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x, tilePos.y - 0.5f), 1 << LayerMask.NameToLayer("Farmland"));
        if (currentTile && currentTile.gameObject.name == "Plowed soil")
        {
            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x - 0.5f, tilePos.y - 0.5f), 1 << LayerMask.NameToLayer("Farmland"));
            if (currentTile && currentTile.gameObject.name == "Plowed soil" && left) sprite += "1";
            else sprite += "0";

            sprite += "1";

            currentTile = Physics2D.OverlapPoint(new Vector2(tilePos.x + 0.5f, tilePos.y - 0.5f), 1 << LayerMask.NameToLayer("Farmland"));
            if (currentTile && currentTile.gameObject.name == "Plowed soil" && right) sprite += "1";
            else sprite += "0";
        }
        else sprite += "000";
        
        farmFloor.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Plowed soil/" + sprite);
    }
}