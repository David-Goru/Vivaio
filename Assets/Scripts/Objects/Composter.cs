using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Composter : BuildableObject
{
    [SerializeField]
    public MachineState State;
    [SerializeField]
    public Timer Timer;
    [SerializeField]
    public int MaxCompost;
    [SerializeField]
    public int Compost;
    [SerializeField]
    public int Fertilizer;

    public Composter(string translationKey) : base("Composter", 1, 1, translationKey)
    {
        State = MachineState.AVAILABLE;
        MaxCompost = 10;
        Compost = 0;
        Fertilizer = 0;
    }

    public void AddCompost()
    {
        if (State != MachineState.AVAILABLE) return;
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return;
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        if (basket.Product != null && basket.Product.Name == "Sticks")
        {
            int needs = MaxCompost - Compost;
            if (needs > basket.Amount)
            {
                Compost += basket.Amount;
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

                Compost = MaxCompost;
                State = MachineState.WORKING;
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Working");
                OnTimeReached createFertilizer = CreateFertilizer;
                Timer = new Timer(createFertilizer, 120);
                TimeSystem.Data.Timers.Add(Timer);                
                
                UI.Elements["Composter available"].SetActive(false);
                UI.Elements["Composter working"].SetActive(true);
                UI.Elements["Composter finished"].SetActive(false);
            }
        }
        
        UI.Elements["Composter compost amount content bar"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Compost / (float)MaxCompost * 5)];
    }

    public void TakeFertilizer()
    {
        if (State != MachineState.FINISHED) return;

        int amountTaken = Inventory.AddObject(new Fertilizer(Fertilizer, 10, "Fertilizer"));
        if (amountTaken == Fertilizer)
        {
            Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Available");
            Fertilizer = 0;
            State = MachineState.AVAILABLE;
            Model.transform.Find("Warning").gameObject.SetActive(false);

            UI.Elements["Composter compost amount content bar"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Compost / (float)MaxCompost * 5)];
            UI.Elements["Composter available"].SetActive(true);
            UI.Elements["Composter working"].SetActive(false);
            UI.Elements["Composter finished"].SetActive(false);
        }
        else
        {
            Fertilizer -= amountTaken;
            UI.Elements["Composter fertilizer amount"].GetComponent<Text>().text = "x " + Fertilizer;
        }
    }

    public void CreateFertilizer()
    {
        Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Finished");
        State = MachineState.FINISHED;
        Compost = 0;
        Fertilizer = 5;
        Model.transform.Find("Warning").gameObject.SetActive(true);
    }

    public override void ActionOne()
    {
        AddCompost();
        if (UI.ObjectOnUI == this && UI.Elements["Composter"].activeSelf) UI.ObjectOnUI.OpenUI();
    }

    public override void ActionTwoHard()
    {
        TakeFertilizer();
        if (UI.ObjectOnUI == this && UI.Elements["Composter"].activeSelf) UI.ObjectOnUI.OpenUI();
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
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Working");
                break;
            case MachineState.FINISHED:
                Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Finished");
                Model.transform.Find("Warning").gameObject.SetActive(true);
                break;
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        UI.Elements["Composter compost amount content bar"].GetComponent<Image>().sprite = UI.Sprites["Content bar " + Mathf.Ceil((float)Compost / (float)MaxCompost * 5)];

        switch (State)
        {
            case MachineState.AVAILABLE:
                UI.Elements["Composter available"].SetActive(true);
                UI.Elements["Composter working"].SetActive(false);
                UI.Elements["Composter finished"].SetActive(false);
                break;
            case MachineState.WORKING:
                UI.Elements["Composter available"].SetActive(false);
                UI.Elements["Composter working"].SetActive(true);
                UI.Elements["Composter finished"].SetActive(false);
                break;
            case MachineState.FINISHED:
                UI.Elements["Composter fertilizer amount"].GetComponent<Text>().text = "x " + Fertilizer;
                UI.Elements["Composter available"].SetActive(false);
                UI.Elements["Composter working"].SetActive(false);
                UI.Elements["Composter finished"].SetActive(true);
                break;
        }

        UI.Elements["Composter"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Composter"].SetActive(false);
    }

    public override void UpdateUI()
    {
        if (State == MachineState.WORKING)
        {
            int hours = (int)Timer.Time / 60;
            int minutes = Timer.Time - hours * 60;
            UI.Elements["Composter working text"].GetComponent<Text>().text = string.Format(Localization.Translations["composter_working_text"], hours, minutes);
        }
        else if (State == MachineState.FINISHED && !UI.Elements["Composter finished"].activeSelf)
        {
            UI.Elements["Composter fertilizer amount"].GetComponent<Text>().text = "x " + Fertilizer;
            UI.Elements["Composter working"].SetActive(false);
            UI.Elements["Composter finished"].SetActive(true);
        }
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Composter take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Composter add compost button"].GetComponent<Button>().onClick.AddListener(() => AddCompostButton());
        UI.Elements["Composter take fertilizer"].GetComponent<Button>().onClick.AddListener(() => TakeFertilizerButton());
        UI.Elements["Composter fertilizer icon"].GetComponent<Button>().onClick.AddListener(() => TakeFertilizerButton());
    }
    
    public static void AddCompostButton()
    {
        ((Composter)UI.ObjectOnUI).AddCompost();
    }
    
    public static void TakeFertilizerButton()
    {
        ((Composter)UI.ObjectOnUI).TakeFertilizer();
    }
}