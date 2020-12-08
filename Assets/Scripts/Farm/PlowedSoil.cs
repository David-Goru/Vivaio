using UnityEngine;

public class PlowedSoil : MonoBehaviour
{
    Crop crop;
    int waterUnits = 0;
    bool hasCrop = false;
    bool hasFertilizer = false;
    bool hasDripBottle = false;
    bool hasDripIrrigationKit = false;
    string sprite;

    public PlowedSoilData Save()
    {
        return new PlowedSoilData(crop, hasCrop, hasFertilizer, hasDripBottle, hasDripIrrigationKit, waterUnits, transform.position, sprite);
    }

    public void Load(PlowedSoilData f)
    {
        Transform ground = GameObject.Find("Farm").transform;
        gameObject.transform.SetParent(ground);
        SetSprite(f.Sprite);

        crop = f.Crop;
        hasCrop = f.HasCrop;
        if (hasCrop) crop.Load(gameObject);
        waterUnits = f.WaterUnits;
        if (f.HasFertilizer) AddFertilizer();
        if (f.HasDripBottle) AddDripBottle(waterUnits);
        if (f.HasDripIrrigationKit) AddDripIrrigationKit();
    }

    public void RemovePlant()
    {
        transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.SeedPlanted;
        transform.Find("Watered dirt").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        if (hasFertilizer)
        {
            hasFertilizer = false;
            transform.Find("Fertilizer").gameObject.SetActive(false);
        }
        crop = null;
        hasCrop = false;
    }

    public bool CheckFertilizer()
    {
        return hasFertilizer;
    }

    public bool AddFertilizer()
    {
        if (hasFertilizer) return false;
        hasFertilizer = true;
        transform.Find("Fertilizer").gameObject.SetActive(true);
        return true;
    }

    public bool AddDripBottle(int units)
    {
        if (hasDripBottle || hasDripIrrigationKit) return false;
        hasDripBottle = true;
        waterUnits = units;
        GameObject bottle = Instantiate<GameObject>(Resources.Load<GameObject>("Farm/Drip bottle"), transform.position, transform.rotation);
        bottle.transform.SetParent(transform);
        bottle.name = "Drip bottle";
        if (waterUnits == 0) bottle.transform.Find("Warning").gameObject.SetActive(true);
        else
        {
            bottle.transform.Find("Warning").gameObject.SetActive(false);
            AutoWater();
        }
        transform.Find("Drip bottle").Find("0").gameObject.SetActive(false);
        transform.Find("Drip bottle").Find(waterUnits.ToString()).gameObject.SetActive(true);
        return true;
    }

    public bool AddDripIrrigationKit()
    {
        if (hasDripBottle || hasDripIrrigationKit) return false;
        hasDripIrrigationKit = true;
        GameObject kit = Instantiate<GameObject>(Resources.Load<GameObject>("Farm/Drip irrigation kit"), transform.position, transform.rotation);
        kit.transform.SetParent(transform);
        kit.name = "Drip irrigation kit";
        if (GameObject.FindGameObjectWithTag("Water pump") == null) kit.transform.Find("Warning").gameObject.SetActive(true);
        else
        {
            kit.transform.Find("Warning").gameObject.SetActive(false);
            if (TimeSystem.Data.CurrentMinute >= 630 && TimeSystem.Data.CurrentMinute < 720) AutoWaterKit();
        }
        return true;
    }

    public void CheckDripIrrigationWarning(bool newState)
    {
        if (hasDripIrrigationKit)
        {
            transform.Find("Drip irrigation kit").Find("Warning").gameObject.SetActive(newState);
        }
    }

    public void AutoWater()
    {
        if (hasDripBottle && waterUnits > 0 && hasCrop && !crop.Water())
        {
            transform.Find("Drip bottle").Find(waterUnits.ToString()).gameObject.SetActive(false);
            waterUnits--;
            transform.Find("Drip bottle").Find(waterUnits.ToString()).gameObject.SetActive(true);

            if (waterUnits == 0) transform.Find("Drip bottle").Find("Warning").gameObject.SetActive(true);
        }
    }

    public void AutoWaterKit()
    {
        if (hasDripIrrigationKit && hasCrop && !crop.Water())
        {
            ((WaterPump)ObjectsHandler.Data.Objects.Find(x => x.Name == "Water pump")).GetWater(1);
            transform.Find("Drip irrigation kit").Find("Sprite").gameObject.GetComponent<Animator>().SetBool("Watering", true);
        }
    }

    public void TakeDripBottle()
    {
        if (!hasDripBottle)
        {
            TakeDripIrrigationKit();
            return;
        }

        if (Inventory.AddObject(new DripBottle(waterUnits, "DripBottle")))
        {
            waterUnits = 0;
            hasDripBottle = false;
            Destroy(transform.Find("Drip bottle").gameObject);
        }
    }

    public void TakeDripIrrigationKit()
    {
        if (!hasDripIrrigationKit) return;

        if (Inventory.AddObject(new DripIrrigationKit("DripIrrigationKit")))
        {
            hasDripIrrigationKit = false;
            Destroy(transform.Find("Drip irrigation kit").gameObject);
        }
    }

    public void NewDay()
    {
        if (hasCrop) crop.NewDay();
        if (hasDripIrrigationKit) transform.Find("Drip irrigation kit").Find("Sprite").gameObject.GetComponent<Animator>().SetBool("Watering", false);
    }

    public void SetSprite(string sprite)
    {
        this.sprite = sprite;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Farm/Plowed soil/" + sprite);
    }

    public void UseTool()
    {
        string tool = Inventory.Data.ObjectInHand.Name;
        if (Inventory.Data.ObjectInHand is Seed) tool = "Seed";
        switch(tool)
        {
            case "Drip bottle":
                if (AddDripBottle(((DripBottle)Inventory.Data.ObjectInHand).WaterUnits)) Inventory.RemoveObject();
                break;
            case "Drip irrigation kit":
                if (AddDripIrrigationKit()) Inventory.RemoveObject();
                break;  
            case "Shovel":
                if (hasCrop)
                {
                    PlayerControls.DoingAnim = true;
                    StartCoroutine(PlayerControls.DoAnim("Shovel", (Vector2)transform.position));
                    Master.RunSoundStatic(((Tool)Inventory.Data.ObjectInHand).Clips[0]);
                    RemovePlant();
                }
                break;            
            case "Watering can":
                WateringCan wc = (WateringCan)Inventory.Data.ObjectInHand;
                if (hasDripBottle)
                {
                    int unitsNeeded = (10 - waterUnits) / 2;
                    int unitsAvailable = wc.Remaining;
                    int unitsToUse = (unitsAvailable > unitsNeeded) ? unitsNeeded : unitsAvailable;

                    if (unitsToUse > 0)
                    {
                        transform.Find("Drip bottle").Find("Warning").gameObject.SetActive(false);
                        PlayerControls.DoingAnim = true;
                        StartCoroutine(PlayerControls.DoAnim("Water", (Vector2)transform.position));
                        Master.RunSoundStatic(((Tool)Inventory.Data.ObjectInHand).Clips[0]);
                        wc.UseTool(unitsToUse);
                        transform.Find("Drip bottle").Find(waterUnits.ToString()).gameObject.SetActive(false);
                        waterUnits += unitsToUse * 2;
                        transform.Find("Drip bottle").Find(waterUnits.ToString()).gameObject.SetActive(true);

                        if (hasCrop && !crop.Water())
                        {
                            transform.Find("Drip bottle").Find(waterUnits.ToString()).gameObject.SetActive(false);
                            waterUnits--;
                            transform.Find("Drip bottle").Find(waterUnits.ToString()).gameObject.SetActive(true);
                        }
                    }
                }
                else if (hasCrop)
                {
                    if (wc.CheckTool() && !crop.Water())
                    {
                        PlayerControls.DoingAnim = true;
                        StartCoroutine(PlayerControls.DoAnim("Water", (Vector2)transform.position));
                        Master.RunSoundStatic(((Tool)Inventory.Data.ObjectInHand).Clips[0]);
                        wc.UseTool(1);
                    }
                }
                break;            
            case "Basket":            
                if (hasCrop && crop.Harvest())
                {  
                    PlayerControls.DoingAnim = true;  
                    StartCoroutine(PlayerControls.DoAnim("Basket", (Vector2)transform.position));
                    Master.RunSoundStatic(((Tool)Inventory.Data.ObjectInHand).Clips[0]);
                }
                break;
            case "Seed":
                if (hasCrop) return;

                Seed seed = (Seed)Inventory.Data.ObjectInHand;
                PlayerControls.DoingAnim = true;
                GameObject.Find("Player").GetComponent<PlayerControls>().StartCoroutine(PlayerControls.DoAnim("Seed", (Vector2)transform.position));

                if (seed.Type == "Grass")
                {
                    Vector3 pos = transform.position;

                    foreach (Transform t in transform.Find("Vertices"))
                    {
                        Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
                        v.State = VertexState.Available;
                    }

                    if (hasDripBottle)
                    {
                        GameObject item = Instantiate(Resources.Load<GameObject>("Item"), pos, transform.rotation);
                        item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Drip bottle");
                        item.GetComponent<Item>().ItemObject = new DripBottle(waterUnits, "DripBottle");
                    }
                    
                    GameObject farmLand = Instantiate(Resources.Load<GameObject>("Farm/Farm land"), transform.position, Quaternion.Euler(0, 0, 0));
                    Transform ground = GameObject.Find("Farm").transform;
                    farmLand.transform.SetParent(ground);                    
                    MapSystem.Data.FarmLands.Add(farmLand.transform.position);
                    Farm.PlowedSoils.Remove(this);
                    Destroy(gameObject);

                    foreach (Collider2D col in Physics2D.OverlapCircleAll(pos, 1.5f, 1 << LayerMask.NameToLayer("Plowed soil")))
                    {
                        Farm.UpdateFarm(col.gameObject);
                    }
                }
                else
                {
                    crop = new Crop(seed.Type, gameObject);
                    hasCrop = true;
                    AutoWater();
                }
                seed.UseSeed();
                Master.RunSoundStatic(SoundsHandler.UseSeedsStatic);
                break;
            case "Fertilizer":
                if (hasFertilizer) return;
                
                Fertilizer fertilizer = (Fertilizer)Inventory.Data.ObjectInHand;
                if (!hasCrop || crop.GetGrowLevel() == 0)
                {
                    PlayerControls.DoingAnim = true;
                    StartCoroutine(PlayerControls.DoAnim("Seed", (Vector2)transform.position));
                    Master.RunSoundStatic(SoundsHandler.UseFertilizerStatic);
                    if (AddFertilizer()) fertilizer.UseFertilizer();
                }
                break;
        }        
    }
}