using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmFloor : MonoBehaviour
{
    bool hasPlant;

    void Start()
    {
        hasPlant = false;
    }

    void OnMouseDown()
    {
        if (PlayerTools.ToolOnHand != null && !PlayerTools.DoingAnim && !EventSystem.current.IsPointerOverGameObject())
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
                    if (hasPlant && Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
                    {
                        if (PlayerTools.ToolOnHand.CheckTool() && !Farm.Crops.Find(x => x.GetPot() == gameObject).Water())
                        {
                            StartCoroutine(PlayerTools.DoAnim("Water", (Vector2)transform.position));

                            PlayerTools.ToolOnHand.UseTool();
                        }
                    }
                    break;
                case "Seed":
                    if (!hasPlant && Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f && name == "Plowed soil")
                    {
                        StartCoroutine(PlayerTools.DoAnim("Seed", (Vector2)transform.position));

                        PlayerTools.ToolOnHand.UseTool();
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