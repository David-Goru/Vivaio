using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CashRegister : MonoBehaviour
{
    public static CashRegisterData Data;
    public static GameObject CashRegisterModel;
    public static List<Vector2> CustomerPos;

    // When loading a game
    public static bool Load(CashRegisterData data)
    {
        try
        {
            Data = data;
        }
        catch (Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "CashRegister", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new CashRegisterData();
        Data.CashLog = new List<ShopTicket>();

        foreach (Transform t in CashRegisterModel.transform.Find("Vertices"))
        {                            
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            if (v != null) v.State = VertexState.Occuppied;
        }

        return true;
    }

    void Update()
    {
        if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) > 2)
        {
            GameObject.Find("UI").transform.Find("Cash register").gameObject.SetActive(false);
            enabled = false;
        }
    }

    public static void OpenCashLog()
    {
        GameObject ui = GameObject.Find("UI");

        Transform content = ui.transform.Find("Cash register").Find("Log").Find("Viewport").Find("Content");
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }

        for (int i = Data.CashLog.Count - 1; i >= 0; i--)
        {
            GameObject ticket = Instantiate(Resources.Load<GameObject>("Shop/Ticket"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            ticket.transform.SetParent(content);
            ticket.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Shop/Customer photos/" + Data.CashLog[i].Consumer);
            ticket.transform.Find("Name").GetComponent<Text>().text = Data.CashLog[i].Consumer;
            ticket.transform.Find("Date").GetComponent<Text>().text = "Day " + Data.CashLog[i].Day;

            string itemsList = Data.CashLog[i].ItemsBought;
            int totalLines = Data.CashLog[i].TotalLines;

            itemsList += string.Format("Total amount: {0}$", Data.CashLog[i].Total);

            ticket.transform.Find("Items list").GetComponent<Text>().text = itemsList;

            // Update sizes and positions
            ticket.GetComponent<RectTransform>().sizeDelta = new Vector2(335, 90 + 25 * totalLines);
            ticket.transform.Find("Items list").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(305, 25 * totalLines);
            ticket.transform.Find("Items list").gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(170, -87.5f - 12.5f * totalLines);
            ticket.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        ui.transform.Find("Cash register").gameObject.SetActive(true);
        CashRegisterModel.GetComponent<CashRegister>().enabled = true;
    }
}