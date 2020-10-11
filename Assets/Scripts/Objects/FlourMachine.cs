using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool HasFlour;
    [SerializeField]
    public int Compost;

    public FlourMachine() : base("Flour machine", 1, 1)
    {
        State = MachineState.AVAILABLE;
        MaxAmount = 10;
        HasFlour = false;
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
            }
        }
    }

    public bool TakeFlour()
    {
        if (State != MachineState.FINISHED) return false;
        if (HasFlour)
        {
            if (Inventory.AddObject(new IObject("Flour", 5, 10)))
            {
                HasFlour = false;
                if (Compost == 0)
                {
                    Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Available");
                    Amount = 0;
                    State = MachineState.AVAILABLE;
                    Model.transform.Find("Warning").gameObject.SetActive(false);
                }
                return true;
            }
        }
        return false;
    }

    public bool TakeCompost()
    {
        if (State != MachineState.FINISHED) return false;
        if (Compost == 0) return false;
        if (Inventory.Data.ObjectInHand != null && Inventory.Data.ObjectInHand is Basket)
        {
            Basket basket = (Basket)Inventory.Data.ObjectInHand;
            if (basket.Amount > 0 && basket.Product.Name != "Sticks") return false;
            
            int amount = (10 - basket.Amount > Compost ? Compost : 10 - basket.Amount);

            if (amount == 0) return false;

            basket.Product = Products.ProductsList.Find(x => x.Name == "Sticks");
            Compost -= amount;
            basket.Amount += amount;
            Inventory.ChangeObject();

            if (Compost == 0 && !HasFlour)
            {
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Available");
                Amount = 0;
                State = MachineState.AVAILABLE;
                Model.transform.Find("Warning").gameObject.SetActive(false);
            }
            return true;
        }
        return false;
    }

    public void CreateFlour()
    {
        Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Finished");
        State = MachineState.FINISHED;
        Model.transform.Find("Warning").gameObject.SetActive(true);  
        HasFlour = true; 
        Compost = Random.Range(1, 4);     
        Master.Data.LastDayEnergyUsage++;
    }

    public override void ActionOne()
    {
        AddWheat();
        if (ObjectUI.ObjectHandling == this && ObjectUI.FlourMachineUI.activeSelf) ObjectUI.OpenUI(this);
    }

    public override void ActionTwoHard()
    {
        if (!TakeFlour()) TakeCompost();
        if (ObjectUI.ObjectHandling == this && ObjectUI.FlourMachineUI.activeSelf) ObjectUI.OpenUI(this);
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
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Finished");
                Model.transform.Find("Warning").gameObject.SetActive(true);
                break;
        }
    }
}