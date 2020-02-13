using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmFloor : MonoBehaviour
{
    bool hasPlant;
    bool hasDripBottle;
    int waterUnits;

    void Start()
    {
        hasPlant = false;
        hasDripBottle = false;
        waterUnits = 0;
    }

    public void AutoWater()
    {
        if (waterUnits > 0)
        {
            transform.Find("Drip bottle").Find(((int)Mathf.Ceil(waterUnits / 2)).ToString()).gameObject.SetActive(false);
            waterUnits--;
            Debug.Log(waterUnits + ", " + ((int)Mathf.Ceil((float)waterUnits / 2)).ToString());
            transform.Find("Drip bottle").Find(((int)Mathf.Ceil((float)waterUnits / 2)).ToString()).gameObject.SetActive(true);
            Farm.Crops.Find(x => x.GetPot() == gameObject).Water();
        }
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Inventory.InventoryText.text == "Drip bottle" && !hasDripBottle)
            {
                Inventory.ChangeObject("", "None");
                hasDripBottle = true;
                GameObject bottle = Instantiate<GameObject>(Resources.Load<GameObject>("Farm/Drip bottle"), transform.position, transform.rotation);
                bottle.transform.SetParent(transform);
                bottle.name = "Drip bottle";
            }
            else if (PlayerTools.ToolOnHand != null && !PlayerTools.DoingAnim)
            {
                switch (PlayerTools.ToolOnHand.Name)
                {
                    case "Hoe":
                        if (!hasPlant && Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f && name == "Grass")
                        {
                            StartCoroutine(PlayerTools.DoAnim("Hoe", (Vector2)transform.position));

                            name = "Plowed soil";
                            Farm.UpdateFarm(gameObject);

                            foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, 1.5f))
                            {
                                if (col.gameObject.name == "Plowed soil") Farm.UpdateFarm(col.gameObject);
                            }
                        }
                        break;
                    case "Watering can":
                        if (hasDripBottle)
                        {
                            int unitsNeeded = (10 - waterUnits) / 2;
                            int unitsAvailable = PlayerTools.ToolOnHand.Remaining;
                            int unitsToUse = (unitsAvailable > unitsNeeded) ? unitsNeeded : unitsAvailable;

                            if (unitsToUse > 0)
                            {
                                StartCoroutine(PlayerTools.DoAnim("Water", (Vector2)transform.position));
                                PlayerTools.ToolOnHand.UseTool(unitsToUse);
                                transform.Find("Drip bottle").Find(((int)Mathf.Ceil((float)waterUnits / 2)).ToString()).gameObject.SetActive(false);
                                waterUnits += unitsToUse * 2;
                                transform.Find("Drip bottle").Find(((int)Mathf.Ceil((float)waterUnits / 2)).ToString()).gameObject.SetActive(true);
                            }
                        }
                        else if (hasPlant && Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.0f)
                        {
                            if (PlayerTools.ToolOnHand.CheckTool() && !Farm.Crops.Find(x => x.GetPot() == gameObject).Water())
                            {
                                StartCoroutine(PlayerTools.DoAnim("Water", (Vector2)transform.position));
                                PlayerTools.ToolOnHand.UseTool(1);
                            }
                        }
                        break;
                    case "Seed":
                        if (!hasPlant && Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f && name == "Plowed soil")
                        {
                            StartCoroutine(PlayerTools.DoAnim("Seed", (Vector2)transform.position));

                            PlayerTools.ToolOnHand.UseTool(0);
                            Farm.Crops.Add(new Crop(Farm.Plants[(GameObject.Find("Tools").transform.Find("Seed").GetComponent("SeedTool") as SeedTool).SeedName], gameObject));
                            hasPlant = true;
                        }
                        break;
                    case "Shovel":
                        if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
                        {
                            if (hasPlant)
                            {
                                StartCoroutine(PlayerTools.DoAnim("Shovel", (Vector2)transform.position));

                                Crop plant = Farm.Crops.Find(x => x.GetPot() == gameObject);
                                plant.Delete();
                                hasPlant = false;
                            }
                            else if (name != "Grass")
                            {
                                StartCoroutine(PlayerTools.DoAnim("Shovel", (Vector2)transform.position));

                                name = "Grass";
                                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Grass");
                                foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, 1.5f))
                                {
                                    if (col.gameObject.name == "Plowed soil") Farm.UpdateFarm(col.gameObject);
                                }
                            }
                        }
                        break;
                    case "Basket":
                        if (hasPlant && Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f
                            && Farm.Crops.Find(x => x.GetPot() == gameObject).Harvest())
                        {    
                            StartCoroutine(PlayerTools.DoAnim("Basket", (Vector2)transform.position));

                            hasPlant = false;
                        }
                        break;
                }
            }
        }
    }
}