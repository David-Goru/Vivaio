using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static GameObject InventorySlot;
    public static Text InventoryText;    
    
    public static InventoryData Data;

    void Start()
    {
        InventorySlot = GameObject.Find("UI").transform.Find("Inventory").Find("Object").gameObject;
        InventoryText = InventorySlot.transform.parent.Find("Object info").gameObject.GetComponent<Text>();
    }    

    // When loading a game
    public static bool Load(InventoryData data)
    {
        try
        {
            Data = data;
            if (data.ObjectInHand != null) ChangeObject();
        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "Inventory", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new InventoryData();

        return true;
    }

    public static void RemoveObject()
    {
        InventorySlot.SetActive(false);
        InventorySlot.transform.Find("Subobject").gameObject.SetActive(false);
        InventoryText.text = "";
        Data.ObjectInHand = null;

        Transform UI = GameObject.Find("UI").transform;
        UI.Find("Build button").gameObject.SetActive(false);
        UI.Find("Throw object").gameObject.SetActive(false);
        UI.Find("Letter").gameObject.SetActive(false);
        UI.Find("Open letter").gameObject.SetActive(false);
    }

    public static bool AddObject(IObject item)
    {
        if (Data.ObjectInHand == null)
        {
            Data.ObjectInHand = item;
            ChangeObject();
            return true;
        }
        else if (Data.ObjectInHand.Name == item.Name && (Data.ObjectInHand.Stack + item.Stack) <= item.MaxStack)
        {
            Data.ObjectInHand.Stack += item.Stack;
            ChangeObject();
            return true;
        }
        return false;
    }

    public static void ChangeObject()
    {
        InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + Data.ObjectInHand.Name);

        Transform UI = GameObject.Find("UI").transform;
        InventoryText.text = Data.ObjectInHand.Name;
        UI.Find("Throw object").gameObject.SetActive(true);

        if (Data.ObjectInHand is Seed)
        {
            Seed seed = (Seed)Data.ObjectInHand;
            InventoryText.text = seed.Name + " (" + seed.Stack + ")";
        }
        else if (Data.ObjectInHand is Fertilizer)
        {
            Fertilizer fertilizer = (Fertilizer)Data.ObjectInHand;
            InventoryText.text = "Fertilizer (" + fertilizer.Stack + ")";
        }
        else if (Data.ObjectInHand is Tool)
        {
            if (Data.ObjectInHand is WateringCan)
            {
                WateringCan wc = (WateringCan)Data.ObjectInHand;
                InventoryText.text = wc.Name + " (" + wc.Remaining + "/10)";
                if (wc.Remaining == 0) InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Watering can empty");
            }
            else if (Data.ObjectInHand is Basket)
            {
                Basket basket = (Basket)Data.ObjectInHand;
                if (basket.Product != null)
                {
                    InventoryText.text = basket.Name + " (" + basket.Amount + "/20)";
                    InventorySlot.transform.Find("Subobject").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + basket.Product.Name);
                    InventorySlot.transform.Find("Subobject").gameObject.SetActive(true);
                }
                else InventorySlot.transform.Find("Subobject").gameObject.SetActive(false);
            }
        }
        else if (Data.ObjectInHand is BuildableObject)
        {
            UI.Find("Build button").gameObject.SetActive(true);
            if (Data.ObjectInHand is Floor || Data.ObjectInHand is Wall) InventoryText.text = Data.ObjectInHand.Name + " (" + ((BuildableObject)Data.ObjectInHand).Stack + ")";
        }
        else if (Data.ObjectInHand is Letter)
        {
            Letter letter = (Letter)Data.ObjectInHand;
            InventoryText.text = letter.Type + " letter";
            InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
            UI.Find("Open letter").gameObject.SetActive(true);
        }
        else if (Data.ObjectInHand.Name == "Drip bottle")
        {
            InventoryText.text = Data.ObjectInHand.Name + " (" + ((DripBottle)Data.ObjectInHand).WaterUnits + "u)";
            InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Drip bottle/" + ((DripBottle)Data.ObjectInHand).WaterUnits);
        }
        else if (Data.ObjectInHand.MaxStack > 1) // Flour, bread dough...
        {
            InventoryText.text = Data.ObjectInHand.Name + "(" + Data.ObjectInHand.Stack + ")";
        }
        
        InventorySlot.SetActive(true);
    }
}