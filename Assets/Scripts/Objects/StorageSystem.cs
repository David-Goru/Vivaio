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
                        else if (Items[i] is Seed) StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + Items[i].Name);
                        else StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + Items[i].Name);

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
                Inventory.ObjectInHand = Items[itemID];
                Inventory.ChangeObject();            
                
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

            if (Inventory.ObjectInHand != null && !(Inventory.ObjectInHand is Tool))
            {
                int slotNumber = itemID + 1;
                Items[itemID] = Inventory.ObjectInHand;
                Inventory.RemoveObject();

                GameObject.Find("UI").transform.Find("Take storage").gameObject.SetActive(false);
                SetUIButton(StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), itemID, true);
                if (Items[itemID] is Letter)
                {
                    Letter letter = (Letter)Items[itemID];                     
                    StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
                }
                else StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + Items[itemID].Name);
              
            }
        }

        public void RemoveBox()
        {
            StorageList.Remove(this);
            Destroy(BoxObject);
        }
    }
}