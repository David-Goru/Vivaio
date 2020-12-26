using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BottlesRecycler : BuildableObject
{
    [SerializeField]
    public int BottlesAmount;
    [SerializeField]
    public int MaxAmount;

    public BottlesRecycler(string translationKey) : base("Bottles recycler", 1, 1, translationKey)
    {
        BottlesAmount = 0;
        MaxAmount = 18;
    }

    public void TakeBottles()
    {
        int amount = Inventory.AddObject(new IObject("Glass bottle", "", BottlesAmount, 10, "GlassBottle"));
        if (amount > 0)
        {
            BottlesAmount -= amount;
            UI.Elements["Bottles recycler bottles amount"].GetComponent<Text>().text = string.Format("{0}/{1}", BottlesAmount, MaxAmount);
            UI.Elements["Bottles recycler bottles"].GetComponent<Image>().sprite = UI.Sprites[BottlesAmount > 0 ? "Glass bottle" : "None"];
            Model.transform.Find("Bottles").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/Bottle recycler stand/Bottles " + (BottlesAmount - 1));
        }        
    }

    public override void ActionTwoHard()
    {
        TakeBottles();
        if (UI.ObjectOnUI == this && UI.Elements["Bottles recycler"].activeSelf) OpenUI();
    }

    public override void ActionTwo()
    {
        UI.OpenNewObjectUI(this);
    }

    public override void LoadObjectCustom()
    {
        Model.transform.Find("Bottles").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/Bottle recycler stand/Bottles " + (BottlesAmount - 1));
    }

    public void AddBottle(int amount)
    {
        BottlesAmount -= amount;
        Model.transform.Find("Bottles").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/Bottle recycler stand/Bottles " + (BottlesAmount - 1));
        if (UI.ObjectOnUI == this)
        {
            UI.Elements["Bottles recycler bottles amount"].GetComponent<Text>().text = string.Format("{0}/{1}", BottlesAmount, MaxAmount);
            UI.Elements["Bottles recycler bottles"].GetComponent<Image>().sprite = UI.Sprites[BottlesAmount > 0 ? "Glass bottle" : "None"];
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        UI.Elements["Bottles recycler bottles amount"].GetComponent<Text>().text = string.Format("{0}/{1}", BottlesAmount, MaxAmount);
        UI.Elements["Bottles recycler bottles"].GetComponent<Image>().sprite = UI.Sprites[BottlesAmount > 0 ? "Glass bottle" : "None"];
        UI.Elements["Bottles recycler"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Bottles recycler"].SetActive(false);
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Bottles recycler take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Bottles recycler take bottles"].GetComponent<Button>().onClick.AddListener(() => TakeBottlesButton());
    }
    
    public static void TakeBottlesButton()
    {
        ((BottlesRecycler)UI.ObjectOnUI).TakeBottles();
    }
}