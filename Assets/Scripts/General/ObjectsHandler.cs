using System.Collections.Generic;
using UnityEngine;

public class ObjectsHandler : MonoBehaviour
{
    public static ObjectsData Data;

    // When loading a game
    public static bool Load(ObjectsData data)
    {
        try
        {
            Data = data;

            foreach (IObject o in Data.Objects)
            {
                if (o.Placed) // Built object
                {
                    GameObject model = Instantiate(Resources.Load<GameObject>("Objects/" + o.Name), o.WorldPosition, Quaternion.Euler(0, 0, 0));                    
                    model.GetComponent<BoxCollider2D>().enabled = true;
                    if (o.Name == "Bed")
                    {
                        TimeSystem.Bed = model;
                        TimeSystem.Sleep();
                    }
                    else if (o.Name == "Cash register")
                    {
                        CashRegister.CashRegisterModel = model;

                        CashRegister.CustomerPos = new List<Vector2>();
                        foreach (Transform t in CashRegister.CashRegisterModel.transform.Find("Customer position"))
                        {
                            CashRegister.CustomerPos.Add(t.position);
                        }
                    }

                    if (o.CanRot)
                    {
                        if (o is Gate) model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<ObjectInfo>("Objects info/" + o.Name).Sprites[o.Rotation + (((Gate)o).Opened ? 2 : 0)];
                        else model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<ObjectInfo>("Objects info/" + o.Name).Sprites[o.Rotation];

                        if (model.transform.Find("Obstacle 0") != null) model.transform.Find("Obstacle " + o.Rotation).gameObject.SetActive(true);
                    }
                    else if (model.transform.Find("Obstacle") != null) model.transform.Find("Obstacle").gameObject.SetActive(true);
                    o.Model = model;
                    if (o is Composter)
                    {
                        Composter c = (Composter)o;
                        switch (c.State)
                        {
                            case MachineState.AVAILABLE:
                                /* Fix for Alpha 5 to Alpha 6 conversion */
                                if (c.Amount == c.MaxAmount)
                                {
                                    c.State = MachineState.WORKING;
                                    c.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Working");
                                    OnTimeReached createFertilizer = c.CreateFertilizer;
                                    c.Timer = new Timer(createFertilizer, 120);
                                    TimeSystem.Data.Timers.Add(c.Timer);
                                }
                                /* End fix */
                                break;
                            case MachineState.WORKING:
                                c.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Working");
                                break;
                            case MachineState.FINISHED:
                                c.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Finished");
                                c.Model.transform.Find("Warning").gameObject.SetActive(true);
                                break;
                        }
                    }
                    else if (o is DeseedingMachine)
                    {
                        DeseedingMachine dm = (DeseedingMachine)o;
                        switch (dm.State)
                        {
                            case MachineState.WORKING:
                                dm.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Working");
                                break;
                            case MachineState.FINISHED:
                                dm.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Finished");
                                dm.Model.transform.Find("Warning").gameObject.SetActive(true);
                                break;
                        }
                    }
                    else if (o is SeedBox)
                    {
                        SeedBox sb = (SeedBox)o;
                        
                        for (int i = 0; i < sb.Seeds.Length; i++)
                        {
                            if (sb.Seeds[i] != null) 
                            {
                                sb.Model.transform.Find("Slots").Find("Slot " + i).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Farm/Seeds box/" + sb.Seeds[i].Type + " bag");
                                sb.Model.transform.Find("Slots").Find("Slot " + i).gameObject.SetActive(true);
                            }
                        }
                    }
                    else if (o is FlourMachine)
                    {
                        FlourMachine fm = (FlourMachine)o;
                        switch (fm.State)
                        {
                            case MachineState.WORKING:
                                fm.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Working");
                                break;
                            case MachineState.FINISHED:
                                fm.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Flour machine/Finished");
                                fm.Model.transform.Find("Warning").gameObject.SetActive(true);
                                break;
                        }
                    }
                    else if (o is BreadMachine)
                    {
                        BreadMachine bm = (BreadMachine)o;
                        switch (bm.State)
                        {
                            case MachineState.WORKING:
                                bm.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Working");
                                break;
                            case MachineState.FINISHED:
                                bm.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Bread machine/Finished");
                                bm.Model.transform.Find("Warning").gameObject.SetActive(true);
                                break;
                        }
                    }
                    else if (o is Furnace)
                    {
                        Furnace f = (Furnace)o;
                        switch (f.State)
                        {
                            case MachineState.WORKING:
                                f.Model.transform.Find("Sprite").gameObject.SetActive(false);
                                f.Model.transform.Find("Working").gameObject.SetActive(true);
                                f.Model.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Deseeding machine/Working");
                                break;
                            case MachineState.FINISHED:
                                f.Model.transform.Find("Warning").gameObject.SetActive(true);
                                break;
                        }
                    }
                    else if (o is ProductBox)
                    {
                        ProductBox pb = (ProductBox)o;
                        if (pb.Amount > 0)
                        {
                            pb.Item = Products.ProductsList.Find(x => x.Name == pb.ItemName);
                            pb.Model.transform.Find("Product").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Big display/" + pb.Item.Name);
                            pb.Model.transform.Find("Product").gameObject.SetActive(true);
                        }
                    }
                    else if (o is Stand)
                    {
                        Stand s = (Stand)o;
                        if (s.Amount > 0)
                        {
                            s.Item = Products.ProductsList.Find(x => x.Name == s.ItemName);
                            s.Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/" + s.DisplayType + "/" + s.Item.Name);
                            s.Model.transform.Find("Display").gameObject.SetActive(true);
                        }                        

                        s.CustomerPos = new List<Vector2>();
                        foreach (Transform t in s.Model.transform.Find("Customer position"))
                        {
                            s.CustomerPos.Add(t.position);
                        }
                    }
                    else if (o is Sign)
                    {
                        Sign s = (Sign)o;
                        s.UpdateIcon(s.Icon);
                    }
                }
                else // Item on the floor
                {                    
                    GameObject item = Instantiate(Resources.Load<GameObject>("Item"), o.WorldPosition, Quaternion.Euler(0, 0, 0));
                    Sprite sprite = Resources.Load<Sprite>("UI/" + o.Name);

                    if (o is Tool && o is WateringCan)
                    {
                        WateringCan wc = (WateringCan)o;
                        if (wc.Remaining == 0) sprite = Resources.Load<Sprite>("UI/Watering can empty");
                    }
                    else if (o is Letter)
                    {
                        Letter letter = (Letter)o;
                        sprite = Resources.Load<Sprite>("UI/" + (letter.Read ? "Open" : "Closed") + " letter");
                    }
                    else if (o.Name == "Drip bottle") sprite = Resources.Load<Sprite>("UI/Drip bottle/" + ((DripBottle)o).WaterUnits);

                    item.GetComponent<SpriteRenderer>().sprite = sprite;
                    item.GetComponent<Item>().ItemObject = o;
                }
            }
        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "ObjectsHandler", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new ObjectsData();
        Data.Objects = new List<IObject>();

        BuildableObject bed = new BuildableObject("Bed", 1, 1);
        bed.Placed = true;
        bed.Model = Instantiate(Resources.Load<GameObject>("Objects/Bed"), new Vector2(-48.25f, 7.75f), Quaternion.Euler(0, 0, 0));     
        bed.WorldPosition = bed.Model.transform.position;

        foreach (Transform t in bed.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(bed);
        TimeSystem.Bed = bed.Model;

        BuildableObject cashRegister = new BuildableObject("Cash register", 1, 1);
        cashRegister.Placed = true;
        cashRegister.Model = Instantiate(Resources.Load<GameObject>("Objects/Cash register"), new Vector2(-13.75f, 9.25f), Quaternion.Euler(0, 0, 0));     
        cashRegister.WorldPosition = cashRegister.Model.transform.position;  

        CashRegister.CustomerPos = new List<Vector2>();
        foreach (Transform t in cashRegister.Model.transform.Find("Customer position"))
        {
            CashRegister.CustomerPos.Add(t.position);
        }

        foreach (Transform t in cashRegister.Model.transform.Find("Vertices"))
        {
            Vertex v = VertexSystem.Vertices.Find(x => x.Pos == new Vector2(t.transform.position.x, t.transform.position.y));
            v.State = VertexState.Occuppied;
        }

        Data.Objects.Add(cashRegister);
        CashRegister.CashRegisterModel = cashRegister.Model;

        return true;
    }
}