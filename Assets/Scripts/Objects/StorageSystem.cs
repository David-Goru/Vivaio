using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageSystem : MonoBehaviour
{
    public static List<Box> StorageList;

    void Start()
    {
        StorageList = new List<Box>();
    }

    public class Box
    {
        public IObject[] Items;
        public GameObject BoxObject;

        public Box(GameObject box)
        {
            BoxObject = box;
            Items = new IObject[4];
        }
        
        public void BoxClicked()
        {
            GameObject StorageUI = GameObject.Find("UI").transform.Find("Storage").gameObject;
            if ((StorageUI.GetComponent("StorageHandler") as StorageHandler).Box != BoxObject) return;

            if (Vector2.Distance(GameObject.Find("Player").transform.position, BoxObject.transform.position) <= 1.5f)
            {
                bool hasItems = false;
                for (int i = 0; i < 4; i++)
                { 
                    int slotNumber = i + 1;
                    StorageUI.transform.Find("Slot " + slotNumber).gameObject.SetActive(true);
                    if (Items[i] == null)
                    {
                        SetUIButton(StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), i, false);
                        StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Add");
                    }
                    else
                    {
                        hasItems = true;
                        SetUIButton(StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), i, true);
                        if (Items[i] is Letter)
                        {
                            Letter letter = (Letter)Items[i];                     
                            StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
                        }
                        else
                        {
                            ItemsHandler.Item item = (ItemsHandler.Item)Items[i];
                            StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + item.Name);
                        }
                    }
                }
                if (!hasItems) GameObject.Find("UI").transform.Find("Take storage").gameObject.SetActive(true);
                StorageUI.SetActive(true);
            }
        }    

        public void SetUIButton(Button button, int i, bool take)  
        {
            button.onClick.RemoveAllListeners();
            if (take) button.onClick.AddListener(() => TakeItem(i));
            else button.onClick.AddListener(() => PutItem(i));
        }  

        public void TakeItem(int itemID)
        {
            GameObject StorageUI = GameObject.Find("UI").transform.Find("Storage").gameObject;
            if ((StorageUI.GetComponent("StorageHandler") as StorageHandler).Box != BoxObject) return;

            if (!Inventory.InventorySlot.activeSelf)
            {
                if (Items[itemID] is Letter)
                {
                    Inventory.ObjectInHand = (Letter)Items[itemID];
                    Inventory.ChangeObject("Letter", "Letter");
                }
                else
                {
                    ItemsHandler.Item item = (ItemsHandler.Item)Items[itemID];
                    if (item.Use == "Build")
                    {
                        if (item.Name == "Shop tile") PlayerTools.TilesAmount = item.Amount;
                        (GameObject.Find("Farm handler").GetComponent("Build") as Build).ObjectName = item.Name;
                        GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(true);
                        Inventory.ChangeObject(item.Name, "Object");
                    }
                    else if (item.Use == "Farm") // Drip bottle
                    {
                        Inventory.ChangeObject(item.Name, "Object");
                    }
                    else // They are seeds
                    {
                        SeedTool seedTool = (GameObject.Find("Tools").transform.Find("Seed").GetComponent("SeedTool") as SeedTool);
                        seedTool.SeedName = item.Use;
                        seedTool.Remaining = item.Amount;
                        seedTool.TakeTool();
                    }
                }
                
                Items[itemID] = null;

                bool hasItems = false;
                for (int i = 0; i < 4; i++)
                {
                    if (Items[i] != null)
                    {
                        hasItems = true;
                        break;
                    }
                }
                if (!hasItems) GameObject.Find("UI").transform.Find("Take storage").gameObject.SetActive(true);

                int slotID = itemID + 1;
                SetUIButton(StorageUI.transform.Find("Slot " + slotID).gameObject.GetComponent<Button>(), itemID, false);
                StorageUI.transform.Find("Slot " + slotID).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Add");
            }
        }

        public void PutItem(int itemID)
        {
            GameObject StorageUI = GameObject.Find("UI").transform.Find("Storage").gameObject;
            if ((StorageUI.GetComponent("StorageHandler") as StorageHandler).Box != BoxObject) return;

            if (Inventory.InventorySlot.activeSelf)
            {
                int slotNumber = itemID + 1;
                if (Inventory.ObjectInHand != null && Inventory.ObjectInHand is Letter)
                {
                    Items[itemID] = Inventory.ObjectInHand;
                    GameObject.Find("UI").transform.Find("Open letter").gameObject.SetActive(false);
                    GameObject.Find("UI").transform.Find("Letter").gameObject.SetActive(false);
                    Inventory.ChangeObject("", "None");
                    Inventory.ObjectInHand = null;
                }
                else if (ItemsHandler.ItemsList.Find(x => x.Name == Inventory.InventoryText.text) != null)
                {
                    Items[itemID] = ItemsHandler.ItemsList.Find(x => x.Name == Inventory.InventoryText.text);
                    ItemsHandler.Item item = (ItemsHandler.Item)Items[itemID];
                    if (item.Name == "Shop tile") item.Amount = PlayerTools.TilesAmount;
                    Inventory.ChangeObject("", "None");
                    GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(false);
                }
                else if (PlayerTools.ToolOnHand != null && PlayerTools.ToolOnHand.Name == "Seed")
                {
                    Items[itemID] = ItemsHandler.ItemsList.Find(x => x.Name == (GameObject.Find("Tools").transform.Find("Seed").GetComponent("SeedTool") as SeedTool).SeedName + " seeds");
                    ItemsHandler.Item item = (ItemsHandler.Item)Items[itemID];
                    item.Amount = PlayerTools.ToolOnHand.Remaining;
                    PlayerTools.ToolOnHand.LetTool();
                }
                else return;

                GameObject.Find("UI").transform.Find("Take storage").gameObject.SetActive(false);
                SetUIButton(StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), itemID, true);
                if (Items[itemID] is Letter)
                {
                    Letter letter = (Letter)Items[itemID];                     
                    StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
                }
                else
                {
                    ItemsHandler.Item item = (ItemsHandler.Item)Items[itemID];
                    StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + item.Name);
                }                
            }
        }

        public void RemoveBox()
        {
            StorageList.Remove(this);
            Destroy(BoxObject);
        }
    }
}