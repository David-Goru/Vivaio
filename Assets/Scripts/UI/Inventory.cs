using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static GameObject InventorySlot;
    public static Text InventoryText;    
    public static IObject ObjectInHand;

    void Start()
    {
        InventorySlot = GameObject.Find("UI").transform.Find("Inventory").Find("Object").gameObject;
        InventoryText = InventorySlot.transform.parent.Find("Object info").gameObject.GetComponent<Text>();
    }

    public static void RemoveObject()
    {
        InventorySlot.SetActive(false);
        InventorySlot.transform.Find("Subobject").gameObject.SetActive(false);
        InventoryText.text = "";
        ObjectInHand = null;

        Transform UI = GameObject.Find("UI").transform;
        UI.Find("Build button").gameObject.SetActive(false);
        UI.Find("Throw seeds").gameObject.SetActive(false);
        UI.Find("Letter").gameObject.SetActive(false);
        UI.Find("Open letter").gameObject.SetActive(false);
    }

    public static void ChangeObject()
    {
        InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + ObjectInHand.Name);

        Transform UI = GameObject.Find("UI").transform;
        InventoryText.text = ObjectInHand.Name;
        if (ObjectInHand is Seed)
        {
            Seed seed = (Seed)ObjectInHand;
            InventoryText.text = seed.Name + " (" + seed.Amount + ")";
            UI.Find("Throw seeds").gameObject.SetActive(true);
        }
        else if (ObjectInHand is Tool)
        {
            if (ObjectInHand is WateringCan)
            {
                WateringCan wc = (WateringCan)ObjectInHand;
                InventoryText.text = wc.Name + " (" + wc.Remaining + "/10)";
                if (wc.Remaining == 0) InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Watering can empty");
            }
            else if (ObjectInHand is Basket)
            {
                Basket basket = (Basket)ObjectInHand;
                if (basket.Product != null)
                {
                    InventoryText.text = basket.Name + " (" + basket.Amount + "/20)";
                    InventorySlot.transform.Find("Subobject").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + basket.Product.Name);
                    InventorySlot.transform.Find("Subobject").gameObject.SetActive(true);
                }
                else InventorySlot.transform.Find("Subobject").gameObject.SetActive(false);
            }
        }
        else if (ObjectInHand is BuildableObject)
        {
            UI.Find("Build button").gameObject.SetActive(true);
            if (ObjectInHand.Name == "Shop tile") InventoryText.text = ObjectInHand.Name + " (" + ((BuildableObject)ObjectInHand).Amount + ")";
        }
        else if (ObjectInHand is Letter)
        {
            Letter letter = (Letter)ObjectInHand;
            InventoryText.text = letter.Type + " letter";
            InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
            UI.Find("Open letter").gameObject.SetActive(true);
        }
        InventorySlot.SetActive(true);
    }
}