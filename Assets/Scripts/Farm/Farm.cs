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

        UnwateredSeed = Resources.Load<Sprite>("Unwatered seed planted");
        WateredSeed = Resources.Load<Sprite>("Watered seed planted");

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
}