using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop
{
    // Info
    Plant plant;
    GameObject pot;
    int amount;

    // State
    int growLevel;
    bool watered;
    int daysUntilDry;

    public Crop(Plant plant, GameObject pot)
    {
        this.plant = plant;
        this.daysUntilDry = plant.DaysUntilDry;
        this.pot = pot;

        // Random
        amount = Random.Range(plant.MinAmount, plant.MaxAmount + (pot.GetComponent<FarmFloor>().CheckFertilizer() ? plant.FertilizerExtra : 0));

        // Default
        growLevel = 0;
        watered = false;

        // Instantiate crop
        GameObject crop = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Crops/Crop"));
        crop.transform.position = pot.transform.position;
        crop.transform.SetParent(pot.transform);
        crop.name = "Crop";

        // Set sprite        
        pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.UnwateredSeed;
    }

    public void NewDay()
    {
        // Check if the plant can grow
        if (growLevel < plant.Levels)
        {
            if (!watered)
            {
                // If the plant can grow, if it wasn't watered and it wasn't a seed, it will dry a bit
                if (growLevel > 0)
                {
                    daysUntilDry--;
                    if (daysUntilDry == 0)
                    {
                        pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = plant.Dry[growLevel - 1];
                    }
                }
            }
            else
            {
                // If the plant was watered and can grow, grow it
                watered = false;
                growLevel++;
                pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = plant.Unwatered[growLevel - 1];
            }            
            pot.GetComponent<FarmFloor>().AutoWater();
        }
        else if (daysUntilDry > 0)
        {
            // The plant can't grow more so dry it a bit
            daysUntilDry--;
            if (daysUntilDry == 0) pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = plant.Dry[growLevel - 1];
        }
    }

    public bool Water()
    {
        // Check if the plant was already watered and, if not, water it
        if (watered) return true;
        else if (daysUntilDry > 0)
        {
            daysUntilDry = 2;
            watered = true;
            if (growLevel == 0) pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = Farm.WateredSeed;
            else pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = plant.Watered[growLevel - 1];
        }
        return false;
    }

    public bool Harvest()
    {
        Basket basket = (Basket)Inventory.ObjectInHand;
        if (daysUntilDry == 0)
        {
            if ((basket.Amount + 2) <= 20 && (basket.Product == null || basket.Product.Name == "Sticks"))
            {
                basket.Product = Products.ProductsList.Find(x => x.Name == "Sticks");
                basket.Amount += 2;
                Inventory.ChangeObject();
                Delete();
                return true;
            }
        }
        if (growLevel == plant.Levels && (basket.Amount + amount) <= 20 && (basket.Product == null || basket.Product.Name == plant.Name))
        {
            basket.Product = Products.ProductsList.Find(x => x.Name == plant.Name);
            basket.Amount += amount;
            Inventory.ChangeObject();
            daysUntilDry = 0;
            pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = plant.Dry[growLevel - 1];
            return true;
        }
        return false;
    }

    public GameObject GetPot()
    {
        return pot;
    }

    public void Delete()
    {
        pot.GetComponent<FarmFloor>().RemoveFertilizer();
        pot.GetComponent<FarmFloor>().RemovePlant();
        MonoBehaviour.Destroy(pot.transform.Find("Crop").gameObject);
        Farm.Crops.Remove(this);
    }
}