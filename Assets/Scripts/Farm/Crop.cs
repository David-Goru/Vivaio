using UnityEngine;

[System.Serializable]
public class Crop
{
    // Info
    [SerializeField]
    string plantName;
    [SerializeField]
    int amount;

    // State
    [SerializeField]
    int growLevel;
    [SerializeField]
    bool watered;
    [SerializeField]
    int daysUntilDry;

    // Pot
    [System.NonSerialized]
    GameObject pot;

    public Crop(string plantName, GameObject pot)
    {
        this.plantName = plantName;
        this.daysUntilDry = Farm.Plants[plantName].DaysUntilDry;
        this.pot = pot;

        // Random
        amount = Random.Range(Farm.Plants[plantName].MinAmount, Farm.Plants[plantName].MaxAmount + (pot.GetComponent<PlowedSoil>().CheckFertilizer() ? Farm.Plants[plantName].FertilizerExtra : 0));

        // Default
        growLevel = 0;
        watered = false;

        // Enable crop
        pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Load(GameObject pot)
    {
        this.pot = pot;
        pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().enabled = true;

        if (daysUntilDry <= 0) pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.Plants[plantName].Dry[growLevel - 1];
        else if (growLevel > Farm.Plants[plantName].Levels) pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.Plants[plantName].Harvested;
        else if (growLevel != 0) pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.Plants[plantName].Normal[growLevel - 1];

        if (watered) pot.transform.Find("Watered dirt").gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void NewDay()
    {
        // Check if the plant can grow
        if (growLevel < Farm.Plants[plantName].Levels)
        {
            if (!watered)
            {
                // If the plant can grow, if it wasn't watered and it wasn't a seed, it will dry a bit
                if (growLevel > 0)
                {
                    daysUntilDry--;
                    if (daysUntilDry == 0)
                    {
                        pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.Plants[plantName].Dry[growLevel - 1];
                    }
                }
            }
            else
            {
                // If the plant was watered and can grow, grow it
                watered = false;
                pot.transform.Find("Watered dirt").gameObject.GetComponent<SpriteRenderer>().enabled = false;
                growLevel++;
                pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.Plants[plantName].Normal[growLevel - 1];
            }            
            pot.GetComponent<PlowedSoil>().AutoWater();
        }
        else if (growLevel == Farm.Plants[plantName].Levels && daysUntilDry > 0)
        {
            // The plant can't grow more so dry it a bit
            daysUntilDry--;
            if (daysUntilDry == 0) pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.Plants[plantName].Dry[growLevel - 1];
        }
    }

    public bool Water()
    {
        // Check if the plant was already watered or is dried and, if not, water it
        if (watered || daysUntilDry <= 0) return true;
        else if (daysUntilDry > 0)
        {
            daysUntilDry = 2;
            watered = true;
            pot.transform.Find("Watered dirt").gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
            return false;
    }

    public bool Harvest()
    {
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        if (growLevel > Farm.Plants[plantName].Levels) // Harvested but with compost available
        {
            if ((basket.Amount + 1) <= 20 && (basket.Product == null || basket.Product.Name == "Sticks"))
            {
                basket.Product = Products.ProductsList.Find(x => x.Name == "Sticks");
                basket.Amount += 1;
                Inventory.ChangeObject();
                pot.GetComponent<PlowedSoil>().RemovePlant();
                return true;
            }
        }
        else if (daysUntilDry <= 0)
        {
            if ((basket.Amount + 2) <= 20 && (basket.Product == null || basket.Product.Name == "Sticks"))
            {
                basket.Product = Products.ProductsList.Find(x => x.Name == "Sticks");
                basket.Amount += 2;
                Inventory.ChangeObject();
                pot.GetComponent<PlowedSoil>().RemovePlant();
                return true;
            }
        }
        else if (growLevel == Farm.Plants[plantName].Levels && (basket.Amount + amount) <= 20 && (basket.Product == null || basket.Product.Name == Farm.Plants[plantName].Name))
        {
            basket.Product = Products.ProductsList.Find(x => x.Name == Farm.Plants[plantName].Name);
            basket.Amount += amount;
            Inventory.ChangeObject();
            growLevel++;
            pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.Plants[plantName].Harvested;
            return true;
        }
        return false;
    }

    public int GetGrowLevel()
    {
        return growLevel;
    }
}