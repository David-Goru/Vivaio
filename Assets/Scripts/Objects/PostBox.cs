using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PostBox : MonoBehaviour
{
    public static Queue<Letter> Letters;

    void Start()
    {
        Letters = new Queue<Letter>();

        // Send first letter ever (debt information letter)
        string type = "SDPF"; // State Department of Plants and Farms
        string title = "Debt information";
        string body = string.Format("Hello sir,\nfrom the <b>State Department of Farms and Plants</b>, we inform you that a new debt has been created in your farm, due to:\n       {0}\nThe total debt ascends to <b>{1}€</b>. The debt will be paid in amounts of <b>{2}€</b> every day if possible. You won’t be able to request a new upgrade until the current debt is completely paid.", "New farm bought", Master.Debt, Master.DailyDebt);
        string signature = "<i>SDFP</i>";
        Letter debtLetter = new Letter(type, title, body, signature);
        Letters.Enqueue(debtLetter);
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
                body = string.Format("Hello,\nI am {0}, one of your closest customers. I would like to tell you that your shop is just… amazing. You have had me coming over and over, and always had what I needed. Please, keep doing what you do. I would love to see how far your shop goes.", c.Name);
                signature = string.Format("<i>Your happy customer, {0}</i>", c.Name);
                Letter customerLetter = new Letter(type, title, body, signature);
                Letters.Enqueue(customerLetter);
            }
            else if (c.Trust <= 10 && !c.LetterSent)
            {
                c.LetterSent = true;
                type = "Customer";
                title = "Unhappy customer";
                body = string.Format("Hello,\nI am {0}, and I am writing this just as a warning. After coming to your shop several days, I have realized that your shop is not supplying your customers with the right products. I would love to see you improve and learn from your mistakes. Please, focus on what you do. Ask for help if you need, there are a lot of farmers that would love to teach you how to build a good shop. Do not hate me, I am writing this to help, to make a constructive critic. And, as so, please, take into consideration what I have said. Success comes from knowing that you did your best to become the best that you are capable of becoming.", c.Name);
                signature = string.Format("<i>Your unhappy customer, {0}</i>", c.Name);
                Letter customerLetter = new Letter(type, title, body, signature);
                Letters.Enqueue(customerLetter);
            }
        }

        // Check for debt letters
        // Daily debt
        type = "SDPF";
        title = "Last day expenses";
        body = string.Format("From the <b>State Department of Farms and Plants</b> we inform you that the day {0} expenses are as follows:\n        Energy usage: -{1}€\n        Water usage: -{2}€\n", Master.Day - 1, 1, Master.LastDayWaterUsage);
        if (Master.LastDayDebt > 0) body += string.Format("        Debt: -{0}€\n        ------------------\n        Total: -{1}€\n        Remaining debt: {2}€", Master.LastDayDebt, Master.LastDayPaid, Master.Debt);
        else body += string.Format("        ------------------\n        Total: -{0}€", Master.LastDayPaid);
        signature = "<i>SDFP</i>";
        Letter dailyDebtLetter = new Letter(type, title, body, signature);
        Letters.Enqueue(dailyDebtLetter);

        // Fully paid debt
        if (Master.Debt == 0 && Master.LastDayDebt > 0)
        {
            type = "SDPF";
            title = "Debt paid";
            body = "Hello sir,\nfrom the <b>State Department of Farms and Plants</b>, we thank you for paying properly the last debt, and so, we inform you that you are able to request a new upgrade for your farm.";
            signature = "<i>SDFP</i>";
            Letter lastPaymentLetter = new Letter(type, title, body, signature);
        }

        // Update mail box warning
        if (Letters.Count > 0)
        {
            transform.Find("Warning").gameObject.SetActive(true);
        }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) > 1.5f) return;
        
        if (Letters.Count > 0 && !Inventory.InventorySlot.activeSelf)
        {
            Letter letterToTake = Letters.Dequeue();
            Inventory.ObjectInHand = letterToTake;
            Inventory.ChangeObject();
        }
        
        // Update mail box warning
        if (Letters.Count == 0)
        {
            transform.Find("Warning").gameObject.SetActive(false);
        }
    }
}