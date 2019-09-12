﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static GameObject InventorySlot;
    public static Text InventoryText;

    void Start()
    {
        InventorySlot = GameObject.Find("UI").transform.Find("Inventory").Find("Object").gameObject;
        InventoryText = InventorySlot.transform.parent.Find("Object info").gameObject.GetComponent<Text>();
    }

    public static void ChangeObject(string objectName, string type)
    {
        switch (type)
        {
            case "None":
                InventorySlot.SetActive(false);
                InventorySlot.transform.Find("Subobject").gameObject.SetActive(false);
                InventoryText.text = "";
                break;
            case "Tool":
                InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Tools/" + objectName);
                InventorySlot.SetActive(true);
                InventoryText.text = objectName;
                break;
            case "Seed":
                InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Seeds/" + objectName + " seed");
                InventorySlot.SetActive(true);
                InventoryText.text = "Seed: " + objectName;
                break;
        }
    }

    public static void ChangeSubobject(string objectName, string type)
    {
        switch (type)
        {
            case "None":
                InventorySlot.transform.Find("Subobject").gameObject.SetActive(false);
                break;
            case "Crop":
                InventorySlot.transform.Find("Subobject").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Crops/" + objectName);
                InventorySlot.transform.Find("Subobject").gameObject.SetActive(true);
                InventoryText.text = string.Format("Basket\n{0}/{1} of {2}", PlayerTools.CropsInBasket, 20, objectName.ToLower());
                break;
        }
    }
}