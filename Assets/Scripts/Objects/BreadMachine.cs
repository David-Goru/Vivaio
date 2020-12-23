using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BreadMachine : BuildableObject
{
    [SerializeField]
    public MachineState State;
    [SerializeField]
    public Timer Timer;
    [SerializeField]
    public int FlourAmount;    
    [SerializeField]
    public int WaterAmount;
    [SerializeField]
    public int DoughAmount;

    public BreadMachine(string translationKey) : base("Bread machine", 1, 1, translationKey)
    {
        State = MachineState.AVAILABLE;
        FlourAmount = 0;
        WaterAmount = 0;
        DoughAmount = 0;
    }

    public void AddFlour()
    {
        if (State != MachineState.AVAILABLE) return;
        if (Inventory.Data.ObjectInHand == null || Inventory.Data.ObjectInHand.Name != "Flour") return;
        
        int needs = 5 - FlourAmount;
        if (needs > Inventory.Data.ObjectInHand.Stack)
        {
            FlourAmount += Inventory.Data.ObjectInHand.Stack;
            Inventory.RemoveObject();
        }
        else
        {            
            if (needs == Inventory.Data.ObjectInHand.Stack) Inventory.RemoveObject();
            else 
            {
                Inventory.Data.ObjectInHand.Stack -= needs;
                Inventory.ChangeObject();
            }

            FlourAmount = 5;

            if (WaterAmount == 10)
            {
                State = MachineState.WORKING;
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Working");
                OnTimeReached createBreadDough = CreateBreadDough;
                Timer = new Timer(createBreadDough, 180);
                TimeSystem.Data.Timers.Add(Timer);
                UI.Elements["Bread machine available"].SetActive(false);
                UI.Elements["Bread machine working"].SetActive(true);
                UI.Elements["Bread machine finished"].SetActive(false);
            }
        }
        UI.Elements["Bread machine flour amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)FlourAmount / (float)5 * 5)];
    }

    public void AddWater()
    {
        if (State != MachineState.AVAILABLE) return;
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is WateringCan)) return;
        WateringCan wc = (WateringCan)Inventory.Data.ObjectInHand;
        
        int needs = 10 - WaterAmount;

        if (needs > wc.Remaining)
        {
            WaterAmount += wc.Remaining;
            wc.Remaining = 0;
            Inventory.ChangeObject();
        }
        else
        {
            wc.Remaining -= needs;
            Inventory.ChangeObject();
            WaterAmount = 10;

            if (FlourAmount == 5)
            {
                State = MachineState.WORKING;
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Working");
                OnTimeReached createBreadDough = CreateBreadDough;
                Timer = new Timer(createBreadDough, 180);
                TimeSystem.Data.Timers.Add(Timer);
                UI.Elements["Bread machine available"].SetActive(false);
                UI.Elements["Bread machine working"].SetActive(true);
                UI.Elements["Bread machine finished"].SetActive(false);
            }
        }
        UI.Elements["Bread machine water amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)WaterAmount / (float)10 * 5)];
    }

    public void TakeBreadDough()
    {
        if (State != MachineState.FINISHED) return;
        if (DoughAmount > 0)
        {
            int amountTaken = Inventory.AddObject(new IObject("Bread dough", "", DoughAmount, 10, "BreadDough"));
            if (amountTaken == DoughAmount)
            {
                DoughAmount = 0;                
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Available");
                State = MachineState.AVAILABLE;
                Model.transform.Find("Warning").gameObject.SetActive(false);
                UI.Elements["Bread machine take bread dough image"].GetComponent<Image>().sprite = UI.Sprites["None"];
                UI.Elements["Bread machine take bread dough amount"].GetComponent<Text>().text = "x 0";
                UI.Elements["Bread machine flour amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)FlourAmount / (float)5 * 5)];
                UI.Elements["Bread machine water amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)WaterAmount / (float)10 * 5)];
                UI.Elements["Bread machine available"].SetActive(true);
                UI.Elements["Bread machine working"].SetActive(false);
                UI.Elements["Bread machine finished"].SetActive(false);
            }
            else
            {
                DoughAmount -= amountTaken;
                UI.Elements["Bread machine take bread dough amount"].GetComponent<Text>().text = "x " + DoughAmount;
            }
        }
    }

    public void CreateBreadDough()
    {
        Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Finished");
        State = MachineState.FINISHED;
        Model.transform.Find("Warning").gameObject.SetActive(true);  
        DoughAmount = 5;
        FlourAmount = 0;
        WaterAmount = 0;
        Master.Data.LastDayEnergyUsage++;
    }

    public override void ActionOne()
    {
        AddFlour();
        AddWater();
        if (UI.ObjectOnUI == this && UI.Elements["Bread machine"].activeSelf) OpenUI();
    }

    public override void ActionTwoHard()
    {
        TakeBreadDough();
        if (UI.ObjectOnUI == this && UI.Elements["Bread machine"].activeSelf) OpenUI();
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
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Finished");
                Model.transform.Find("Warning").gameObject.SetActive(true);
                break;
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        UI.Elements["Bread machine flour amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)FlourAmount / (float)5 * 5)];
        UI.Elements["Bread machine water amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)WaterAmount / (float)10 * 5)];

        switch (State)
        {
            case MachineState.AVAILABLE:
                UI.Elements["Bread machine available"].SetActive(true);
                UI.Elements["Bread machine working"].SetActive(false);
                UI.Elements["Bread machine finished"].SetActive(false);
                break;
            case MachineState.WORKING:
                UI.Elements["Bread machine available"].SetActive(false);
                UI.Elements["Bread machine working"].SetActive(true);
                UI.Elements["Bread machine finished"].SetActive(false);
                break;
            case MachineState.FINISHED:
                UI.Elements["Bread machine take bread dough image"].GetComponent<Image>().sprite = UI.Sprites["Bread dough"];
                UI.Elements["Bread machine take bread dough amount"].GetComponent<Text>().text = "x " + DoughAmount;
                
                UI.Elements["Bread machine available"].SetActive(false);
                UI.Elements["Bread machine working"].SetActive(false);
                UI.Elements["Bread machine finished"].SetActive(true);
                break;
        }

        UI.Elements["Bread machine"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Bread machine"].SetActive(false);
    }

    public override void UpdateUI()
    {
        if (State == MachineState.WORKING)
        {
            int minutes = Timer.Time;
            UI.Elements["Bread machine working text"].GetComponent<Text>().text = string.Format(Localization.Translations["bread_machine_working"], minutes);
        }
        else if (State == MachineState.FINISHED && !UI.Elements["Bread machine finished"].activeSelf)
        {
            UI.Elements["Bread machine working"].SetActive(false);
            UI.Elements["Bread machine take bread dough image"].GetComponent<Image>().sprite = UI.Sprites["Bread dough"];
            UI.Elements["Bread machine take bread dough amount"].GetComponent<Text>().text = "x 5";
            UI.Elements["Bread machine finished"].SetActive(true);
        }
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Bread machine take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Bread machine add flour"].GetComponent<Button>().onClick.AddListener(() => AddFlourButton());
        UI.Elements["Bread machine add water"].GetComponent<Button>().onClick.AddListener(() => AddWaterButton());
        UI.Elements["Bread machine take bread dough"].GetComponent<Button>().onClick.AddListener(() => TakeBreadDoughButton());
        UI.Elements["Bread machine take bread dough image"].GetComponent<Button>().onClick.AddListener(() => TakeBreadDoughButton());
    }
    
    public static void AddFlourButton()
    {
        ((BreadMachine)UI.ObjectOnUI).AddFlour();
    }
    
    public static void AddWaterButton()
    {
        ((BreadMachine)UI.ObjectOnUI).AddWater();
    }
    
    public static void TakeBreadDoughButton()
    {
        ((BreadMachine)UI.ObjectOnUI).TakeBreadDough();
    }
}