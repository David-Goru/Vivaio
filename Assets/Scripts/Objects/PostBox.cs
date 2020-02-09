using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PostBox : MonoBehaviour
{
    public GameObject InvoiceUI;

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) > 1.5f) return;
        if (Master.Day == 0)
        {
            InvoiceUI.transform.Find("Remaining debt").GetComponent<Text>().text = string.Format("Remaining debt: {0}€", Master.Debt);
            InvoiceUI.SetActive(true);
            return;
        }
        InvoiceUI.transform.Find("Energy").GetComponent<Text>().text = string.Format("Energy usage: -{0}€", 1);
        InvoiceUI.transform.Find("Water").GetComponent<Text>().text = string.Format("Water usage: -{0}€", Master.LastDayWaterUsage);
        InvoiceUI.transform.Find("Debt").GetComponent<Text>().text = string.Format("Debt: -{0}€", Master.LastDayDebt);
        InvoiceUI.transform.Find("Total paid").GetComponent<Text>().text = string.Format("Total: -{0}€", Master.LastDayPaid);
        InvoiceUI.transform.Find("Remaining debt").GetComponent<Text>().text = string.Format("Remaining debt: {0}€", Master.Debt);
        InvoiceUI.SetActive(true);
    }

    void Update()
    {
        if (InvoiceUI.activeSelf && Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) > 1.5f) InvoiceUI.SetActive(false);
    }
}