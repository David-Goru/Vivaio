using System.Collections;
using UnityEngine;

[System.Serializable]
public class Composter : BuildableObject
{
    [SerializeField]
    public MachineState State;
    [SerializeField]
    public Timer Timer;
    [SerializeField]
    public int MaxAmount;
    [SerializeField]
    public int Amount;

    public Composter(string translationKey) : base("Composter", 1, 1, translationKey)
    {
        State = MachineState.AVAILABLE;
        MaxAmount = 10;
    }

    public void AddCompost()
    {
        if (State != MachineState.AVAILABLE) return;
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return;
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        if (basket.Product != null && basket.Product.Name == "Sticks")
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
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Working");
                OnTimeReached createFertilizer = CreateFertilizer;
                Timer = new Timer(createFertilizer, 120);
                TimeSystem.Data.Timers.Add(Timer);
            }
        }
    }

    public bool TakeFertilizer()
    {
        if (State != MachineState.FINISHED) return false;
        if (Inventory.AddObject(new Fertilizer(5, 10, "Fertilizer")))
        {
            Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Available");
            Amount = 0;
            State = MachineState.AVAILABLE;
            Model.transform.Find("Warning").gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    public void CreateFertilizer()
    {
        Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Finished");
        State = MachineState.FINISHED;
        Model.transform.Find("Warning").gameObject.SetActive(true);
    }

    public override void ActionOne()
    {
        AddCompost();
        if (ObjectUI.ObjectHandling == this && ObjectUI.ComposterUI.activeSelf) ObjectUI.OpenUI(this);
    }

    public override void ActionTwoHard()
    {
        TakeFertilizer();
        if (ObjectUI.ObjectHandling == this && ObjectUI.ComposterUI.activeSelf) ObjectUI.OpenUI(this);
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
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Finished");
                Model.transform.Find("Warning").gameObject.SetActive(true);
                break;
        }
    }
}