using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Furnace : BuildableObject
{
    [SerializeField]
    public MachineState State;
    [SerializeField]
    public Timer Timer;
    [SerializeField]
    public int Amount;
    [SerializeField]
    public int MaxAmount;
    [SerializeField]
    public string ProductBaked;

    public Furnace() : base("Furnace", 1, 1)
    {
        State = MachineState.AVAILABLE;
        ProductBaked = "Bread";
        Amount = 0;
        MaxAmount = 10;
    }

    public void AddProduct()
    {
        if (Inventory.Data.ObjectInHand == null || Inventory.Data.ObjectInHand.Name != "Bread dough") return;

        int needs = MaxAmount - Amount;
        if (needs > Inventory.Data.ObjectInHand.Stack)
        {
            Amount += Inventory.Data.ObjectInHand.Stack;
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

            Amount = 10;
        }
    }

    public bool TakeProduct()
    {        
        if (Inventory.AddObject(new IObject("Bread", Amount, 10)))
        {
            Amount = 0;
            State = MachineState.AVAILABLE;
            Model.transform.Find("Warning").gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    public bool TurnOn()
    {
        if (Amount == 0) return false;

        State = MachineState.WORKING;
        Model.transform.Find("Sprite").gameObject.SetActive(false);
        Model.transform.Find("Working").gameObject.SetActive(true);

        OnTimeReached createProduct = CreateProduct;
        Timer = new Timer(createProduct, 120);
        TimeSystem.Data.Timers.Add(Timer);

        return true;
    }

    public void CreateProduct()
    {
        Model.transform.Find("Sprite").gameObject.SetActive(true);
        Model.transform.Find("Working").gameObject.SetActive(false);
        State = MachineState.FINISHED;
        Model.transform.Find("Warning").gameObject.SetActive(true);
        Master.Data.LastDayEnergyUsage++;
    }
}