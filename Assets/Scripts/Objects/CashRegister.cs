using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CashRegister : BuildableObject
{
    [SerializeField] public List<ShopTicket> CashLog;
    [SerializeField] public List<Vector2> CustomerPos;

    public CashRegister(string translationKey) : base("Cash register", 1, 1, translationKey)
    {
        CashLog = new List<ShopTicket>();   
        CustomerPos = new List<Vector2>();
    }

    public override void ActionTwo()
    {
        UI.OpenNewObjectUI(this);
    }

    // UI stuff
    public override void OpenUI()
    {
        for (int i = CashLog.Count - 1; i >= 0; i--)
        {
            GameObject ticket = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Shop/Ticket"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            ticket.transform.SetParent(UI.Elements["Cash register log list"].transform, false);
            ticket.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Shop/Customer photos/" + CashLog[i].Consumer);
            ticket.transform.Find("Name").GetComponent<Text>().text = CashLog[i].Consumer;
            ticket.transform.Find("Date").GetComponent<Text>().text = string.Format(Localization.Translations["Day"], CashLog[i].Day);

            string itemsList = CashLog[i].ItemsBought;
            int totalLines = CashLog[i].TotalLines;

            itemsList += string.Format(Localization.Translations["cash_log_total_amount"], CashLog[i].Total);

            ticket.transform.Find("Items list").GetComponent<Text>().text = itemsList;

            // Update sizes and positions
            ticket.GetComponent<RectTransform>().sizeDelta = new Vector2(335, 90 + 25 * totalLines);
            ticket.transform.Find("Items list").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(305, 25 * totalLines);
            ticket.transform.Find("Items list").gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(170, -87.5f - 12.5f * totalLines);
            ticket.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        UI.Elements["Cash register"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Cash register"].SetActive(false);

        foreach (Transform t in UI.Elements["Cash register log list"].transform)
        {
            MonoBehaviour.Destroy(t.gameObject);
        }
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Cash register take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
    }
}