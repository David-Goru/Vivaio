using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mailbox : MonoBehaviour
{
    public static GameObject Warning;
    public static MailboxData Data;

    // When loading a game
    public static bool Load(MailboxData data)
    {
        try
        {
            Data = data;
            Warning = GameObject.FindGameObjectWithTag("Mailbox").transform.Find("Warning").gameObject;

            if (data.Letters == null) Data.Letters = new Queue<Letter>();
            if (data.Letters.Count > 0) Warning.SetActive(true);
            else Warning.SetActive(false);
        }
        catch (Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "Mailbox", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new MailboxData();
        Warning = GameObject.FindGameObjectWithTag("Mailbox").transform.Find("Warning").gameObject;
        Data.Letters = new Queue<Letter>();

        // Send first letter ever (debt information letter)
        string type = "SDPF"; // State Department of Plants and Farms
        string title = Localization.Translations["debt_information_title"];
        string body = string.Format(Localization.Translations["debt_information_letter_body"], Localization.Translations["debt_information_topic_new_farm"], Master.Data.Debt, Master.Data.DailyDebt);
        string signature = "<i>" + Localization.Translations["SDFP_signature"] + "</i>";
        NewLetter(type, title, body, signature);

        return true;
    }

    public void UpdateLetters()
    {
        string type;
        string title;
        string body;
        string signature;

        // Check for customers letters       
        foreach (Customer c in AI.Customers)
        {
            if (c.Trust >= 100 && !c.LetterSent)
            {
                c.LetterSent = true;
                type = "Customer";
                title = Localization.Translations["happy_customer_title"];
                body = string.Format(Localization.Translations["happy_customer_letter_body"], c.Name);
                signature = string.Format("<i>" + Localization.Translations["happy_customer_signature"] + "</i>", c.Name);
                NewLetter(type, title, body, signature);
            }
            else if (c.Trust <= 10 && !c.LetterSent)
            {
                c.LetterSent = true;
                type = "Customer";
                title = Localization.Translations["unhappy_customer_title"];
                body = string.Format(Localization.Translations["unhappy_customer_letter_body"], c.Name);
                signature = string.Format("<i>" + Localization.Translations["unhappy_customer_signature"] + "</i>", c.Name);
                NewLetter(type, title, body, signature);
            }
        }

        // Check for debt letters
        // Daily debt
        type = "SDPF";
        title = Localization.Translations["last_day_expenses_title"];
        body = string.Format(Localization.Translations["last_day_expenses_letter_body_start"], Master.Data.Day, Master.Data.LastDayEnergyUsage, Master.Data.LastDayWaterUsage);
        if (Master.Data.LastDayDebt > 0) body += string.Format(Localization.Translations["last_day_expenses_letter_body_debt"], Master.Data.LastDayDebt, Master.Data.LastDayPaid, Master.Data.Debt);
        else body += string.Format(Localization.Translations["last_day_expenses_letter_body_nodebt"], Master.Data.LastDayPaid);
        signature = "<i>" + Localization.Translations["SDFP_signature"] + "</i>";
        NewLetter(type, title, body, signature);

        // Fully paid debt
        if (Master.Data.Debt == 0 && Master.Data.LastDayDebt > 0)
        {
            type = "SDPF";
            title = Localization.Translations["debt_paid_title"];
            body = Localization.Translations["debt_paid_letter_body"];
            signature = "<i>" + Localization.Translations["SDFP_signature"] + "</i>";
            NewLetter(type, title, body, signature);
        }

        // New debt
        if (Management.Data.ExpandField)
        {
            type = "SDPF";
            title = Localization.Translations["debt_paid_title"];
            body =  string.Format(Localization.Translations["debt_information_letter_body"], Localization.Translations["debt_information_topic_farm_expansion"], 3000 + 1500 * Management.Data.ExpansionLevel, Master.Data.DailyDebt);
            signature = "<i>" + Localization.Translations["SDFP_signature"] + "</i>";
            NewLetter(type, title, body, signature);
        }
    }

    public static void NewLetter(string type, string title, string body, string signature)
    {
        Data.Letters.Enqueue(new Letter(type, title, body, signature));

        // Update mail box warning
        if (!Warning.activeSelf) Warning.SetActive(true);
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) > 1.5f) return;

        if (Data.Letters.Count > 0 && !Inventory.InventorySlot.activeSelf) Inventory.AddObject(Data.Letters.Dequeue());
        
        // Update mail box warning
        if (Data.Letters.Count == 0) transform.Find("Warning").gameObject.SetActive(false);
    }
}