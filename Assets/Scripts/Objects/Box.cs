using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Box : BuildableObject
{
    [SerializeField]
    public IObject[] Items;
    [SerializeField]
    public bool IsDeliveryBox;
    [SerializeField]
    public DeliveryPoint Point;

    public Box(string name, string translationKey) : base(name, 1, 1, translationKey)
    {
        if (name == "Delivery box" || name == "Present box") IsDeliveryBox = true;
        else IsDeliveryBox = false;
        Items = new IObject[4];        
        Point = null;
    }

    public void SetUIButton(Button button, int i, bool take)  
    {
        button.onClick.RemoveAllListeners();
        if (take) button.onClick.AddListener(() => TakeItem(i));
        else button.onClick.AddListener(() => PutItem(i));
    }  

    public void TakeItem(int itemID)
    {
        GameObject storageUI = GameObject.Find("UI").transform.Find("Storage").gameObject;

        if (Inventory.AddObject(Items[itemID]))
        {
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
            if (!hasItems)
            {
                if (IsDeliveryBox)
                {   
                    RemoveBox();
                    ObjectUI.CloseUIs();
                    return;
                }
                storageUI.transform.Find("Take storage").gameObject.SetActive(true);
            }

            int slotID = itemID + 1;
            if (IsDeliveryBox) storageUI.transform.Find("Slot " + slotID).gameObject.SetActive(false);
            else
            {
                SetUIButton(storageUI.transform.Find("Slot " + slotID).gameObject.GetComponent<Button>(), itemID, false);
                storageUI.transform.Find("Slot " + slotID).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Add");
            }
        }
    }

    public void PutItem(int itemID)
    {
        GameObject storageUI = GameObject.Find("UI").transform.Find("Storage").gameObject;

        if (Inventory.Data.ObjectInHand != null && !(Inventory.Data.ObjectInHand is Tool))
        {
            int slotNumber = itemID + 1;
            Items[itemID] = Inventory.Data.ObjectInHand;
            Inventory.RemoveObject();

            storageUI.transform.Find("Take storage").gameObject.SetActive(false);
            SetUIButton(storageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), itemID, true);
            if (Items[itemID] is Letter)
            {
                Letter letter = (Letter)Items[itemID];                     
                storageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
            }
            else if (Items[itemID].Name == "Drip bottle")
                storageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Drip bottle/" + ((DripBottle)Items[itemID]).WaterUnits);
            else storageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + Items[itemID].Name);
            
        }
    }

    public void UpdatePoint()
    {
        if (Point != null)
        {
            Point.Available = true;
            Point = null;
        }
    }

    public void RemoveBox()
    {
        if (Point != null) Point.Available = true;
        ObjectsHandler.Data.Objects.Remove(this);
        foreach (Transform t in Model.transform.Find("Vertices"))
        {                            
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            if (v != null) v.State = VertexState.Available;
        }
        MonoBehaviour.Destroy(Model);
    }

    public override void ActionTwo()
    {
        ObjectUI.OpenUI(this);
    }
}