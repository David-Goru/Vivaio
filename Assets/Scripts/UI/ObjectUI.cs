using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{
    public static GameObject ComposterUI;
    public static GameObject ProductBoxUI;
    public static GameObject StandUI;
    public static GameObject StorageUI;
    public static GameObject SeedBoxUI;
    public static GameObject DeseedingMachineUI;
    public static GameObject FlourMachineUI;
    public static GameObject BreadMachineUI;
    public static GameObject FurnaceUI;
    public static GameObject SignUI;
    public static UIState State;
    public static IObject ObjectHandling;

    public float ObjectRange = 2.5f;

    void Start()
    {
        Transform ui = GameObject.Find("UI").transform;

        ComposterUI = ui.Find("Composter").gameObject;
        ProductBoxUI = ui.Find("Product box").gameObject;
        StandUI = ui.Find("Shop stand").gameObject;
        StorageUI = ui.Find("Storage").gameObject;
        SeedBoxUI = ui.Find("Seed box").gameObject;
        DeseedingMachineUI = ui.Find("Deseeding machine").gameObject;
        FlourMachineUI = ui.Find("Flour machine").gameObject;
        BreadMachineUI = ui.Find("Bread machine").gameObject;
        FurnaceUI = ui.Find("Furnace").gameObject;
        SignUI = ui.Find("Sign").gameObject;

        State = UIState.NONE;
    }

    void Update()
    {
        if (State == UIState.NONE) return;

        if (Vector2.Distance(Master.Player.transform.position, ObjectHandling.Model.transform.position) > ObjectRange)
        {            
            CloseUI();
            return;
        }

        switch (State)
        {
            case UIState.COMPOSTER:
                Composter c = (Composter)ObjectHandling;

                if (c.State == MachineState.WORKING)
                {
                    int hours = (int)c.Timer.Time / 60;
                    int minutes = c.Timer.Time - hours * 60;
                    ComposterUI.transform.Find("Working").Find("Text").GetComponent<Text>().text = string.Format(Localization.Translations["composter_working_text"], hours, minutes);
                }
                else if (c.State == MachineState.FINISHED && !ComposterUI.transform.Find("Finished").gameObject.activeSelf)
                {
                    ComposterUI.transform.Find("Working").gameObject.SetActive(false);
                    ComposterUI.transform.Find("Finished").gameObject.SetActive(true);
                }
                break;
            case UIState.STAND:
                Stand s = (Stand)ObjectHandling;

                StandUI.transform.Find("Product").Find("Text").GetComponent<Text>().text = string.Format("{0}/{1}", s.Amount, s.MaxAmount);
                StandUI.transform.Find("Price").Find("Placeholder").GetComponent<Text>().text = s.ItemValue.ToString();
                StandUI.transform.Find("Recommended price").GetComponent<Text>().text = string.Format("Recommended price: {0}", s.Item != null ? s.Item.MediumValue : 0);
                StandUI.transform.Find("Product").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + s.ItemName);
                break;
            case UIState.DESEEDINGMACHINE:
                DeseedingMachine dm = (DeseedingMachine)ObjectHandling;

                if (dm.State == MachineState.WORKING)
                {
                    int hours = (int)dm.Timer.Time / 60;
                    int minutes = dm.Timer.Time - hours * 60;
                    DeseedingMachineUI.transform.Find("Working").Find("Text").GetComponent<Text>().text = string.Format(Localization.Translations["deseeding_machine_working"], dm.SeedType, hours, minutes);
                }
                else if (dm.State == MachineState.FINISHED && !DeseedingMachineUI.transform.Find("Finished").gameObject.activeSelf)
                {
                    DeseedingMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    DeseedingMachineUI.transform.Find("Finished").transform.Find("Take seeds").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + dm.SeedType + " seeds");
                    DeseedingMachineUI.transform.Find("Finished").transform.Find("Take seeds").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + dm.Seeds;
                    DeseedingMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Sticks");
                    DeseedingMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + dm.Compost;
                    DeseedingMachineUI.transform.Find("Finished").gameObject.SetActive(true);
                }
                break;            
            case UIState.FLOURMACHINE:
                FlourMachine fm = (FlourMachine)ObjectHandling;

                if (fm.State == MachineState.WORKING)
                {
                    int minutes = fm.Timer.Time;
                    FlourMachineUI.transform.Find("Working").Find("Text").GetComponent<Text>().text = string.Format(Localization.Translations["flour_machine_working"], minutes);
                }
                else if (fm.State == MachineState.FINISHED && !FlourMachineUI.transform.Find("Finished").gameObject.activeSelf)
                {
                    FlourMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    FlourMachineUI.transform.Find("Finished").transform.Find("Take flour").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Flour");
                    FlourMachineUI.transform.Find("Finished").transform.Find("Take flour").Find("Image").Find("Amount").GetComponent<Text>().text = "x 5";
                    FlourMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Sticks");
                    FlourMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + fm.Compost;
                    FlourMachineUI.transform.Find("Finished").gameObject.SetActive(true);
                }
                break;                 
            case UIState.BREADMACHINE:
                BreadMachine bm = (BreadMachine)ObjectHandling;

                if (bm.State == MachineState.WORKING)
                {
                    int minutes = bm.Timer.Time;
                    BreadMachineUI.transform.Find("Working").Find("Text").GetComponent<Text>().text = string.Format(Localization.Translations["bread_machine_working"], minutes);
                }
                else if (bm.State == MachineState.FINISHED && !BreadMachineUI.transform.Find("Finished").gameObject.activeSelf)
                {
                    BreadMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    BreadMachineUI.transform.Find("Finished").transform.Find("Take bread dough").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Bread dough");
                    BreadMachineUI.transform.Find("Finished").transform.Find("Take bread dough").Find("Image").Find("Amount").GetComponent<Text>().text = "x 5";
                    BreadMachineUI.transform.Find("Finished").gameObject.SetActive(true);
                }
                break;                            
            case UIState.FURNACE:
                Furnace f = (Furnace)ObjectHandling;

                if (f.State == MachineState.WORKING)
                {
                    int minutes = f.Timer.Time;
                    FurnaceUI.transform.Find("Working").Find("Text").GetComponent<Text>().text = string.Format(Localization.Translations["furnace_working"], f.ProductBaked, minutes);
                }
                else if (f.State == MachineState.FINISHED && !FurnaceUI.transform.Find("Finished").gameObject.activeSelf)
                {
                    FurnaceUI.transform.Find("Working").gameObject.SetActive(false);
                    FurnaceUI.transform.Find("Finished").transform.Find("Take product").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Bread");
                    FurnaceUI.transform.Find("Finished").transform.Find("Take product").Find("Image").Find("Amount").GetComponent<Text>().text = "x 5";
                    FurnaceUI.transform.Find("Finished").gameObject.SetActive(true);
                }
                break;
        }
    }

    public static void OpenUI(IObject o)
    {        
        CloseUIs();
        if (o is Composter)
        {
            Composter c = (Composter)o;
            State = UIState.COMPOSTER;
            ComposterUI.transform.Find("Available").Find("Add compost").GetComponent<Button>().onClick.RemoveAllListeners();
            ComposterUI.transform.Find("Available").Find("Add compost").GetComponent<Button>().onClick.AddListener(() => AddCompost(c));
            ComposterUI.transform.Find("Available").Find("Take composter").GetComponent<Button>().onClick.RemoveAllListeners();
            ComposterUI.transform.Find("Available").Find("Take composter").GetComponent<Button>().onClick.AddListener(() => TakeObject(c));
            ComposterUI.transform.Find("Finished").Find("Take fertilizer").GetComponent<Button>().onClick.RemoveAllListeners();
            ComposterUI.transform.Find("Finished").Find("Take fertilizer").GetComponent<Button>().onClick.AddListener(() => TakeFertilizer(c));
            ComposterUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)c.Amount / (float)c.MaxAmount * 5));

            switch (c.State)
            {
                case MachineState.AVAILABLE:
                    ComposterUI.transform.Find("Available").gameObject.SetActive(true);
                    ComposterUI.transform.Find("Working").gameObject.SetActive(false);
                    ComposterUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.WORKING:
                    ComposterUI.transform.Find("Available").gameObject.SetActive(false);
                    ComposterUI.transform.Find("Working").gameObject.SetActive(true);
                    ComposterUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.FINISHED:
                    ComposterUI.transform.Find("Available").gameObject.SetActive(false);
                    ComposterUI.transform.Find("Working").gameObject.SetActive(false);
                    ComposterUI.transform.Find("Finished").gameObject.SetActive(true);
                    break;
            }

            ComposterUI.SetActive(true);
        }
        else if (o is ProductBox)
        {
            ProductBox pb = (ProductBox)o;
            State = UIState.PRODUCTBOX;
            ProductBoxUI.transform.Find("Add product").GetComponent<Button>().onClick.RemoveAllListeners();
            ProductBoxUI.transform.Find("Add product").GetComponent<Button>().onClick.AddListener(() => AddProduct(pb));
            ProductBoxUI.transform.Find("Take product").GetComponent<Button>().onClick.RemoveAllListeners();
            ProductBoxUI.transform.Find("Take product").GetComponent<Button>().onClick.AddListener(() => TakeProduct(pb));
            ProductBoxUI.transform.Find("Take product box").GetComponent<Button>().onClick.RemoveAllListeners();
            ProductBoxUI.transform.Find("Take product box").GetComponent<Button>().onClick.AddListener(() => TakeObject(pb));  
            ProductBoxUI.transform.Find("Product").Find("Text").GetComponent<Text>().text = string.Format("{0}/{1}", pb.Amount, pb.MaxAmount);
            ProductBoxUI.transform.Find("Product").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + pb.ItemName);

            ProductBoxUI.SetActive(true); 
        }
        else if (o is Stand)
        {
            Stand s = (Stand)o;
            State = UIState.STAND;
            StandUI.transform.Find("Add product").GetComponent<Button>().onClick.RemoveAllListeners();
            StandUI.transform.Find("Add product").GetComponent<Button>().onClick.AddListener(() => AddProduct(s));
            StandUI.transform.Find("Take product").GetComponent<Button>().onClick.RemoveAllListeners();
            StandUI.transform.Find("Take product").GetComponent<Button>().onClick.AddListener(() => TakeProduct(s));
            StandUI.transform.Find("Take stand").GetComponent<Button>().onClick.RemoveAllListeners();
            StandUI.transform.Find("Take stand").GetComponent<Button>().onClick.AddListener(() => TakeObject(s));  
            StandUI.transform.Find("Product").Find("Text").GetComponent<Text>().text = string.Format("{0}/{1}", s.Amount, s.MaxAmount); 
            StandUI.transform.Find("Price").GetComponent<InputField>().onEndEdit.RemoveAllListeners();
            StandUI.transform.Find("Price").GetComponent<InputField>().onEndEdit.AddListener(delegate{ChangePrice(s, StandUI.transform.Find("Price").GetComponent<InputField>());});
            StandUI.transform.Find("Price").Find("Placeholder").GetComponent<Text>().text = s.ItemValue.ToString();
            StandUI.transform.Find("Recommended price").GetComponent<Text>().text = string.Format(Localization.Translations["stand_recommended_price"], s.Item != null ? s.Item.MediumValue : 0);
            StandUI.transform.Find("Change state").Find("Text").GetComponent<Text>().text = s.Available ? Localization.Translations["stand_disable"] : Localization.Translations["stand_enable"];
            StandUI.transform.Find("Change state").GetComponent<Button>().onClick.RemoveAllListeners();
            StandUI.transform.Find("Change state").GetComponent<Button>().onClick.AddListener(() => ChangeState(s));  

            StandUI.SetActive(true); 
        }
        else if (o is Box)
        {
            Box b = (Box)o;
            State = UIState.STORAGE;
            bool hasItems = false;
            for (int i = 0; i < 4; i++)
            { 
                int slotNumber = i + 1;
                StorageUI.transform.Find("Slot " + slotNumber).gameObject.SetActive(true);
                if (b.Items[i] == null)
                {
                    if (b.IsDeliveryBox) StorageUI.transform.Find("Slot " + slotNumber).gameObject.SetActive(false);
                    else
                    {
                        b.SetUIButton(StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), i, false);
                        StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Add");                        
                    }
                }
                else
                {
                    hasItems = true;
                    if (b.Items[i] is Letter)
                    {
                        Letter letter = (Letter)b.Items[i];                     
                        StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
                    }
                    else if (b.Items[i] is Seed) StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + b.Items[i].Name);
                    else StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + b.Items[i].Name);
                    b.SetUIButton(StorageUI.transform.Find("Slot " + slotNumber).gameObject.GetComponent<Button>(), i, true);

                }
            }
            StorageUI.transform.Find("Take storage").GetComponent<Button>().onClick.RemoveAllListeners();
            StorageUI.transform.Find("Take storage").GetComponent<Button>().onClick.AddListener(() => TakeObject(b));  
            if (!hasItems && !b.IsDeliveryBox) StorageUI.transform.Find("Take storage").gameObject.SetActive(true);
            else StorageUI.transform.Find("Take storage").gameObject.SetActive(false);

            StorageUI.SetActive(true);
        }
        else if (o is SeedBox)
        {
            SeedBox sb = (SeedBox)o;
            State = UIState.SEEDBOX;

            bool canTakeBox = true;

            for (int i = 0; i < 8; i++)
            {
                int pos = i;
                Transform slot = SeedBoxUI.transform.Find("Slots").Find("Slot " + pos);

                slot.GetComponent<Button>().onClick.RemoveAllListeners();
                slot.GetComponent<Button>().onClick.AddListener(() => ClickSeedsSlot(sb, pos));

                if (sb.Seeds[pos] == null)
                {
                    slot.Find("Seed bag").gameObject.SetActive(false);
                    slot.Find("Amount").GetComponent<Text>().text = Localization.Translations["Empty"];
                    slot.GetComponent<Image>().enabled = true;
                }
                else
                {
                    canTakeBox = false;
                    slot.Find("Seed bag").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + sb.Seeds[pos].Name);
                    slot.Find("Seed bag").gameObject.SetActive(true);
                    slot.Find("Amount").GetComponent<Text>().text = sb.Seeds[pos].Stack + "/" + sb.Seeds[pos].MaxStack;
                    slot.GetComponent<Image>().enabled = false;
                }
            }

            SeedBoxUI.transform.Find("Take seed box").GetComponent<Button>().onClick.RemoveAllListeners();
            SeedBoxUI.transform.Find("Take seed box").GetComponent<Button>().onClick.AddListener(() => TakeObject(sb));  

            if (canTakeBox) SeedBoxUI.transform.Find("Take seed box").gameObject.SetActive(true);
            else SeedBoxUI.transform.Find("Take seed box").gameObject.SetActive(false);

            SeedBoxUI.SetActive(true); 
        }
        else if (o is DeseedingMachine)
        {
            DeseedingMachine dm = (DeseedingMachine)o;
            State = UIState.DESEEDINGMACHINE;

            DeseedingMachineUI.transform.Find("Available").transform.Find("Add seed").GetComponent<Button>().onClick.RemoveAllListeners();
            DeseedingMachineUI.transform.Find("Available").transform.Find("Add seed").GetComponent<Button>().onClick.AddListener(() => AddSeed(dm));
            DeseedingMachineUI.transform.Find("Finished").transform.Find("Take seeds").GetComponent<Button>().onClick.RemoveAllListeners();
            DeseedingMachineUI.transform.Find("Finished").transform.Find("Take seeds").GetComponent<Button>().onClick.AddListener(() => TakeSeeds(dm));
            DeseedingMachineUI.transform.Find("Finished").transform.Find("Take compost").GetComponent<Button>().onClick.RemoveAllListeners();
            DeseedingMachineUI.transform.Find("Finished").transform.Find("Take compost").GetComponent<Button>().onClick.AddListener(() => TakeCompost(dm));

            switch (dm.State)
            {
                case MachineState.AVAILABLE:
                    DeseedingMachineUI.transform.Find("Available").gameObject.SetActive(true);
                    DeseedingMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    DeseedingMachineUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.WORKING:
                    DeseedingMachineUI.transform.Find("Available").gameObject.SetActive(false);
                    DeseedingMachineUI.transform.Find("Working").gameObject.SetActive(true);
                    DeseedingMachineUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.FINISHED:
                    string seeds = dm.Seeds > 0 ? dm.SeedType + " seeds" : "None";
                    string compost = dm.Compost > 0 ? "Sticks" : "None";
                    DeseedingMachineUI.transform.Find("Finished").transform.Find("Take seeds").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + seeds);
                    DeseedingMachineUI.transform.Find("Finished").transform.Find("Take seeds").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + dm.Seeds;
                    DeseedingMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + compost);
                    DeseedingMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + dm.Compost;

                    DeseedingMachineUI.transform.Find("Available").gameObject.SetActive(false);
                    DeseedingMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    DeseedingMachineUI.transform.Find("Finished").gameObject.SetActive(true);
                    break;
            }

            DeseedingMachineUI.transform.Find("Available").Find("Take deseeding machine").GetComponent<Button>().onClick.RemoveAllListeners();
            DeseedingMachineUI.transform.Find("Available").Find("Take deseeding machine").GetComponent<Button>().onClick.AddListener(() => TakeObject(dm)); 

            DeseedingMachineUI.SetActive(true); 
        }
        else if (o is FlourMachine)
        {
            FlourMachine fm = (FlourMachine)o;
            State = UIState.FLOURMACHINE;
            
            FlourMachineUI.transform.Find("Available").Find("Add wheat").GetComponent<Button>().onClick.RemoveAllListeners();
            FlourMachineUI.transform.Find("Available").Find("Add wheat").GetComponent<Button>().onClick.AddListener(() => AddWheat(fm));
            FlourMachineUI.transform.Find("Finished").Find("Take flour").GetComponent<Button>().onClick.RemoveAllListeners();
            FlourMachineUI.transform.Find("Finished").Find("Take flour").GetComponent<Button>().onClick.AddListener(() => TakeFlour(fm));
            FlourMachineUI.transform.Find("Finished").Find("Take compost").GetComponent<Button>().onClick.RemoveAllListeners();
            FlourMachineUI.transform.Find("Finished").Find("Take compost").GetComponent<Button>().onClick.AddListener(() => TakeCompost(fm));
            FlourMachineUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)fm.Amount / (float)fm.MaxAmount * 5));

            switch (fm.State)
            {
                case MachineState.AVAILABLE:
                    FlourMachineUI.transform.Find("Available").gameObject.SetActive(true);
                    FlourMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    FlourMachineUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.WORKING:
                    FlourMachineUI.transform.Find("Available").gameObject.SetActive(false);
                    FlourMachineUI.transform.Find("Working").gameObject.SetActive(true);
                    FlourMachineUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.FINISHED:
                    string flour = fm.HasFlour ? "Flour" : "None";
                    string compost = fm.Compost > 0 ? "Sticks" : "None";
                    FlourMachineUI.transform.Find("Finished").transform.Find("Take flour").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + flour);
                    FlourMachineUI.transform.Find("Finished").transform.Find("Take flour").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + (fm.HasFlour ? "5" : "0");
                    FlourMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + compost);
                    FlourMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + fm.Compost;

                    FlourMachineUI.transform.Find("Available").gameObject.SetActive(false);
                    FlourMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    FlourMachineUI.transform.Find("Finished").gameObject.SetActive(true);
                    break;
            }

            FlourMachineUI.transform.Find("Available").Find("Take flour machine").GetComponent<Button>().onClick.RemoveAllListeners();
            FlourMachineUI.transform.Find("Available").Find("Take flour machine").GetComponent<Button>().onClick.AddListener(() => TakeObject(fm));
            
            FlourMachineUI.SetActive(true);
        }
        else if (o is BreadMachine)
        {
            BreadMachine bm = (BreadMachine)o;
            State = UIState.BREADMACHINE;
            
            BreadMachineUI.transform.Find("Available").Find("Add flour").GetComponent<Button>().onClick.RemoveAllListeners();
            BreadMachineUI.transform.Find("Available").Find("Add flour").GetComponent<Button>().onClick.AddListener(() => AddFlour(bm));
            BreadMachineUI.transform.Find("Available").Find("Add water").GetComponent<Button>().onClick.RemoveAllListeners();
            BreadMachineUI.transform.Find("Available").Find("Add water").GetComponent<Button>().onClick.AddListener(() => AddWater(bm));
            BreadMachineUI.transform.Find("Finished").Find("Take bread dough").GetComponent<Button>().onClick.RemoveAllListeners();
            BreadMachineUI.transform.Find("Finished").Find("Take bread dough").GetComponent<Button>().onClick.AddListener(() => TakeBreadDough(bm));
            BreadMachineUI.transform.Find("Available").Find("Content bar flour").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)bm.FlourAmount / (float)5 * 5));
            BreadMachineUI.transform.Find("Available").Find("Content bar water").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)bm.WaterAmount / (float)10 * 5));

            switch (bm.State)
            {
                case MachineState.AVAILABLE:
                    BreadMachineUI.transform.Find("Available").gameObject.SetActive(true);
                    BreadMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    BreadMachineUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.WORKING:
                    BreadMachineUI.transform.Find("Available").gameObject.SetActive(false);
                    BreadMachineUI.transform.Find("Working").gameObject.SetActive(true);
                    BreadMachineUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.FINISHED:
                    BreadMachineUI.transform.Find("Finished").transform.Find("Take bread dough").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Bread dough");
                    BreadMachineUI.transform.Find("Finished").transform.Find("Take bread dough").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + bm.DoughAmount;
                    
                    BreadMachineUI.transform.Find("Available").gameObject.SetActive(false);
                    BreadMachineUI.transform.Find("Working").gameObject.SetActive(false);
                    BreadMachineUI.transform.Find("Finished").gameObject.SetActive(true);
                    break;
            }

            BreadMachineUI.transform.Find("Available").Find("Take bread machine").GetComponent<Button>().onClick.RemoveAllListeners();
            BreadMachineUI.transform.Find("Available").Find("Take bread machine").GetComponent<Button>().onClick.AddListener(() => TakeObject(bm));

            BreadMachineUI.SetActive(true);
        }
        else if (o is Furnace)
        {
            Furnace f = (Furnace)o;
            State = UIState.FURNACE;

            FurnaceUI.transform.Find("Available").Find("Add product").GetComponent<Button>().onClick.RemoveAllListeners();
            FurnaceUI.transform.Find("Available").Find("Add product").GetComponent<Button>().onClick.AddListener(() => AddProduct(f));
            FurnaceUI.transform.Find("Available").Find("Turn on").GetComponent<Button>().onClick.RemoveAllListeners();
            FurnaceUI.transform.Find("Available").Find("Turn on").GetComponent<Button>().onClick.AddListener(() => TurnOn(f));
            FurnaceUI.transform.Find("Finished").Find("Take product").GetComponent<Button>().onClick.RemoveAllListeners();
            FurnaceUI.transform.Find("Finished").Find("Take product").GetComponent<Button>().onClick.AddListener(() => TakeProduct(f));
            FurnaceUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)f.Amount / (float)f.MaxAmount * 5));

            switch (f.State)
            {
                case MachineState.AVAILABLE:
                    FurnaceUI.transform.Find("Available").gameObject.SetActive(true);
                    FurnaceUI.transform.Find("Working").gameObject.SetActive(false);
                    FurnaceUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.WORKING:
                    FurnaceUI.transform.Find("Available").gameObject.SetActive(false);
                    FurnaceUI.transform.Find("Working").gameObject.SetActive(true);
                    FurnaceUI.transform.Find("Finished").gameObject.SetActive(false);
                    break;
                case MachineState.FINISHED:
                    FurnaceUI.transform.Find("Finished").transform.Find("Take product").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + f.ProductBaked);
                    FurnaceUI.transform.Find("Finished").transform.Find("Take product").Find("Image").Find("Amount").GetComponent<Text>().text = "x " + f.Amount;
                    
                    FurnaceUI.transform.Find("Available").gameObject.SetActive(false);
                    FurnaceUI.transform.Find("Working").gameObject.SetActive(false);
                    FurnaceUI.transform.Find("Finished").gameObject.SetActive(true);
                    break;
            }


            FurnaceUI.transform.Find("Available").Find("Take furnace").GetComponent<Button>().onClick.RemoveAllListeners();
            FurnaceUI.transform.Find("Available").Find("Take furnace").GetComponent<Button>().onClick.AddListener(() => TakeObject(f));
            
            FurnaceUI.SetActive(true);
        }
        else if (o is Sign)
        {
            Sign s = (Sign)o;
            State = UIState.SIGN;
            
            if (!IconsHandler.Icons.Exists(x => x.Name == s.Icon)) s.Icon = "None";

            SignUI.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = IconsHandler.Icons.Find(x => x.Name == s.Icon).Sprite;

            SignUI.transform.Find("Take sign").GetComponent<Button>().onClick.RemoveAllListeners();
            SignUI.transform.Find("Take sign").GetComponent<Button>().onClick.AddListener(() => TakeObject(s));
            SignUI.SetActive(true);
        }

        ObjectHandling = o;
    }

    public static void CloseUIs()
    {
        switch (State)
        {
            case UIState.COMPOSTER:
                ComposterUI.SetActive(false);
                break;
            case UIState.PRODUCTBOX:
                ProductBoxUI.SetActive(false);
                break;
            case UIState.STAND:
                StandUI.SetActive(false);
                break;
            case UIState.STORAGE:
                StorageUI.SetActive(false);
                break;
            case UIState.SEEDBOX:
                SeedBoxUI.SetActive(false);
                break;
            case UIState.DESEEDINGMACHINE:
                DeseedingMachineUI.SetActive(false);
                break;
            case UIState.FLOURMACHINE:
                FlourMachineUI.SetActive(false);
                break;
            case UIState.BREADMACHINE:
                BreadMachineUI.SetActive(false);
                break;
            case UIState.FURNACE:
                FurnaceUI.SetActive(false);
                break;
            case UIState.SIGN:
                SignUI.SetActive(false);
                break;
        }
        State = UIState.NONE;
    }

    // Composter
    public static void AddCompost(Composter c)
    {
        c.AddCompost();
        ComposterUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)c.Amount / (float)c.MaxAmount * 5));
        if (c.State == MachineState.WORKING)
        {
            ComposterUI.transform.Find("Available").gameObject.SetActive(false);
            ComposterUI.transform.Find("Working").gameObject.SetActive(true);
            ComposterUI.transform.Find("Finished").gameObject.SetActive(false);
        }
    }
    
    public static void TakeFertilizer(Composter c)
    {
        if (c.TakeFertilizer())
        {
            ComposterUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)c.Amount / (float)c.MaxAmount * 5));

            ComposterUI.transform.Find("Available").gameObject.SetActive(true);
            ComposterUI.transform.Find("Working").gameObject.SetActive(false);
            ComposterUI.transform.Find("Finished").gameObject.SetActive(false);
        }
    }

    // Product box
    public static void AddProduct(ProductBox pb)
    {
        pb.AddProduct();
        ProductBoxUI.transform.Find("Product").Find("Text").GetComponent<Text>().text = string.Format("{0}/{1}", pb.Amount, pb.MaxAmount);
        ProductBoxUI.transform.Find("Product").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + pb.ItemName);
    }

    public static void TakeProduct(ProductBox pb)
    {
        pb.TakeProduct();
        ProductBoxUI.transform.Find("Product").Find("Text").GetComponent<Text>().text = string.Format("{0}/{1}", pb.Amount, pb.MaxAmount);
        ProductBoxUI.transform.Find("Product").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + pb.ItemName);
    }

    // Stand
    public static void AddProduct(Stand s)
    {
        s.AddProduct();
        StandUI.transform.Find("Product").Find("Text").GetComponent<Text>().text = string.Format("{0}/{1}", s.Amount, s.MaxAmount);
        StandUI.transform.Find("Price").Find("Placeholder").GetComponent<Text>().text = s.ItemValue.ToString();
        StandUI.transform.Find("Recommended price").GetComponent<Text>().text = string.Format(Localization.Translations["stand_recommended_price"], s.Item != null ? s.Item.MediumValue : 0);
        StandUI.transform.Find("Product").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + s.ItemName);
    }

    public static void TakeProduct(Stand s)
    {
        s.TakeProduct();
        StandUI.transform.Find("Product").Find("Text").GetComponent<Text>().text = string.Format("{0}/{1}", s.Amount, s.MaxAmount);
        StandUI.transform.Find("Product").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + s.ItemName);
    }

    public static void ChangePrice(Stand s, InputField input)
    {
        if (input.text != "")
        {
            s.ItemValue = int.Parse(input.text);
            input.text = "";
        }
    }

    public static void ChangeState(Stand s)
    {
        s.Available = !s.Available;
        StandUI.transform.Find("Change state").Find("Text").GetComponent<Text>().text = s.Available ? Localization.Translations["stand_disable"] : Localization.Translations["stand_enable"];
    }

    // Seed box
    public static void ClickSeedsSlot(SeedBox sb, int pos)
    {
        sb.ClickSlot(pos);
        if (sb.Seeds[pos] == null)
        {
            SeedBoxUI.transform.Find("Slots").Find("Slot " + pos).Find("Seed bag").gameObject.SetActive(false);
            SeedBoxUI.transform.Find("Slots").Find("Slot " + pos).Find("Amount").GetComponent<Text>().text = Localization.Translations["Empty"];
            SeedBoxUI.transform.Find("Slots").Find("Slot " + pos).GetComponent<Image>().enabled = true;

            bool canTakeBox = true;
            for (int j = 0; j < 8; j++)
            {
                if (sb.Seeds[j] != null)
                {
                    canTakeBox = false;
                    break;
                }
            }
            
            if (canTakeBox) SeedBoxUI.transform.Find("Take seed box").gameObject.SetActive(true);
            else SeedBoxUI.transform.Find("Take seed box").gameObject.SetActive(false);
        }
        else
        {
            SeedBoxUI.transform.Find("Slots").Find("Slot " + pos).Find("Seed bag").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + sb.Seeds[pos].Name);
            SeedBoxUI.transform.Find("Slots").Find("Slot " + pos).Find("Seed bag").gameObject.SetActive(true);
            SeedBoxUI.transform.Find("Slots").Find("Slot " + pos).Find("Amount").GetComponent<Text>().text = sb.Seeds[pos].Stack + "/" + sb.Seeds[pos].MaxStack;
            SeedBoxUI.transform.Find("Slots").Find("Slot " + pos).GetComponent<Image>().enabled = false;
            SeedBoxUI.transform.Find("Take seed box").gameObject.SetActive(false);
        }
    }

    // Deseeding machine
    public static void AddSeed(DeseedingMachine dm)
    {
        if (dm.AddSeed())
        {
            DeseedingMachineUI.transform.Find("Available").gameObject.SetActive(false);
            DeseedingMachineUI.transform.Find("Working").gameObject.SetActive(true);
            DeseedingMachineUI.transform.Find("Finished").gameObject.SetActive(false);
        }
    }
    
    public static void TakeSeeds(DeseedingMachine dm)
    {
        if (dm.TakeSeeds())
        {
            DeseedingMachineUI.transform.Find("Finished").transform.Find("Take seeds").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/None");
            DeseedingMachineUI.transform.Find("Finished").transform.Find("Take seeds").Find("Image").Find("Amount").GetComponent<Text>().text = "x 0";

            if (dm.State == MachineState.AVAILABLE)
            {
                DeseedingMachineUI.transform.Find("Available").gameObject.SetActive(true);
                DeseedingMachineUI.transform.Find("Working").gameObject.SetActive(false);
                DeseedingMachineUI.transform.Find("Finished").gameObject.SetActive(false);
            }
        }
    }
    
    public static void TakeCompost(DeseedingMachine dm)
    {
        if (dm.TakeCompost())
        {
            DeseedingMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/None");
            DeseedingMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").Find("Amount").GetComponent<Text>().text = "x 0";

            if (dm.State == MachineState.AVAILABLE)
            {
                DeseedingMachineUI.transform.Find("Available").gameObject.SetActive(true);
                DeseedingMachineUI.transform.Find("Working").gameObject.SetActive(false);
                DeseedingMachineUI.transform.Find("Finished").gameObject.SetActive(false);
            }
        }
    }

    // Flour machine
    public static void AddWheat(FlourMachine fm)
    {
        fm.AddWheat();
        FlourMachineUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)fm.Amount / (float)fm.MaxAmount * 5));
        if (fm.State == MachineState.WORKING)
        {
            FlourMachineUI.transform.Find("Available").gameObject.SetActive(false);
            FlourMachineUI.transform.Find("Working").gameObject.SetActive(true);
            FlourMachineUI.transform.Find("Finished").gameObject.SetActive(false);
        }
    }
    
    public static void TakeFlour(FlourMachine fm)
    {
        if (fm.TakeFlour())
        {
            FlourMachineUI.transform.Find("Finished").transform.Find("Take flour").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/None");
            FlourMachineUI.transform.Find("Finished").transform.Find("Take flour").Find("Image").Find("Amount").GetComponent<Text>().text = "x 0";

            FlourMachineUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)fm.Amount / (float)fm.MaxAmount * 5));

            if (fm.State == MachineState.AVAILABLE)
            {
                FlourMachineUI.transform.Find("Available").gameObject.SetActive(true);
                FlourMachineUI.transform.Find("Working").gameObject.SetActive(false);
                FlourMachineUI.transform.Find("Finished").gameObject.SetActive(false);
            }
        }
    }
    
    public static void TakeCompost(FlourMachine fm)
    {
        if (fm.TakeCompost())
        {
            FlourMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/None");
            FlourMachineUI.transform.Find("Finished").transform.Find("Take compost").Find("Image").Find("Amount").GetComponent<Text>().text = "x 0";

            FlourMachineUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)fm.Amount / (float)fm.MaxAmount * 5));

            if (fm.State == MachineState.AVAILABLE)
            {
                FlourMachineUI.transform.Find("Available").gameObject.SetActive(true);
                FlourMachineUI.transform.Find("Working").gameObject.SetActive(false);
                FlourMachineUI.transform.Find("Finished").gameObject.SetActive(false);
            }
        }
    }

    // Bread machine
    public static void AddFlour(BreadMachine bm)
    {
        bm.AddFlour();
        BreadMachineUI.transform.Find("Available").Find("Content bar flour").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)bm.FlourAmount / (float)5 * 5));
        if (bm.State == MachineState.WORKING)
        {
            BreadMachineUI.transform.Find("Available").gameObject.SetActive(false);
            BreadMachineUI.transform.Find("Working").gameObject.SetActive(true);
            BreadMachineUI.transform.Find("Finished").gameObject.SetActive(false);
        }
    }

    public static void AddWater(BreadMachine bm)
    {
        bm.AddWater();
        BreadMachineUI.transform.Find("Available").Find("Content bar water").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)bm.WaterAmount / (float)10 * 5));
        if (bm.State == MachineState.WORKING)
        {
            BreadMachineUI.transform.Find("Available").gameObject.SetActive(false);
            BreadMachineUI.transform.Find("Working").gameObject.SetActive(true);
            BreadMachineUI.transform.Find("Finished").gameObject.SetActive(false);
        }
    }
    
    public static void TakeBreadDough(BreadMachine bm)
    {
        if (bm.TakeBreadDough())
        {
            BreadMachineUI.transform.Find("Finished").transform.Find("Take bread dough").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/None");
            BreadMachineUI.transform.Find("Finished").transform.Find("Take bread dough").Find("Image").Find("Amount").GetComponent<Text>().text = "x 0";

            BreadMachineUI.transform.Find("Available").Find("Content bar flour").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)bm.FlourAmount / (float)5 * 5));
            BreadMachineUI.transform.Find("Available").Find("Content bar water").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)bm.WaterAmount / (float)10 * 5));

            if (bm.State == MachineState.AVAILABLE)
            {
                BreadMachineUI.transform.Find("Available").gameObject.SetActive(true);
                BreadMachineUI.transform.Find("Working").gameObject.SetActive(false);
                BreadMachineUI.transform.Find("Finished").gameObject.SetActive(false);
            }
        }
    }

    // Furnace
    public static void AddProduct(Furnace f)
    {
        f.AddProduct();
        FurnaceUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)f.Amount / (float)f.MaxAmount * 5));
    }

    public static void TurnOn(Furnace f)
    {
        if (f.TurnOn())
        {        
            FurnaceUI.transform.Find("Available").gameObject.SetActive(false);
            FurnaceUI.transform.Find("Working").gameObject.SetActive(true);
            FurnaceUI.transform.Find("Finished").gameObject.SetActive(false);
        }
    }
    
    public static void TakeProduct(Furnace f)
    {
        if (f.TakeProduct())
        {
            FurnaceUI.transform.Find("Available").Find("Content bar").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Content bar/" + Mathf.Ceil((float)f.Amount / (float)f.MaxAmount * 5));
            FurnaceUI.transform.Find("Available").gameObject.SetActive(true);
            FurnaceUI.transform.Find("Working").gameObject.SetActive(false);
            FurnaceUI.transform.Find("Finished").gameObject.SetActive(false);
        }
    }

    // Sign
    public static void ChooseIcon(string icon)
    {
        if (ObjectHandling is Sign)
        {
            Sign s = (Sign)ObjectHandling;
            s.UpdateIcon(icon);
            SignUI.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = IconsHandler.Icons.Find(x => x.Name == s.Icon).Sprite;
        }
    }

    // General
    public static void TakeObject(BuildableObject b)
    {  
        if (Inventory.Data.ObjectInHand != null) return;

        switch (State)
        {
            case UIState.COMPOSTER:
                ComposterUI.SetActive(false);
                break;
            case UIState.PRODUCTBOX:
                ProductBoxUI.SetActive(false);
                break;
            case UIState.STAND:
                Master.Data.Stands.Remove((Stand)b);
                StandUI.SetActive(false);
                break;
            case UIState.STORAGE:
                StorageUI.SetActive(false);
                break;
            case UIState.SEEDBOX:
                SeedBoxUI.SetActive(false);
                break;
            case UIState.DESEEDINGMACHINE:
                DeseedingMachineUI.SetActive(false);
                break;
            case UIState.FLOURMACHINE:
                FlourMachineUI.SetActive(false);
                break;
            case UIState.BREADMACHINE:
                BreadMachineUI.SetActive(false);
                break;
            case UIState.FURNACE:
                FurnaceUI.SetActive(false);
                break;
            case UIState.SIGN:
                SignUI.SetActive(false);
                break;
        }
        State = UIState.NONE;

        Inventory.AddObject(b);
        ObjectsHandler.Data.Objects.Remove(b);
        b.Placed = false;
        foreach (Transform t in b.Model.transform.Find("Vertices"))
        {                            
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            if (v != null) v.State = VertexState.Available;
        }
        Destroy(b.Model);
    }

    public void CloseUI()
    {
        switch (State)
        {
            case UIState.COMPOSTER:
                ComposterUI.SetActive(false);
                break;
            case UIState.PRODUCTBOX:
                ProductBoxUI.SetActive(false);
                break;
            case UIState.STAND:
                StandUI.SetActive(false);
                break;
            case UIState.STORAGE:
                StorageUI.SetActive(false);
                break;
            case UIState.SEEDBOX:
                SeedBoxUI.SetActive(false);
                break;
            case UIState.DESEEDINGMACHINE:
                DeseedingMachineUI.SetActive(false);
                break;
            case UIState.FLOURMACHINE:
                FlourMachineUI.SetActive(false);
                break;
            case UIState.BREADMACHINE:
                BreadMachineUI.SetActive(false);
                break;
            case UIState.FURNACE:
                FurnaceUI.SetActive(false);
                break;
            case UIState.SIGN:
                SignUI.SetActive(false);
                break;
        }
        State = UIState.NONE;
    }
}

public enum UIState
{
    NONE,
    COMPOSTER,
    PRODUCTBOX,
    STAND,
    STORAGE,
    SEEDBOX,
    DESEEDINGMACHINE,
    FLOURMACHINE,
    BREADMACHINE,
    FURNACE,
    SIGN
}