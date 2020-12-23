using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SeedBox : BuildableObject
{    
    [SerializeField]
    public Seed[] Seeds;

    public SeedBox(string translationKey) : base("Seed box", 1, 1, translationKey)
    {
        Seeds = new Seed[8];
    }

    public void ClickSlot(int pos)
    {
        if (Inventory.Data.ObjectInHand is Seed)
        {
            Seed s = (Seed)Inventory.Data.ObjectInHand;
            if (Seeds[pos] == null)
            {
                Seeds[pos] = s;
                Seeds[pos].MaxStack = 25;
                Model.transform.Find("Slots").Find("Slot " + pos).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Farm/Seeds box/" + s.Type + " bag");
                Model.transform.Find("Slots").Find("Slot " + pos).gameObject.SetActive(true);
                Inventory.RemoveObject();
            }
            else if (Seeds[pos].Type == s.Type)
            {
                if (Seeds[pos].Stack < Seeds[pos].MaxStack)
                {
                    int amount = s.Stack;
                    if (Seeds[pos].Stack + amount <= Seeds[pos].MaxStack)
                    {
                        Seeds[pos].Stack += amount;
                        Inventory.RemoveObject();
                    }
                    else
                    {
                        amount = Seeds[pos].MaxStack - Seeds[pos].Stack;
                        Seeds[pos].Stack = Seeds[pos].MaxStack;
                        s.Stack -= amount;
                        Inventory.ChangeObject();
                    }
                }
                else if (s.Stack < s.MaxStack)
                {
                    int amount = s.MaxStack - s.Stack;
                    if (amount < Seeds[pos].Stack)
                    {
                        s.Stack += amount;
                        Seeds[pos].Stack -= amount;
                    }
                    else
                    {
                        s.Stack += Seeds[pos].Stack;
                        Seeds[pos] = null;
                        Model.transform.Find("Slots").Find("Slot " + pos).gameObject.SetActive(false);
                    }
                    Inventory.ChangeObject();
                }
            }
        }
        else if (Inventory.Data.ObjectInHand == null && Seeds[pos] != null)
        {
            Seed s;
            if (Seeds[pos].Stack > 10)
            {
                Seeds[pos].Stack -= 10;
                s = new Seed(Seeds[pos].Type, 10, 10, Seeds[pos].Type + "Seeds");
            }
            else
            {
                s = new Seed(Seeds[pos].Type, Seeds[pos].Stack, 10, Seeds[pos].Type + "Seeds");
                Seeds[pos] = null;
                Model.transform.Find("Slots").Find("Slot " + pos).gameObject.SetActive(false);
            }
            Inventory.AddObject(s);
        }
    }

    public void ClickSeedsSlot(int pos)
    {
        ClickSlot(pos);
        if (Seeds[pos] == null)
        {
            UI.Elements["Seed box seed bag slot " + pos].SetActive(false);
            UI.Elements["Seed box amount slot " + pos].GetComponent<Text>().text = Localization.Translations["Empty"];
            UI.Elements["Seed box slot " + pos].GetComponent<Image>().enabled = true;

            bool canTakeBox = true;
            for (int j = 0; j < 8; j++)
            {
                if (Seeds[j] != null)
                {
                    canTakeBox = false;
                    break;
                }
            }
            
            UI.Elements["Seed box take object button"].SetActive(canTakeBox);
        }
        else
        {
            UI.Elements["Seed box seed bag slot " + pos].GetComponent<Image>().sprite = UI.Sprites[Seeds[pos].Name];
            UI.Elements["Seed box seed bag slot " + pos].SetActive(true);
            UI.Elements["Seed box amount slot " + pos].GetComponent<Text>().text = Seeds[pos].Stack + "/" + Seeds[pos].MaxStack;
            UI.Elements["Seed box slot " + pos].GetComponent<Image>().enabled = false;
            UI.Elements["Seed box take object button"].SetActive(false);
        }
    }

    public override void ActionTwoHard()
    {
        if (!(Inventory.Data.ObjectInHand is Seed)) return;

        Seed s = (Seed)Inventory.Data.ObjectInHand;

        // First we check the slots with the same seeds
        for (int i = 0; i < Seeds.Length; i++)
        {
            if (Seeds[i] != null && Seeds[i].Type == s.Type)
            {
                ClickSlot(i);
                if (!(Inventory.Data.ObjectInHand is Seed))
                {
                    if (UI.ObjectOnUI == this && UI.Elements["Seed box"].activeSelf) OpenUI();
                    return;
                }
            }
        }

        // Then we check the slots with the different seeds
        for (int i = 0; i < Seeds.Length; i++)
        {
            if (Seeds[i] == null)
            {
                ClickSlot(i);
                if (!(Inventory.Data.ObjectInHand is Seed))
                {
                    if (UI.ObjectOnUI == this && UI.Elements["Seed box"].activeSelf) OpenUI();
                    return;
                }
            }
        }
    }

    public override void ActionTwo()
    {
        UI.OpenNewObjectUI(this);
    }

    public override void LoadObjectCustom()
    {
        for (int i = 0; i < Seeds.Length; i++)
        {
            if (Seeds[i] != null) 
            {
                Model.transform.Find("Slots").Find("Slot " + i).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Farm/Seeds box/" + Seeds[i].Type + " bag");
                Model.transform.Find("Slots").Find("Slot " + i).gameObject.SetActive(true);
            }
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        bool canTakeBox = true;

        for (int i = 0; i < 8; i++)
        {
            int pos = i;

            UI.Elements["Seed box slot " + pos].GetComponent<Button>().onClick.RemoveAllListeners();
            UI.Elements["Seed box slot " + pos].GetComponent<Button>().onClick.AddListener(() => ClickSeedsSlot(pos));

            if (Seeds[pos] == null)
            {
                UI.Elements["Seed box seed bag slot " + pos].SetActive(false);
                UI.Elements["Seed box amount slot " + pos].GetComponent<Text>().text = Localization.Translations["Empty"];
                UI.Elements["Seed box slot " + pos].GetComponent<Image>().enabled = true;
            }
            else
            {
                canTakeBox = false;
                UI.Elements["Seed box seed bag slot " + pos].GetComponent<Image>().sprite = UI.Sprites[Seeds[pos].Name];
                UI.Elements["Seed box seed bag slot " + pos].SetActive(true);
                UI.Elements["Seed box amount slot " + pos].GetComponent<Text>().text = Seeds[pos].Stack + "/" + Seeds[pos].MaxStack;
                UI.Elements["Seed box slot " + pos].GetComponent<Image>().enabled = false;
            }
        }

        UI.Elements["Seed box take object button"].SetActive(canTakeBox);
        UI.Elements["Seed box"].SetActive(true); 
    }

    public override void CloseUI()
    {
        UI.Elements["Seed box"].SetActive(false);
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Seed box take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());  
    }
}