using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandUI : MonoBehaviour
{
    public static Stand StandObject;

    public void ReActivate()
    {
        transform.Find("Price").GetComponent<InputField>().text = "";
        transform.Find("Item name").GetComponent<Text>().text = StandObject.Item.Name;
        transform.Find("Price").Find("Placeholder").GetComponent<Text>().text = StandObject.ItemValue.ToString();
        transform.Find("Recommended price").GetComponent<Text>().text = "Recommended price: " + StandObject.Item.MediumValue;

        if (StandObject.Available) transform.Find("Change state").transform.Find("Text").GetComponent<Text>().text = "Disable stand";
        else transform.Find("Change state").transform.Find("Text").GetComponent<Text>().text = "Enable stand";
    }

    void Update()
    {
        if (Vector2.Distance(GameObject.Find("Player").transform.position, StandObject.Model.transform.position) > 1.5f) gameObject.SetActive(false);
    }

    public void ChangeState()
    {
        if (StandObject.Available)
        {
            StandObject.Available = false;
            transform.Find("Change state").transform.Find("Text").GetComponent<Text>().text = "Enable stand";
        }
        else
        {
            StandObject.Available = true;
            transform.Find("Change state").transform.Find("Text").GetComponent<Text>().text = "Disable stand";
        }
    }

    public void UpdatePrice()
    {
        int.TryParse(transform.Find("Price").GetComponent<InputField>().text, out StandObject.ItemValue);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
