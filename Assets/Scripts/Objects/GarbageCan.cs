using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GarbageCan : BuildableObject
{
    [System.NonSerialized]
    public List<IObject> Items;

    public GarbageCan(string translationKey) : base("Garbage can", 1, 1, translationKey)
    {
        Items = new List<IObject>();
    }

    public void ThrowItem()
    {
        if (Inventory.Data.ObjectInHand != null && !(Inventory.Data.ObjectInHand is Tool))
        {
            IObject trash = Inventory.Data.ObjectInHand;
            Items.Add(trash);

            GameObject itemUI = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Trash"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            itemUI.transform.SetParent(UI.Elements["Garbage can objects list"].transform, false);
            itemUI.transform.SetAsFirstSibling();
            itemUI.transform.Find("Image").GetComponent<Image>().sprite = trash.GetUISprite();
            itemUI.transform.Find("Name").GetComponent<Text>().text = trash.GetUIName();
            itemUI.GetComponent<Button>().onClick.AddListener(() => GetItemBack(trash, itemUI));

            Inventory.RemoveObject();
        }
    }

    public void GetItemBack(IObject item, GameObject trashIcon)
    {
        int amountTaken = Inventory.AddObject(item);
        if (amountTaken == item.Stack)
        {
            Items.Remove(item);
            MonoBehaviour.Destroy(trashIcon);
        }
        else
        {
            item.Stack -= amountTaken;
            trashIcon.transform.Find("Name").GetComponent<Text>().text = item.GetUIName();
        }       
    }

    public override void ActionTwo()
    {
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Garbage can/Open");
        UI.OpenNewObjectUI(this);
    }

    public override void LoadObjectCustom()
    {
        Items = new List<IObject>();
    }

    // UI stuff
    public override void OpenUI()
    {
        if (Items.Count > 0)
        {
            UI.Elements["Garbage can take object button"].SetActive(false);
            foreach (IObject item in Items)
            {
                GameObject itemUI = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Trash"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                itemUI.transform.SetParent(UI.Elements["Garbage can objects list"].transform, false);
                itemUI.transform.SetAsFirstSibling();
                itemUI.transform.Find("Image").GetComponent<Image>().sprite = item.GetUISprite();
                itemUI.transform.Find("Name").GetComponent<Text>().text = item.GetUIName();
                itemUI.GetComponent<Button>().onClick.AddListener(() => GetItemBack(item, itemUI));
            }
        }
        else UI.Elements["Garbage can take object button"].SetActive(true);
        
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Garbage can/Open");
        UI.Elements["Garbage can"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Garbage can"].SetActive(false);
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Garbage can/Closed");

        foreach (Transform uiItem in UI.Elements["Garbage can objects list"].transform)
        {
            MonoBehaviour.Destroy(uiItem.gameObject);
        }
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Garbage can take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Garbage can throw object"].GetComponent<Button>().onClick.AddListener(() => ThrowItemButton());
    }
    
    public static void ThrowItemButton()
    {
        ((GarbageCan)UI.ObjectOnUI).ThrowItem();
    }
}