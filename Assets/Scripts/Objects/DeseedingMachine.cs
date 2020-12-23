using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DeseedingMachine : BuildableObject
{
    [SerializeField]
    public MachineState State;
    [SerializeField]
    public Timer Timer;
    [SerializeField]
    public int Seeds;
    [SerializeField]
    public int Compost;
    [SerializeField]
    public string SeedType;

    public DeseedingMachine(string translationKey) : base("Deseeding machine", 1, 1, translationKey)
    {
        State = MachineState.AVAILABLE;
    }

    public void AddSeed()
    {
        if (State != MachineState.AVAILABLE) return;
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return;
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        if (basket.Product != null && basket.Product.Name != "Sticks")
        {
            SeedType = basket.Product.Name;
            basket.Amount--;
            if (basket.Amount == 0) basket.Product = null;
            Inventory.ChangeObject();

            State = MachineState.WORKING;
            Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Working");
            OnTimeReached createSeeds = CreateSeeds;
            Timer = new Timer(createSeeds, 1200);
            TimeSystem.Data.Timers.Add(Timer);
            
            UI.Elements["Deseeding machine available"].SetActive(false);
            UI.Elements["Deseeding machine working"].SetActive(true);
            UI.Elements["Deseeding machine finished"].SetActive(false);
        }
    }

    public bool TakeSeeds()
    {
        if (State != MachineState.FINISHED) return false;
        if (Seeds == 0) return false;
        int amountTaken = Inventory.AddObject(new Seed(SeedType, Seeds, 10, SeedType + "Seeds"));

        if (amountTaken == Seeds)
        {
            Seeds = 0;

            UI.Elements["Deseeding machine take seeds image"].GetComponent<Image>().sprite = UI.Sprites["None"];
            UI.Elements["Deseeding machine take seeds amount"].GetComponent<Text>().text = "x 0";

            if (Compost == 0)
            {
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Available");
                State = MachineState.AVAILABLE;
                Model.transform.Find("Warning").gameObject.SetActive(false);
                UI.Elements["Deseeding machine available"].SetActive(true);
                UI.Elements["Deseeding machine working"].SetActive(false);
                UI.Elements["Deseeding machine finished"].SetActive(false);
            }
            return true;
        }
        else if (amountTaken > 0)
        {
            Seeds -= amountTaken;
            UI.Elements["Deseeding machine take seeds amount"].GetComponent<Text>().text = "x " + Seeds;
            return true;
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

            UI.Elements["Deseeding machine take compost amount"].GetComponent<Text>().text = "x " + Compost;

            if (Compost == 0 && Seeds == 0)
            {
                UI.Elements["Deseeding machine take compost image"].GetComponent<Image>().sprite = UI.Sprites["None"];
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Available");
                State = MachineState.AVAILABLE;
                Model.transform.Find("Warning").gameObject.SetActive(false);

                UI.Elements["Deseeding machine available"].SetActive(true);
                UI.Elements["Deseeding machine working"].SetActive(false);
                UI.Elements["Deseeding machine finished"].SetActive(false);
            }
        }
    }

    public void CreateSeeds()
    {
        Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Finished");
        State = MachineState.FINISHED;
        Model.transform.Find("Warning").gameObject.SetActive(true);
        Seeds = Random.Range(3, 7);
        Compost = Random.Range(1, 4);
        Master.Data.LastDayEnergyUsage++;
    }

    public override void ActionOne()
    {
        AddSeed();
        if (UI.ObjectOnUI == this && UI.Elements["Deseeding machine"].activeSelf) OpenUI();
    }

    public override void ActionTwoHard()
    {
        if (!TakeSeeds()) TakeCompost();
        if (UI.ObjectOnUI == this && UI.Elements["Deseeding machine"].activeSelf) OpenUI();
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
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Finished");
                Model.transform.Find("Warning").gameObject.SetActive(true);
                break;
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        switch (State)
        {
            case MachineState.AVAILABLE:
                UI.Elements["Deseeding machine available"].SetActive(true);
                UI.Elements["Deseeding machine working"].SetActive(false);
                UI.Elements["Deseeding machine finished"].SetActive(false);
                break;
            case MachineState.WORKING:
                UI.Elements["Deseeding machine available"].SetActive(false);
                UI.Elements["Deseeding machine working"].SetActive(true);
                UI.Elements["Deseeding machine finished"].SetActive(false);
                break;
            case MachineState.FINISHED:
                string seeds = Seeds > 0 ? SeedType + " seeds" : "None";
                string compost = Compost > 0 ? "Sticks" : "None";
                UI.Elements["Deseeding machine take seeds image"].GetComponent<Image>().sprite = UI.Sprites[seeds];
                UI.Elements["Deseeding machine take seeds amount"].GetComponent<Text>().text = "x " + Seeds;
                UI.Elements["Deseeding machine take compost image"].GetComponent<Image>().sprite = UI.Sprites[compost];
                UI.Elements["Deseeding machine take compost amount"].GetComponent<Text>().text = "x " + Compost;

                UI.Elements["Deseeding machine available"].SetActive(false);
                UI.Elements["Deseeding machine working"].SetActive(false);
                UI.Elements["Deseeding machine finished"].SetActive(true);
                break;
        }

        UI.Elements["Deseeding machine"].SetActive(true); 
    }

    public override void CloseUI()
    {
        UI.Elements["Deseeding machine"].SetActive(false);
    }

    public override void UpdateUI()
    {
        if (State == MachineState.WORKING)
        {
            int hours = (int)Timer.Time / 60;
            int minutes = Timer.Time - hours * 60;
            UI.Elements["Deseeding machine working text"].GetComponent<Text>().text = string.Format(Localization.Translations["deseeding_machine_working"], SeedType, hours, minutes);
        }
        else if (State == MachineState.FINISHED && !UI.Elements["Deseeding machine finished"].activeSelf)
        {
            UI.Elements["Deseeding machine working"].SetActive(false);
            UI.Elements["Deseeding machine take seeds image"].GetComponent<Image>().sprite = UI.Sprites[SeedType + " seeds"];
            UI.Elements["Deseeding machine take seeds amount"].GetComponent<Text>().text = "x " + Seeds;
            UI.Elements["Deseeding machine take compost image"].GetComponent<Image>().sprite = UI.Sprites["Sticks"];
            UI.Elements["Deseeding machine take compost amount"].GetComponent<Text>().text = "x " + Compost;
            UI.Elements["Deseeding machine finished"].SetActive(true);
        }
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Deseeding machine take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Deseeding machine add seed"].GetComponent<Button>().onClick.AddListener(() => AddSeedButton());
        UI.Elements["Deseeding machine take seeds"].GetComponent<Button>().onClick.AddListener(() => TakeSeedsButton());
        UI.Elements["Deseeding machine take seeds image"].GetComponent<Button>().onClick.AddListener(() => TakeSeedsButton());
        UI.Elements["Deseeding machine take compost"].GetComponent<Button>().onClick.AddListener(() => TakeCompostButton());
        UI.Elements["Deseeding machine take compost image"].GetComponent<Button>().onClick.AddListener(() => TakeCompostButton());
    }
    
    public static void AddSeedButton()
    {
        ((DeseedingMachine)UI.ObjectOnUI).AddSeed();
    }
    
    public static void TakeSeedsButton()
    {
        ((DeseedingMachine)UI.ObjectOnUI).TakeSeeds();
    }
    
    public static void TakeCompostButton()
    {
        ((DeseedingMachine)UI.ObjectOnUI).TakeCompost();
    }
}