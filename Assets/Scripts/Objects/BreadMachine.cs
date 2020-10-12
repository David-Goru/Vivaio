using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }
        }
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
            }
        }
    }

    public bool TakeBreadDough()
    {
        if (State != MachineState.FINISHED) return false;
        if (DoughAmount > 0)
        {
            if (Inventory.AddObject(new IObject("Bread dough", 5, 10, "BreadDough")))
            {
                DoughAmount = 0;                
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Available");
                State = MachineState.AVAILABLE;
                Model.transform.Find("Warning").gameObject.SetActive(false);
                return true;
            }
        }
        return false;
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
        if (ObjectUI.ObjectHandling == this && ObjectUI.BreadMachineUI.activeSelf) ObjectUI.OpenUI(this);
    }

    public override void ActionTwoHard()
    {
        TakeBreadDough();
        if (ObjectUI.ObjectHandling == this && ObjectUI.BreadMachineUI.activeSelf) ObjectUI.OpenUI(this);
    }

    public override void ActionTwo()
    {
        ObjectUI.OpenUI(this);
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
}