using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Furnace(string translationKey) : base("Furnace", 1, 1, translationKey)
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
        UI.Elements["Furnace product amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Amount / (float)MaxAmount * 5)];
    }

    public void TakeProduct()
    {     
        int amountTaken = Inventory.AddObject(new IObject("Bread", "", Amount, 10, "Bread"));   
        if (amountTaken == Amount)
        {
            Amount = 0;
            State = MachineState.AVAILABLE;
            Model.transform.Find("Warning").gameObject.SetActive(false);
            UI.Elements["Furnace product amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Amount / (float)MaxAmount * 5)];
            UI.Elements["Furnace available"].SetActive(true);
            UI.Elements["Furnace working"].SetActive(false);
            UI.Elements["Furnace finished"].SetActive(false);
        }
        else
        {
            Amount -= amountTaken;
            UI.Elements["Furnace take product amount"].GetComponent<Text>().text = "x " + Amount;
        }
    }

    public void TurnOn()
    {
        if (Amount == 0) return;

        State = MachineState.WORKING;
        Model.transform.Find("Sprite").gameObject.SetActive(false);
        Model.transform.Find("Working").gameObject.SetActive(true);

        OnTimeReached createProduct = CreateProduct;
        Timer = new Timer(createProduct, 120);
        TimeSystem.Data.Timers.Add(Timer);
        
        UI.Elements["Furnace available"].SetActive(false);
        UI.Elements["Furnace working"].SetActive(true);
        UI.Elements["Furnace finished"].SetActive(false);
    }

    public void CreateProduct()
    {
        Model.transform.Find("Sprite").gameObject.SetActive(true);
        Model.transform.Find("Working").gameObject.SetActive(false);
        State = MachineState.FINISHED;
        Model.transform.Find("Warning").gameObject.SetActive(true);
        Master.Data.LastDayEnergyUsage++;
    }

    public override void ActionOne()
    {
        if (State == MachineState.AVAILABLE)
        {
            AddProduct();
            if (Amount == MaxAmount) TurnOn();
            if (UI.ObjectOnUI == this && UI.Elements["Furnace"].activeSelf) OpenUI();
        }
    }

    public override void ActionTwoHard()
    {
        if (Amount > 0 && State == MachineState.FINISHED)
        {
            TakeProduct();
            if (UI.ObjectOnUI == this && UI.Elements["Furnace"].activeSelf) OpenUI();
        }
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
                Model.transform.Find("Sprite").gameObject.SetActive(false);
                Model.transform.Find("Working").gameObject.SetActive(true);
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Warning").gameObject.SetActive(true);
                break;
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        UI.Elements["Furnace product amount"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Amount / (float)MaxAmount * 5)];

        switch (State)
        {
            case MachineState.AVAILABLE:
                UI.Elements["Furnace available"].SetActive(true);
                UI.Elements["Furnace working"].SetActive(false);
                UI.Elements["Furnace finished"].SetActive(false);
                break;
            case MachineState.WORKING:
                UI.Elements["Furnace available"].SetActive(false);
                UI.Elements["Furnace working"].SetActive(true);
                UI.Elements["Furnace finished"].SetActive(false);
                break;
            case MachineState.FINISHED:
                UI.Elements["Furnace take product image"].GetComponent<Image>().sprite = UI.Sprites[ProductBaked];
                UI.Elements["Furnace take product amount"].GetComponent<Text>().text = "x " + Amount;
                
                UI.Elements["Furnace available"].SetActive(false);
                UI.Elements["Furnace working"].SetActive(false);
                UI.Elements["Furnace finished"].SetActive(true);
                break;
        }
        
        UI.Elements["Furnace"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Furnace"].SetActive(false);
    }

    public override void UpdateUI()
    {
        if (State == MachineState.WORKING)
        {
            int minutes = Timer.Time;
            UI.Elements["Furnace working text"].GetComponent<Text>().text = string.Format(Localization.Translations["furnace_working"], ProductBaked, minutes);
        }
        else if (State == MachineState.FINISHED && !UI.Elements["Furnace finished"].activeSelf)
        {
            UI.Elements["Furnace working"].SetActive(false);
            UI.Elements["Furnace take product image"].GetComponent<Image>().sprite = UI.Sprites["Bread"];
            UI.Elements["Furnace take product amount"].GetComponent<Text>().text = "x 5";
            UI.Elements["Furnace finished"].SetActive(true);
        }
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Furnace take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());  
        UI.Elements["Furnace add product"].GetComponent<Button>().onClick.AddListener(() => AddProductButton());
        UI.Elements["Furnace turn on"].GetComponent<Button>().onClick.AddListener(() => TurnOnButton());
        UI.Elements["Furnace take product"].GetComponent<Button>().onClick.AddListener(() => TakeProductButton());
        UI.Elements["Furnace take product image"].GetComponent<Button>().onClick.AddListener(() => TakeProductButton());
    }
    
    public static void AddProductButton()
    {
        ((Furnace)UI.ObjectOnUI).AddProduct();
    }
    
    public static void TurnOnButton()
    {
        ((Furnace)UI.ObjectOnUI).TurnOn();
    }
    
    public static void TakeProductButton()
    {
        ((Furnace)UI.ObjectOnUI).TakeProduct();
    }
}