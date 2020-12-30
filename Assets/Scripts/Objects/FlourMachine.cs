using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FlourMachine : BuildableObject
{
    [SerializeField]
    public MachineState State;
    [SerializeField]
    public Timer Timer;
    [SerializeField]
    public int MaxAmount;
    [SerializeField]
    public int Amount;
    [SerializeField]
    public int Flour;
    [SerializeField]
    public int Compost;

    public FlourMachine(string translationKey) : base("Flour machine", 1, 1, translationKey)
    {
        State = MachineState.AVAILABLE;
        MaxAmount = 10;
        Flour = 0;
        Compost = 0;
    }

    public void AddWheat()
    {
        if (State != MachineState.AVAILABLE) return;
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return;
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        if (basket.Product != null && basket.Product.Name == "Wheat")
        {
            int needs = MaxAmount - Amount;
            if (needs > basket.Amount)
            {
                Amount += basket.Amount;
                basket.Amount = 0;
                basket.Product = null;
                Inventory.ChangeObject();
                UI.Elements["Flour machine wheat amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Amount / (float)MaxAmount * 5)];
            }
            else
            {
                if (needs == basket.Amount)
                {
                    basket.Amount = 0;
                    basket.Product = null;
                }
                else basket.Amount -= needs;
                Inventory.ChangeObject();

                Amount = 10;
                State = MachineState.WORKING;
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Working");
                OnTimeReached createFlour = CreateFlour;
                Timer = new Timer(createFlour, 60);
                TimeSystem.Data.Timers.Add(Timer);
                UI.Elements["Flour machine available"].SetActive(false);
                UI.Elements["Flour machine working"].SetActive(true);
                UI.Elements["Flour machine finished"].SetActive(false);
            }
        }
    }

    public bool TakeFlour()
    {
        if (State != MachineState.FINISHED) return false;
        if (Flour > 0)
        {
            int amountTaken = Inventory.AddObject(new IObject("Flour", Flour, 10, "Flour"));
            if (amountTaken == Flour)
            {
                Flour = 0;
                UI.Elements["Flour machine take flour image"].GetComponent<Image>().sprite = UI.Sprites["None"];
                UI.Elements["Flour machine take flour amount"].GetComponent<Text>().text = "x 0";

                UI.Elements["Flour machine wheat amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Amount / (float)MaxAmount * 5)];

                if (Compost == 0)
                {
                    Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Available");
                    Amount = 0;
                    State = MachineState.AVAILABLE;
                    Model.transform.Find("Warning").gameObject.SetActive(false);
                    UI.Elements["Flour machine available"].SetActive(true);
                    UI.Elements["Flour machine working"].SetActive(false);
                    UI.Elements["Flour machine finished"].SetActive(false);
                }
                return true;
            }
            else if (amountTaken > 0)
            {
                Flour -= amountTaken;
                UI.Elements["Flour machine take flour amount"].GetComponent<Text>().text = "x " + Flour;
                return true;
            }
        }
        return false;
    }

    public void TakeCompost()
    {
        if (State != MachineState.FINISHED) return;
        if (Compost == 0) return;
        if (Inventory.Data.ObjectInHand != null && Inventory.Data.ObjectInHand is Basket)
        {
            Basket basket = (Basket)Inventory.Data.ObjectInHand;
            if (basket.Amount > 0 && basket.Product.Name != "Sticks") return;
            
            int amount = (10 - basket.Amount > Compost ? Compost : 10 - basket.Amount);

            if (amount == 0) return;

            basket.Product = Products.ProductsList.Find(x => x.Name == "Sticks");
            Compost -= amount;
            basket.Amount += amount;
            Inventory.ChangeObject();
            UI.Elements["Flour machine take compost amount"].GetComponent<Text>().text = "x " + Compost;

            UI.Elements["Flour machine wheat amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Amount / (float)MaxAmount * 5)];

            if (Compost == 0 && Flour == 0)
            {
                UI.Elements["Flour machine take compost image"].GetComponent<Image>().sprite = UI.Sprites["None"];
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Available");
                Amount = 0;
                State = MachineState.AVAILABLE;
                Model.transform.Find("Warning").gameObject.SetActive(false);
                UI.Elements["Flour machine available"].SetActive(true);
                UI.Elements["Flour machine working"].SetActive(false);
                UI.Elements["Flour machine finished"].SetActive(false);
            }
        }
    }

    public void CreateFlour()
    {
        Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Finished");
        State = MachineState.FINISHED;
        Model.transform.Find("Warning").gameObject.SetActive(true);  
        Flour = 5; 
        Compost = Random.Range(1, 4);     
        Master.Data.LastDayEnergyUsage++;
    }

    public override void ActionOne()
    {
        AddWheat();
        if (UI.ObjectOnUI == this && UI.Elements["Flour machine"].activeSelf) OpenUI();
    }

    public override void ActionTwoHard()
    {
        if (!TakeFlour()) TakeCompost();
        if (UI.ObjectOnUI == this && UI.Elements["Flour machine"].activeSelf) OpenUI();
    }

    public override void ActionTwo()
    {
        UI.OpenNewObjectUI(this);
    }

    public override void LoadObjectCustom()
    {
        switch (State)
        {
            case MachineState.WORKING:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Finished");
                Model.transform.Find("Warning").gameObject.SetActive(true);
                break;
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        UI.Elements["Flour machine wheat amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Amount / (float)MaxAmount * 5)];

        switch (State)
        {
            case MachineState.AVAILABLE:
                UI.Elements["Flour machine available"].SetActive(true);
                UI.Elements["Flour machine working"].SetActive(false);
                UI.Elements["Flour machine finished"].SetActive(false);
                break;
            case MachineState.WORKING:
                UI.Elements["Flour machine available"].SetActive(false);
                UI.Elements["Flour machine working"].SetActive(true);
                UI.Elements["Flour machine finished"].SetActive(false);
                break;
            case MachineState.FINISHED:
                string flour = Flour > 0 ? "Flour" : "None";
                string compost = Compost > 0 ? "Sticks" : "None";
                UI.Elements["Flour machine take flour image"].GetComponent<Image>().sprite = UI.Sprites[flour];
                UI.Elements["Flour machine take flour amount"].GetComponent<Text>().text = "x " + Flour;
                UI.Elements["Flour machine take compost image"].GetComponent<Image>().sprite = UI.Sprites[compost];
                UI.Elements["Flour machine take compost amount"].GetComponent<Text>().text = "x " + Compost;

                UI.Elements["Flour machine available"].SetActive(false);
                UI.Elements["Flour machine working"].SetActive(false);
                UI.Elements["Flour machine finished"].SetActive(true);
                break;
        }
        
        UI.Elements["Flour machine"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Flour machine"].SetActive(false);
    }

    public override void UpdateUI()
    {
        if (State == MachineState.WORKING)
        {
            int minutes = Timer.Time;
            UI.Elements["Flour machine working text"].GetComponent<Text>().text = string.Format(Localization.Translations["flour_machine_working"], minutes);
        }
        else if (State == MachineState.FINISHED && !UI.Elements["Flour machine finished"].activeSelf)
        {
            UI.Elements["Flour machine working"].SetActive(false);
            UI.Elements["Flour machine take flour image"].GetComponent<Image>().sprite = UI.Sprites["Flour"];
            UI.Elements["Flour machine take flour amount"].GetComponent<Text>().text = "x 5";
            UI.Elements["Flour machine take compost image"].GetComponent<Image>().sprite = UI.Sprites["Sticks"];
            UI.Elements["Flour machine take compost amount"].GetComponent<Text>().text = "x " + Compost;
            UI.Elements["Flour machine finished"].SetActive(true);
        }
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Flour machine take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Flour machine add wheat"].GetComponent<Button>().onClick.AddListener(() => AddWheatButton());
        UI.Elements["Flour machine take flour"].GetComponent<Button>().onClick.AddListener(() => TakeFlourButton());
        UI.Elements["Flour machine take flour image"].GetComponent<Button>().onClick.AddListener(() => TakeFlourButton());
        UI.Elements["Flour machine take compost"].GetComponent<Button>().onClick.AddListener(() => TakeCompostButton());
        UI.Elements["Flour machine take compost image"].GetComponent<Button>().onClick.AddListener(() => TakeCompostButton());
    }
    
    public static void AddWheatButton()
    {
        ((FlourMachine)UI.ObjectOnUI).AddWheat();
    }
    
    public static void TakeFlourButton()
    {
        ((FlourMachine)UI.ObjectOnUI).TakeFlour();
    }
    
    public static void TakeCompostButton()
    {
        ((FlourMachine)UI.ObjectOnUI).TakeCompost();
    }
}