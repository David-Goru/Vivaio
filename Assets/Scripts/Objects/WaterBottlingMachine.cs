using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WaterBottlingMachine : BuildableObject
{
    [SerializeField]
    public MachineState State;
    [SerializeField]
    public Timer Timer;

    public WaterBottlingMachine(string translationKey) : base("Water bottling machine", 1, 1, translationKey)
    {
        State = MachineState.AVAILABLE;
    }

    public void AddBottle()
    {
        if (State != MachineState.AVAILABLE) return;
        if (Inventory.Data.ObjectInHand == null || Inventory.Data.ObjectInHand.Name != "Glass bottle") return;

        Inventory.RemoveObject(1);
        State = MachineState.WORKING;
        Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Water bottling machine/Working");
        OnTimeReached createWaterBottle = CreateWaterBottle;
        Timer = new Timer(createWaterBottle, 20);
        TimeSystem.Data.Timers.Add(Timer);
        UI.Elements["Water bottling machine available"].SetActive(false);
        UI.Elements["Water bottling machine working"].SetActive(true);
        UI.Elements["Water bottling machine finished"].SetActive(false);
    }

    public void TakeWaterBottle()
    {
        if (State != MachineState.FINISHED) return;
        if (Inventory.AddObject(new IObject("Water bottle", "", 1, 10, "WaterBottle")) > 0)
        {             
            Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Water bottling machine/Available");
            State = MachineState.AVAILABLE;
            Model.transform.Find("Warning").gameObject.SetActive(false);
            UI.Elements["Water bottling machine take water bottle image"].GetComponent<Image>().sprite = UI.Sprites["None"];
            UI.Elements["Water bottling machine take water bottle amount"].GetComponent<Text>().text = "x 0";
            UI.Elements["Water bottling machine available"].SetActive(true);
            UI.Elements["Water bottling machine working"].SetActive(false);
            UI.Elements["Water bottling machine finished"].SetActive(false);
        }
    }

    public void CreateWaterBottle()
    {
        Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Water bottling machine/Finished");
        State = MachineState.FINISHED;
        Model.transform.Find("Warning").gameObject.SetActive(true);
        Master.Data.LastDayEnergyUsage++;
    }

    public override void ActionOne()
    {
        AddBottle();
        if (UI.ObjectOnUI == this && UI.Elements["Water bottling machine"].activeSelf) OpenUI();
    }

    public override void ActionTwoHard()
    {
        TakeWaterBottle();
        if (UI.ObjectOnUI == this && UI.Elements["Water bottling machine"].activeSelf) OpenUI();
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
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Water bottling machine/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Water bottling machine/Finished");
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
                UI.Elements["Water bottling machine available"].SetActive(true);
                UI.Elements["Water bottling machine working"].SetActive(false);
                UI.Elements["Water bottling machine finished"].SetActive(false);
                break;
            case MachineState.WORKING:
                UI.Elements["Water bottling machine available"].SetActive(false);
                UI.Elements["Water bottling machine working"].SetActive(true);
                UI.Elements["Water bottling machine finished"].SetActive(false);
                break;
            case MachineState.FINISHED:
                UI.Elements["Water bottling machine take water bottle image"].GetComponent<Image>().sprite = UI.Sprites["Water bottle"];
                UI.Elements["Water bottling machine take water bottle amount"].GetComponent<Text>().text = "x 1";
                
                UI.Elements["Water bottling machine available"].SetActive(false);
                UI.Elements["Water bottling machine working"].SetActive(false);
                UI.Elements["Water bottling machine finished"].SetActive(true);
                break;
        }

        UI.Elements["Water bottling machine"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Water bottling machine"].SetActive(false);
    }

    public override void UpdateUI()
    {
        if (State == MachineState.WORKING)
        {
            int minutes = Timer.Time;
            UI.Elements["Water bottling machine working text"].GetComponent<Text>().text = string.Format(Localization.Translations["water_bottling_machine_working"], minutes);
        }
        else if (State == MachineState.FINISHED && !UI.Elements["Water bottling machine finished"].activeSelf)
        {
            UI.Elements["Water bottling machine working"].SetActive(false);
            UI.Elements["Water bottling machine take water bottle image"].GetComponent<Image>().sprite = UI.Sprites["Water bottle"];
            UI.Elements["Water bottling machine take water bottle amount"].GetComponent<Text>().text = "x 1";
            UI.Elements["Water bottling machine finished"].SetActive(true);
        }
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Water bottling machine take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Water bottling machine add bottle"].GetComponent<Button>().onClick.AddListener(() => AddBottleButton());
        UI.Elements["Water bottling machine take water bottle"].GetComponent<Button>().onClick.AddListener(() => TakeWaterBottleButton());
        UI.Elements["Water bottling machine take water bottle image"].GetComponent<Button>().onClick.AddListener(() => TakeWaterBottleButton());
    }
    
    public static void AddBottleButton()
    {
        ((WaterBottlingMachine)UI.ObjectOnUI).AddBottle();
    }
    
    public static void TakeWaterBottleButton()
    {
        ((WaterBottlingMachine)UI.ObjectOnUI).TakeWaterBottle();
    }
}