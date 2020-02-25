using UnityEngine;

public class ToolPhysical : MonoBehaviour
{
    public string Type;
    public Tool Tool;

    void Start()
    {
        switch (Type)
        {
            case "Hoe":
                ToolsHolder.Hoe = new Hoe();
                ToolsHolder.Hoe.Name = "Hoe";
                ToolsHolder.Hoe.OnHand = false;
                ToolsHolder.Hoe.Model = gameObject;
                Tool = ToolsHolder.Hoe;
                break;
            case "Shovel":
                ToolsHolder.Shovel = new Shovel();
                ToolsHolder.Shovel.Name = "Shovel";
                ToolsHolder.Shovel.OnHand = false;
                ToolsHolder.Shovel.Model = gameObject;
                Tool = ToolsHolder.Shovel;
                break;
            case "Watering can":
                ToolsHolder.WateringCan = new WateringCan();
                ToolsHolder.WateringCan.Name = "Watering can";
                ToolsHolder.WateringCan.Remaining = 0;
                ToolsHolder.WateringCan.OnHand = false;
                ToolsHolder.WateringCan.Model = gameObject;
                Tool = ToolsHolder.WateringCan;
                break;
            case "Basket":
                ToolsHolder.Basket = new Basket();
                ToolsHolder.Basket.Name = "Basket";
                ToolsHolder.Basket.Amount = 0;
                ToolsHolder.Basket.Product = null;
                ToolsHolder.Basket.OnHand = false;
                ToolsHolder.Basket.Model = gameObject;
                Tool = ToolsHolder.Basket;
                break;
        }
    }    

    void OnMouseDown()
    {
        if (Inventory.ObjectInHand == null && Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= 1) Tool.TakeTool();
    }

    void OnMouseOver()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 1)
            transform.Find("Text").gameObject.SetActive(false);
        else if (Inventory.ObjectInHand == null) transform.Find("Text").gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        if (transform.Find("Text").gameObject.activeSelf) transform.Find("Text").gameObject.SetActive(false);
    }
}