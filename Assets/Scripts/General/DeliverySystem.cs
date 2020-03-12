using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliverySystem : MonoBehaviour
{
    public static List<DeliveryBox> DeliveryList;
    public static List<DeliveryPoint> DeliveryPoints;

    public Vector3[] PointPos;

    public class DeliveryBox
    {
        public IObject[] Items;
        public DeliveryPoint Point;
        public GameObject Box;
        public bool Placed;

        public DeliveryBox()
        {            
            Items = new IObject[4];
            Placed = false;
        }

        public void BoxClicked()
        {
            GameObject StorageUI = GameObject.Find("UI").transform.Find("Storage").gameObject;
            if ((StorageUI.GetComponent("StorageHandler") as StorageHandler).Box != Box) return;

            if (Vector2.Distance(GameObject.Find("Player").transform.position, Box.transform.position) <= 1.5f)
            {
                for (int i = 0; i < 4; i++)
                { 
                    int slotNumber = i + 1;
                    if (Items[i] == null) StorageUI.transform.Find("Slot " + slotNumber).gameObject.SetActive(false);
                    else
                    {
                        StorageUI.transform.Find("Slot " + slotNumber).gameObject.SetActive(true);
                        StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + Items[i].Name);
                        SetUIButton(StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), i);
                    }
                }

                StorageUI.SetActive(true);
            }
        }    

        public void SetUIButton(Button button, int i)  
        {
            button.onClick.AddListener(() => TakeItem(i));
        }  

        public void TakeItem(int itemID)
        {
            GameObject StorageUI = GameObject.Find("UI").transform.Find("Storage").gameObject;
            if ((StorageUI.GetComponent("StorageHandler") as StorageHandler).Box != Box) return;

            if (!Inventory.InventorySlot.activeSelf)
            {
                Inventory.ObjectInHand = Items[itemID];
                Inventory.ChangeObject();                
                Items[itemID] = null;

                int count = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Items[i] != null)
                    { 
                        count++;
                        break;
                    }
                }

                if (count == 0)
                {
                    RemoveBox();
                    StorageUI.SetActive(false);
                }
                else 
                {
                    int slotID = itemID + 1;
                    StorageUI.transform.Find("Slot " + slotID).gameObject.SetActive(false);
                }
            }
        }

        public void RemoveBox()
        {
            if (Point.Pos == Box.transform.position)
                Point.Available = true;
            else if (Physics2D.OverlapPoint(Box.transform.position, 1 << LayerMask.NameToLayer("Farmland")))
                Physics2D.OverlapPoint(Box.transform.position, 1 << LayerMask.NameToLayer("Farmland")).gameObject.name = "Grass";
            Destroy(Box);
            DeliveryList.Remove(this);
        }
    }

    public class DeliveryPoint
    {
        public Vector3 Pos;
        public bool Available;

        public DeliveryPoint(Vector3 pos)
        {
            Pos = pos;
            Available = true;
        }
    }

    void Start()
    {
        DeliveryList = new List<DeliveryBox>();
        DeliveryPoints = new List<DeliveryPoint>();

        for (int i = 0; i < PointPos.Length; i++)
        {
            DeliveryPoints.Add(new DeliveryPoint(PointPos[i]));
        }
    }

    public void UpdatePackages()
    {
        foreach (DeliveryBox box in DeliveryList)
        {
            if (box.Placed == false)
            {
                DeliveryPoint point = null;
                List<DeliveryPoint> pointsChecked = new List<DeliveryPoint>();

                while (point == null && pointsChecked.Count < DeliveryPoints.Count)
                {
                    DeliveryPoint pointToCheck = null;
                    while (pointToCheck == null)
                    {
                        pointToCheck = DeliveryPoints[Random.Range(0, DeliveryPoints.Count)];
                        if (pointsChecked.Contains(pointToCheck)) pointToCheck = null;
                        else pointsChecked.Add(pointToCheck);
                    }

                    if (pointToCheck.Available) point = pointToCheck;
                }

                if (point != null)
                {
                    point.Available = false;
                    box.Point = point;
                    box.Box = Instantiate(Resources.Load<GameObject>("Delivery box"), point.Pos, transform.rotation);
                    box.Box.name = "Delivery box";
                    box.Placed = true;
                }
                else break;
            }
        }
    }
}