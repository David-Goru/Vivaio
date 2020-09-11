using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Management : MonoBehaviour
{
    public static ManagementData Data;

    public static GameObject ManagementUI;

    // When loading a game
    public static bool Load(ManagementData data)
    {
        try
        {
            Data = data;

            for (int i = 0; i < data.ExpansionLevel; i++)
            {
                Vector2 pos = new Vector2(-10.25f, 3.375f - i * 2.5f);
                Instantiate(Resources.Load("Middle module"), pos, Quaternion.Euler(0, 0, 0));
            }

            ManagementUI = GameObject.Find("UI").transform.Find("Management").gameObject;
            ManagementUI.transform.Find("Expansion").Find("Expansion text").GetComponent<Text>().text = string.Format("You can make your farm 5 blocks bigger for {0}$.", 4500 + 1500 * data.ExpansionLevel);
            ManagementUI.transform.Find("Expansion").Find("Expand button").Find("Warning").GetComponent<Text>().text = string.Format("New debt: {0}$", 4500 + 1500 * data.ExpansionLevel);
            ManagementUI.transform.Find("Debt").Find("Debt text").GetComponent<Text>().text = string.Format("Remaining debt: {0}$.", Master.Data.Debt);
            ManagementUI.transform.Find("Debt").Find("Pay debt button").Find("Warning").GetComponent<Text>().text = string.Format("-{0}$", Master.Data.Debt);

            if (Master.Data.Debt > 0) ManagementUI.transform.Find("Debt").gameObject.SetActive(true);
            else ManagementUI.transform.Find("Expansion").gameObject.SetActive(true);
        
        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "Management", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    { 
        Data = new ManagementData();
        Data.ExpansionLevel = 0;
        Data.ExpandField = false;

        ManagementUI = GameObject.Find("UI").transform.Find("Management").gameObject;
        ManagementUI.transform.Find("Expansion").Find("Expansion text").GetComponent<Text>().text = string.Format("You can make your farm 5 blocks bigger for {0}$.", 4500 + 1500 * Data.ExpansionLevel);
        ManagementUI.transform.Find("Expansion").Find("Expand button").Find("Warning").GetComponent<Text>().text = string.Format("New debt: {0}$", 4500 + 1500 * Data.ExpansionLevel);
        ManagementUI.transform.Find("Debt").Find("Debt text").GetComponent<Text>().text = string.Format("Remaining debt: {0}$.", Master.Data.Debt);
        ManagementUI.transform.Find("Debt").Find("Pay debt button").Find("Warning").GetComponent<Text>().text = string.Format("-{0}$", Master.Data.Debt);

        ManagementUI.transform.Find("Debt").gameObject.SetActive(true);

        return true;
    }

    public void PayDebt()
    {
        if (Master.Data.Debt > Master.Data.Balance || Data.ExpandField) return;
        Master.UpdateBalance(-Master.Data.Debt);

        Master.Data.LastDayDebt = Master.Data.Debt;
        Master.Data.Debt = 0;

        ManagementUI.transform.Find("Debt").Find("Debt text").GetComponent<Text>().text = string.Format("Debt paid!");
        ManagementUI.transform.Find("Debt").gameObject.SetActive(false);
        ManagementUI.transform.Find("Expansion").gameObject.SetActive(true);
    }

    public void Expand()
    {
        if (Master.Data.Debt > 0 || Data.ExpandField) return;
        Data.ExpansionLevel++;
        int cost = 3000 + 1500 * Data.ExpansionLevel;
        Master.Data.Debt = cost;
        Data.ExpandField = true;

        ManagementUI.transform.Find("Expansion").Find("Expansion text").GetComponent<Text>().text = "Your farm will be expanded 5 blocks tomorrow.";
        ManagementUI.transform.Find("Expansion").Find("Expand button").gameObject.SetActive(false);
    }

    public static void ExpandField()
    {
        Vector2 pos = new Vector2(-10.25f, 5.875f - Data.ExpansionLevel * 2.5f);
        GameObject newModule = Instantiate(Resources.Load<GameObject>("Middle module"), pos, Quaternion.Euler(0, 0, 0));
        Vector2 lastModulePos = new Vector2(-10.25f, 3.375f - Data.ExpansionLevel * 2.5f);
        GameObject.Find("Environment").transform.Find("Last module").position = lastModulePos;

        for (int i = 0; i < AI.CustomerPositions.Count; i++)
        {
            if (AI.CustomerPositions[i].y < 5) AI.CustomerPositions[i] = new Vector2(AI.CustomerPositions[i].x, AI.CustomerPositions[i].y - 2.5f);
        }

        // Add new vertices
        Transform t = newModule.transform.Find("Vertices");
        Vector3 firstV = t.Find("First vertex").position;
        Vector3 lastV = t.Find("Last vertex").position;
        for (float i = firstV.x; i <= lastV.x; i += 0.25f)
        {
            for (float j = firstV.y; j <= lastV.y; j += 0.25f)
            {
                VertexSystem.Vertices.Add(new Vertex(new Vector2(i, j)));
            }
        }

        // Sidewalk
        firstV = t.Find("Sidewalk").Find("First").position;
        lastV = t.Find("Sidewalk").Find("Last").position;
        for (float i = firstV.x; i <= lastV.x; i += 0.25f)
        {
            for (float j = firstV.y; j <= lastV.y; j += 0.25f)
            {
                VertexSystem.Vertices.Add(new Vertex(new Vector2(i, j), VertexState.Walkable));
            }
        }

        ManagementUI.transform.Find("Debt").Find("Debt text").GetComponent<Text>().text = string.Format("Remaining debt: {0}$.", Master.Data.Debt);
        ManagementUI.transform.Find("Debt").Find("Pay debt button").Find("Warning").GetComponent<Text>().text = string.Format("-{0}$", Master.Data.Debt);
        ManagementUI.transform.Find("Expansion").Find("Expansion text").GetComponent<Text>().text = string.Format("You can make your farm 5 blocks bigger for {0}$.", 4500 + 1500 * Data.ExpansionLevel);
        ManagementUI.transform.Find("Expansion").Find("Expand button").Find("Warning").GetComponent<Text>().text = string.Format("New debt: {0}$", 4500 + 1500 * Data.ExpansionLevel);
        ManagementUI.transform.Find("Expansion").Find("Expand button").gameObject.SetActive(true);
        ManagementUI.transform.Find("Expansion").gameObject.SetActive(false);
        ManagementUI.transform.Find("Debt").gameObject.SetActive(true);
        Data.ExpandField = false;
    }
}