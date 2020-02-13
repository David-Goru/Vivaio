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
        public ItemsHandler.Item[] Items;
        public GameObject BoxObject;

        public Box(GameObject box)
        {
            BoxObject = box;
            Items = new ItemsHandler.Item[4];
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
                        StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + Items[i].Name);
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
                if (Items[itemID].Use == "Build")
                {
                    if (Items[itemID].Name == "Shop tile") PlayerTools.TilesAmount = Items[itemID].Amount;
                    (GameObject.Find("Farm handler").GetComponent("Build") as Build).ObjectName = Items[itemID].Name;
                    GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(true);
                    Inventory.ChangeObject(Items[itemID].Name, "Object");
                }
                else if (Items[itemID].Use == "Farm") // Drip bottle
                {
                    Inventory.ChangeObject(Items[itemID].Name, "Object");
                }
                else // They are seeds
                {
                    SeedTool seedTool = (GameObject.Find("Tools").transform.Find("Seed").GetComponent("SeedTool") as SeedTool);
                    seedTool.SeedName = Items[itemID].Use;
                    seedTool.Remaining = Items[itemID].Amount;
                    seedTool.TakeTool();
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
                if (ItemsHandler.ItemsList.Find(x => x.Name == Inventory.InventoryText.text) != null)
                {
                    Items[itemID] = ItemsHandler.ItemsList.Find(x => x.Name == Inventory.InventoryText.text);
                    if (Items[itemID].Name == "Shop tile") Items[itemID].Amount = PlayerTools.TilesAmount;
                    Inventory.ChangeObject("", "None");
                    GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(false);
                }
                else if (PlayerTools.ToolOnHand != null && PlayerTools.ToolOnHand.Name == "Seed")
                {
                    Items[itemID] = ItemsHandler.ItemsList.Find(x => x.Name == (GameObject.Find("Tools").transform.Find("Seed").GetComponent("SeedTool") as SeedTool).SeedName + " seeds");
                    Items[itemID].Amount = PlayerTools.ToolOnHand.Remaining;
                    PlayerTools.ToolOnHand.LetTool();
                }
                else return;

                GameObject.Find("UI").transform.Find("Take storage").gameObject.SetActive(false);
                SetUIButton(StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), itemID, true);
                StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + Items[itemID].Name);
            }
        }

        public void RemoveBox()
        {
            StorageList.Remove(this);
            Destroy(BoxObject);
        }
    }
}