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
        string title = "State Department of Plants and Farms";
        string body = string.Format("Welcome Mr. Farmer to your newly bought plot.\nYour debt produced by the purchase of the land with number 01 amounts to a total of {0}€.\n\nSincerely, \nthe State Department\nof Plants and Farms", Master.Debt);
        Letter debtLetter = new Letter(type, title, body);
        Letters.Enqueue(debtLetter);
    }

    public void UpdateLetters()
    {
        string type;
        string title;
        string body;

        // Check for customers letters       
        foreach (Customer c in AI.AvailableCustomers)
        {
            if (c.Trust >= 100 && !c.LetterSent)
            {
                c.LetterSent = true;
                type = "Customer";
                title = "I'm very happy!";
                body = "Your shop is awesome.\nKeep doing what you do!";
            }
            else if (c.Trust <= 10 && !c.LetterSent)
            {
                c.LetterSent = true;
                type = "Customer";
                title = "I'm very sad!";
                body = "Your shop is a mesh.\nChange what you do!";
            }
        }

        // Check for debt letters
        // Daily debt
        type = "SDPF";
        title = "Invoice information";
        body = string.Format("Energy usage: -{0}€\nWater usage: -{1}€\n", 1, Master.LastDayWaterUsage);
        if (Master.LastDayDebt > 0) body += string.Format("Debt: -{0}€\n------------------\nTotal: -{1}€\nRemaining debt: {2}€\n\nSincerely, \nthe State Department\nof Plants and Farms", Master.LastDayDebt, Master.LastDayPaid, Master.Debt);
        else body += string.Format("------------------\nTotal: -{0}€\n\nSincerely,\nthe State Department\nof Plants and Farms", Master.LastDayDebt, Master.LastDayPaid, Master.Debt);
        Letter dailyDebtLetter = new Letter(type, title, body);
        Letters.Enqueue(dailyDebtLetter);

        // Fully paid debt
        if (Master.Debt == 0 && Master.LastDayDebt > 0)
        {
            type = "SDPF";
            title = "Payment completed";
            body = "Hello Mr. Farmer, thank you for finalizing the payment.\nWe hope our service has helped you, and would love to work with you again.\n\nSincerely,\nthe State Department\nof Plants and Farms";
            Letter lastPaymentLetter = new Letter(type, title, body);
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
            Inventory.ChangeObject("Letter", "Letter");
        }

        //InvoiceUI.transform.Find("Energy").GetComponent<Text>().text = string.Format("Energy usage: -{0}€", 1);
        //InvoiceUI.transform.Find("Water").GetComponent<Text>().text = string.Format("Water usage: -{0}€", Master.LastDayWaterUsage);
        //InvoiceUI.transform.Find("Debt").GetComponent<Text>().text = string.Format("Debt: -{0}€", Master.LastDayDebt);
        //InvoiceUI.transform.Find("Total paid").GetComponent<Text>().text = string.Format("Total: -{0}€", Master.LastDayPaid);
        //InvoiceUI.transform.Find("Remaining debt").GetComponent<Text>().text = string.Format("Remaining debt: {0}€", Master.Debt);
        //InvoiceUI.SetActive(true);
        
        // Update mail box warning
        if (Letters.Count == 0)
        {
            transform.Find("Warning").gameObject.SetActive(false);
        }
    }
}