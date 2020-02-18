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
                InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + objectName);
                InventorySlot.SetActive(true);
                if (objectName == "Watering can")
                { 
                    InventoryText.text = objectName + " (" + PlayerTools.ToolOnHand.Remaining + "/10)";
                    if (PlayerTools.ToolOnHand.Remaining == 0) InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Watering can empty");
                }
                else InventoryText.text = objectName;
                break;
            case "Seed":
                InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + objectName + " seeds");
                InventorySlot.SetActive(true);
                InventoryText.text = objectName + " (" + PlayerTools.ToolOnHand.Remaining + "/10)";
                break;
            case "Letter":
                Letter letter = (Letter)ObjectInHand;
                InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
                InventorySlot.SetActive(true);
                InventoryText.text = letter.Type + " letter";
                GameObject.Find("UI").transform.Find("Open letter").gameObject.SetActive(true);
                break;
            default:
                InventorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + objectName);
                InventoryText.text = objectName;
                InventorySlot.SetActive(true);
                break;
        }
    }

    public static void ChangeSubobject(string objectName, string type)
    {
        switch (type)
        {
            case "None":
                InventorySlot.transform.Find("Subobject").gameObject.SetActive(false);
                InventoryText.text = "Basket";
                break;
            case "Crop":
                InventorySlot.transform.Find("Subobject").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + objectName);
                InventorySlot.transform.Find("Subobject").gameObject.SetActive(true);
                InventoryText.text = string.Format("Basket\n{0}/{1} of {2}", PlayerTools.ToolOnHand.Remaining, 20, objectName.ToLower());
                break;
        }
    }
}