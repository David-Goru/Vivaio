using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public static List<SimpleCrop> SimpleCrops;
    public static List<CropSprite> CropSprites;

    public static Sprite UnwateredSeedSprite;
    public static Sprite WateredSeedSprite;

    public class CropSprite
    {
        public Sprite[] wateredCrops;
        public Sprite[] unwateredCrops;
        public Sprite[] deathCrops;
        public string Type;

        public CropSprite(string type, int states)
        {
            wateredCrops = new Sprite[states];
            unwateredCrops = new Sprite[states];
            deathCrops = new Sprite[states];

            for (int i = 0; i < states; i++)
            {
                wateredCrops[i] = Resources.Load<Sprite>(type + "/Watered/" + i);
                unwateredCrops[i] = Resources.Load<Sprite>(type + "/Unwatered/" + i);
                deathCrops[i] = Resources.Load<Sprite>(type + "/Death/" + i);
            }

            Type = type;
        }
    }

    public class SimpleCrop
    {
        string type;
        int amount;
        int growLevel;
        int maxLevel;
        bool watered;
        int daysUntilDeath;

        public GameObject Pot;

        public SimpleCrop(string type, int minAmount, int maxAmount, int maxLevel, GameObject pot)
        {
            this.type = type;
            amount = Random.Range(minAmount, maxAmount);
            growLevel = -1; // -1 == only seed planted
            this.maxLevel = maxLevel;
            watered = false;
            daysUntilDeath = 2;
            Pot = pot;

            // Instantiate crop
            GameObject crop = Instantiate(Resources.Load<GameObject>("Crop"));
            crop.transform.position = pot.transform.position;
            crop.transform.SetParent(pot.transform);
            crop.name = "Crop";

            // Set sprite
            Pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = UnwateredSeedSprite;
        }

        public void NewDay()
        {
            // Grow plant
            if (growLevel < maxLevel)
            {
                if (!watered)
                {
                    daysUntilDeath--;
                    if (daysUntilDeath == 0)
                    {
                        Pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = CropSprites.Find(x => x.Type == type).deathCrops[growLevel];
                    }
                }
                else
                {
                    watered = false;
                    growLevel++;

                    // Change sprite
                    Sprite newSprite;
                    if (watered) newSprite = CropSprites.Find(x => x.Type == type).wateredCrops[growLevel];
                    else newSprite = CropSprites.Find(x => x.Type == type).unwateredCrops[growLevel];

                    Pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
                }
            }
            else if (daysUntilDeath > 0)
            {
                daysUntilDeath--;
                if (daysUntilDeath == 0) Pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = CropSprites.Find(x => x.Type == type).deathCrops[growLevel];
            }
        }

        public bool Water()
        {
            if (watered) return true;
            else if (daysUntilDeath > 0)
            {
                daysUntilDeath = 2;
                watered = true;
                if (growLevel == -1) Pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = WateredSeedSprite;
                else Pot.transform.Find("Crop").gameObject.GetComponent<SpriteRenderer>().sprite = CropSprites.Find(x => x.Type == type).wateredCrops[growLevel];
            }
            return false;
        }

        public void Harvest()
        {
            if (growLevel == maxLevel && (PlayerTools.CropsInBasket + amount) < 20 && (PlayerTools.TypeInBasket == type || PlayerTools.TypeInBasket == "None"))
            {
                PlayerTools.TypeInBasket = type;
                PlayerTools.CropsInBasket += amount;
                Inventory.ChangeSubobject(type, "Crop");
                Destroy(Pot.transform.Find("Crop").gameObject);
                SimpleCrops.Remove(this);
            }
        }

        public void Delete()
        {
            Destroy(Pot.transform.Find("Crop").gameObject);
            SimpleCrops.Remove(this);
        }
    }

    void Start()
    {
        UnwateredSeedSprite = Resources.Load<Sprite>("Unwatered seed planted");
        WateredSeedSprite = Resources.Load<Sprite>("Watered seed planted");

        CropSprites = new List<CropSprite>();
        CropSprites.Add(new CropSprite("Tomato", 6));
        CropSprites.Add(new CropSprite("Carrot", 4));
        CropSprites.Add(new CropSprite("Potato", 3));

        SimpleCrops = new List<SimpleCrop>();
    }
}