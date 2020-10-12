using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool AddSeed()
    {
        if (State != MachineState.AVAILABLE) return false;
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return false;
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
            return true;
        }
        return false;
    }

    public bool TakeSeeds()
    {
        if (State != MachineState.FINISHED) return false;
        if (Seeds == 0) return false;
        if (Inventory.AddObject(new Seed(SeedType, Seeds, 10, SeedType + "Seeds")))
        {
            Seeds = 0;

            if (Compost == 0)
            {
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Available");
                State = MachineState.AVAILABLE;
                Model.transform.Find("Warning").gameObject.SetActive(false);
            }
            return true;
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

            if (Compost == 0 && Seeds == 0)
            {
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Available");
                State = MachineState.AVAILABLE;
                Model.transform.Find("Warning").gameObject.SetActive(false);
            }
            return true;
        }
        return false;
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
        if (ObjectUI.ObjectHandling == this && ObjectUI.DeseedingMachineUI.activeSelf) ObjectUI.OpenUI(this);
    }

    public override void ActionTwoHard()
    {
        if (!TakeSeeds()) TakeCompost();
        if (ObjectUI.ObjectHandling == this && ObjectUI.DeseedingMachineUI.activeSelf) ObjectUI.OpenUI(this);
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
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Finished");
                Model.transform.Find("Warning").gameObject.SetActive(true);
                break;
        }
    }
}