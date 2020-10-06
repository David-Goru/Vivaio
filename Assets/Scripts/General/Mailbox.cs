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
        string title = "Debt information";
        string body = string.Format("Hello sir,\n        from the <b>State Department of Farms and Plants</b>, we inform you that a new debt has been created in your farm, due to:\n       {0}\nThe total debt ascends to <b>{1}$</b>. The debt will be paid in amounts of <b>{2}$</b> every day if possible. You won’t be able to request a new upgrade until the current debt is completely paid.", "New farm bought", Master.Data.Debt, Master.Data.DailyDebt);
        string signature = "<i>SDFP</i>";
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
                title = "Happy customer";
                body = string.Format("Hello,\n        I am {0}, one of your closest customers. I would like to tell you that your shop is just… amazing. You have had me coming over and over, and always had what I needed. Please, keep doing what you do. I would love to see how far your shop goes.", c.Name);
                signature = string.Format("<i>Your happy customer, {0}</i>", c.Name);
                NewLetter(type, title, body, signature);
            }
            else if (c.Trust <= 10 && !c.LetterSent)
            {
                c.LetterSent = true;
                type = "Customer";
                title = "Unhappy customer";
                body = string.Format("Hello,\n        I am {0}, and I am writing this as a warning. After coming to your shop several days, I have realized that your shop is not supplying your customers with the right products. I would love to see you improving and learning from your mistakes. Please, focus on what you do. Ask for help if you need, there are a lot of farmers that would love to teach you how to build a good shop. Do not hate me, this is a constructive critic. And, as so, please, take into consideration what I have said. Success comes from knowing that you did your best to become the best that you are capable of becoming.", c.Name);
                signature = string.Format("<i>Your unhappy customer, {0}</i>", c.Name);
                NewLetter(type, title, body, signature);
            }
        }

        // Check for debt letters
        // Daily debt
        type = "SDPF";
        title = "Last day expenses";
        body = string.Format("From the <b>State Department of Farms and Plants</b> we inform you that the day {0} expenses are as follows:\n        Energy usage: -{1}$\n        Water usage: -{2}$\n", Master.Data.Day, Master.Data.LastDayEnergyUsage, Master.Data.LastDayWaterUsage);
        if (Master.Data.LastDayDebt > 0) body += string.Format("        Debt: -{0}$\n        ------------------\n        Total: -{1}$\n        Remaining debt: {2}$", Master.Data.LastDayDebt, Master.Data.LastDayPaid, Master.Data.Debt);
        else body += string.Format("        ------------------\n        Total: -{0}$", Master.Data.LastDayPaid);
        signature = "<i>SDFP</i>";
        NewLetter(type, title, body, signature);

        // Fully paid debt
        if (Master.Data.Debt == 0 && Master.Data.LastDayDebt > 0)
        {
            type = "SDPF";
            title = "Debt paid";
            // When upgrades are available:
            body = "Hello sir,\n        from the <b>State Department of Farms and Plants</b>, we thank you for paying properly the last debt, and so, we inform you that you are able to request a new upgrade for your farm.";
            signature = "<i>SDFP</i>";
            NewLetter(type, title, body, signature);
        }

        // New debt
        if (Management.Data.ExpandField)
        {
            type = "SDPF";
            title = "Debt paid";
            body =  string.Format("Hello sir,\n        from the <b>State Department of Farms and Plants</b>, we inform you that a new debt has been created in your farm, due to:\n       {0}\nThe total debt ascends to <b>{1}$</b>. The debt will be paid in amounts of <b>{2}$</b> every day if possible. You won’t be able to request a new upgrade until the current debt is completely paid.", "Farm expansion", 3000 + 1500 * Management.Data.ExpansionLevel, Master.Data.DailyDebt);
            signature = "<i>SDFP</i>";
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