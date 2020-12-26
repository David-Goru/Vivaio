using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{    
    public static InventoryData Data;   

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
        AddObject(new House());

        return true;
    }

    public static bool RemoveObject(int amount = 0)
    {
        if (amount > 0 && Data.ObjectInHand.Stack != amount)
        {
            if (Data.ObjectInHand.Stack < amount) return false;
            Data.ObjectInHand.Stack -= amount;
            UI.Elements["Inventory item text"].GetComponent<Text>().text = Data.ObjectInHand.GetUIName();
            return true;
        }

        UI.Elements["Inventory item"].SetActive(false);
        UI.Elements["Inventory item text"].GetComponent<Text>().text = "";
        Data.ObjectInHand = null;

        UI.Elements["Build object button"].SetActive(false);
        UI.Elements["Throw object button"].SetActive(false);
        UI.Elements["Letter"].SetActive(false);
        UI.Elements["Open letter button"].SetActive(false);
        return true;
    }

    public static int AddObject(IObject item)
    {
        if (Data.ObjectInHand == null)
        {
            Data.ObjectInHand = item;
            ChangeObject();
            return item.Stack;
        }
        else if (Data.ObjectInHand.Name == item.Name)
        {
            if ((Data.ObjectInHand.Stack + item.Stack) <= item.MaxStack)
            {
                Data.ObjectInHand.Stack += item.Stack;
                ChangeObject();
                return item.Stack;
            }
            else
            {
                int amountToTake = item.MaxStack - Data.ObjectInHand.Stack;
                Data.ObjectInHand.Stack += amountToTake;
                return amountToTake;
            }
        }
        return 0;
    }

    public static void ChangeObject()
    {
        UI.Elements["Inventory item text"].GetComponent<Text>().text = Data.ObjectInHand.GetUIName();
        UI.Elements["Inventory item"].GetComponent<Image>().sprite = Data.ObjectInHand.GetUISprite();
        UI.Elements["Inventory item"].SetActive(true);
        UI.Elements["Throw object button"].SetActive(true);
        if (Data.ObjectInHand is BuildableObject) UI.Elements["Build object button"].SetActive(true);
        else if (Data.ObjectInHand is Letter) UI.Elements["Open letter button"].SetActive(true);
    }
}