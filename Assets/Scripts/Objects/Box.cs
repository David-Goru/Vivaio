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
        int amountTaken = Inventory.AddObject(Items[itemID]);
        if (amountTaken == Items[itemID].Stack)
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
                    CloseUI();
                    UI.ObjectOnUI = null;
                    RemoveBox();
                    return;
                }
                UI.Elements["Storage take object button"].SetActive(true);
            }

            int slotNumber = itemID + 1;
            if (IsDeliveryBox) UI.Elements["Storage slot " + slotNumber].SetActive(false);
            else
            {
                SetUIButton(UI.Elements["Storage slot " + slotNumber].GetComponent<Button>(), itemID, false);
                UI.Elements["Storage slot " + slotNumber].GetComponent<Image>().sprite = UI.Sprites["Add"];
            }
        }
        else
        {
            Items[itemID].Stack -= amountTaken;
        }
    }

    public void PutItem(int itemID)
    {
        if (Inventory.Data.ObjectInHand != null && !(Inventory.Data.ObjectInHand is Tool))
        {
            int slotNumber = itemID + 1;
            Items[itemID] = Inventory.Data.ObjectInHand;
            Inventory.RemoveObject();

            UI.Elements["Storage take object button"].SetActive(false);
            SetUIButton(UI.Elements["Storage slot " + slotNumber].GetComponent<Button>(), itemID, true);
            UI.Elements["Storage slot " + slotNumber].GetComponent<Image>().sprite = Items[itemID].GetUISprite();
            
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
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null) v.State = VertexState.Available;
        }
        MonoBehaviour.Destroy(Model);
    }

    public override void ActionTwo()
    {
        UI.OpenNewObjectUI(this);
    }

    public override void LoadObjectCustom()
    {
        if (IsDeliveryBox && Master.GameEdition == "Christmas")
        {
            if (Name == "Present box") Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = VersionHandlerGame.PresentChristmas;
            else Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = VersionHandlerGame.DeliveryBoxChristmas;
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        bool hasItems = false;
        for (int i = 0; i < 4; i++)
        { 
            int slotNumber = i + 1;
            UI.Elements["Storage slot " + slotNumber].SetActive(true);
            if (Items[i] == null)
            {
                if (IsDeliveryBox) UI.Elements["Storage slot " + slotNumber].SetActive(false);
                else
                {
                    SetUIButton(UI.Elements["Storage slot " + slotNumber].GetComponent<Button>(), i, false);
                    UI.Elements["Storage slot " + slotNumber].GetComponent<Image>().sprite = UI.Sprites["Add"];                        
                }
            }
            else
            {
                hasItems = true;
                UI.Elements["Storage slot " + slotNumber].GetComponent<Image>().sprite = Items[i].GetUISprite();
                SetUIButton(UI.Elements["Storage slot " + slotNumber].GetComponent<Button>(), i, true);
            }
        }

        if (!hasItems && !IsDeliveryBox) UI.Elements["Storage take object button"].SetActive(true);
        else UI.Elements["Storage take object button"].SetActive(false);

        UI.Elements["Storage"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Storage"].SetActive(false);
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Storage take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
    }
}