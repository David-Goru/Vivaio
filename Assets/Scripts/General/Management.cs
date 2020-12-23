using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Management : MonoBehaviour
{
    public static ManagementData Data;

    // When loading a game
    public static bool Load(ManagementData data)
    {
        try
        {
            Data = data;

            GameObject module;
            for (int i = 0; i < data.ExpansionLevel; i++)
            {
                Vector2 pos = new Vector2(-10.25f, 3.375f - i * 2.5f);
                module = Instantiate<GameObject>(Resources.Load<GameObject>("Middle module"), pos, Quaternion.Euler(0, 0, 0));
                if (Master.GameEdition == "Halloween") module.transform.Find("HW").gameObject.SetActive(true);
            }

            GameObject.Find("Environment").transform.Find("Last module").position = new Vector2(-10.25f, 3.375f - data.ExpansionLevel * 2.5f);

            UI.Elements["Management expansion text"].GetComponent<Text>().text = string.Format(Localization.Translations["management_expansion_text"], 4500 + 1500 * data.ExpansionLevel);
            UI.Elements["Management expand warning"].GetComponent<Text>().text = string.Format(Localization.Translations["management_expand_warning"], 4500 + 1500 * data.ExpansionLevel);
            UI.Elements["Management debt text"].GetComponent<Text>().text = string.Format(Localization.Translations["management_remaining_debt"], Master.Data.Debt);
            UI.Elements["Management pay debt warning"].GetComponent<Text>().text = string.Format(Localization.Translations["management_pay_debt_warning"], Master.Data.Debt);

            if (Master.Data.Debt > 0) UI.Elements["Management debt"].SetActive(true);
            else UI.Elements["Management expansion"].SetActive(true);
        
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

        UI.Elements["Management expansion text"].GetComponent<Text>().text = string.Format(Localization.Translations["management_expansion_text"], 4500 + 1500 * Data.ExpansionLevel);
        UI.Elements["Management expand warning"].GetComponent<Text>().text = string.Format(Localization.Translations["management_expand_warning"], 4500 + 1500 * Data.ExpansionLevel);
        UI.Elements["Management debt text"].GetComponent<Text>().text = string.Format(Localization.Translations["management_remaining_debt"], Master.Data.Debt);
        UI.Elements["Management pay debt warning"].GetComponent<Text>().text = string.Format(Localization.Translations["management_pay_debt_warning"], Master.Data.Debt);

        UI.Elements["Management debt"].SetActive(true);

        return true;
    }

    public void PayDebt()
    {
        if (Master.Data.Debt > Master.Data.Balance || Data.ExpandField) return;
        Master.UpdateBalance(-Master.Data.Debt);

        Master.Data.LastDayDebt = Master.Data.Debt;
        Master.Data.Debt = 0;

        UI.Elements["Management debt text"].GetComponent<Text>().text = Localization.Translations["management_debt_paid"];
        UI.Elements["Management debt"].SetActive(false);
        UI.Elements["Management expansion"].SetActive(true);
    }

    public void Expand()
    {
        if (Master.Data.Debt > 0 || Data.ExpandField) return;
        Data.ExpansionLevel++;
        int cost = 3000 + 1500 * Data.ExpansionLevel;
        Master.Data.Debt = cost;
        Data.ExpandField = true;

        UI.Elements["Management expansion text"].GetComponent<Text>().text = Localization.Translations["management_expand_announcement"];
        UI.Elements["Management expand button"].SetActive(false);
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

        List<Vertex> verticesToUpdate = new List<Vertex>();

        // Add new vertices
        VertexSystem.ExpandGrid();
        
        // Add farm plots
        Transform t = newModule.transform.Find("Vertices");
        Vector3 firstV = t.Find("First vertex").position;
        Vector3 lastV = t.Find("Last vertex").position;
        MapSystem.ExpandFarm(firstV, lastV);

        UI.Elements["Management debt text"].GetComponent<Text>().text = string.Format(Localization.Translations["management_remaining_debt"], Master.Data.Debt);
        UI.Elements["Management pay debt warning"].GetComponent<Text>().text = string.Format(Localization.Translations["management_pay_debt_warning"], Master.Data.Debt);
        UI.Elements["Management expansion text"].GetComponent<Text>().text = string.Format(Localization.Translations["management_expansion_text"], 4500 + 1500 * Data.ExpansionLevel);
        UI.Elements["Management expand warning"].GetComponent<Text>().text = string.Format(Localization.Translations["management_expand_warning"], 4500 + 1500 * Data.ExpansionLevel);
        UI.Elements["Management expand button"].SetActive(true);
        UI.Elements["Management expansion"].SetActive(false);
        UI.Elements["Management debt"].SetActive(true);
        Data.ExpandField = false;
    }

    public static void UpdateDebt()
    {
        UI.Elements["Management debt text"].GetComponent<Text>().text = string.Format(Localization.Translations["management_remaining_debt"], Master.Data.Debt);
        UI.Elements["Management pay debt warning"].GetComponent<Text>().text = string.Format(Localization.Translations["management_pay_debt_warning"], Master.Data.Debt);
    }
}