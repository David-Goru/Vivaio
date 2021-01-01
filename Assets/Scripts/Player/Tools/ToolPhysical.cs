using UnityEngine;
using UnityEngine.EventSystems;

public class ToolPhysical : MonoBehaviour
{
    public string Type;
    public Tool Tool;
    public AudioClip[] Clips;

    // When loading a game
    public void Load()
    {
        switch (Type)
        {
            case "Hoe":
                Tool = ToolsHolder.Data.Hoe;
                Tool.Clips = Clips;
                break;            
            case "Shovel":
                Tool = ToolsHolder.Data.Shovel;
                Tool.Clips = Clips;
                break;            
            case "Watering can":
                Tool = ToolsHolder.Data.WateringCan;
                Tool.Clips = Clips;
                break;            
            case "Basket":
                Tool = ToolsHolder.Data.Basket;
                Tool.Clips = Clips;
                ToolsHolder.Data.Basket.LoadObjectCustom();                
                break;
        }
        Tool.Model = gameObject;
        if (!Tool.OnStand) Tool.Model.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    // When creating a new game
    public void New()
    {
        switch (Type)
        {
            case "Hoe":
                ToolsHolder.Data.Hoe = new Hoe();
                ToolsHolder.Data.Hoe.Name = "Hoe";
                ToolsHolder.Data.Hoe.OnStand = true;
                ToolsHolder.Data.Hoe.Model = gameObject;
                Tool = ToolsHolder.Data.Hoe;
                Tool.Clips = Clips;
                break;
            case "Shovel":
                ToolsHolder.Data.Shovel = new Shovel();
                ToolsHolder.Data.Shovel.Name = "Shovel";
                ToolsHolder.Data.Shovel.OnStand = true;
                ToolsHolder.Data.Shovel.Model = gameObject;
                Tool = ToolsHolder.Data.Shovel;
                Tool.Clips = Clips;
                break;
            case "Watering can":
                ToolsHolder.Data.WateringCan = new WateringCan();
                ToolsHolder.Data.WateringCan.Name = "Watering can";
                ToolsHolder.Data.WateringCan.Remaining = 0;
                ToolsHolder.Data.WateringCan.OnStand = true;
                ToolsHolder.Data.WateringCan.Model = gameObject;
                Tool = ToolsHolder.Data.WateringCan;
                Tool.Clips = Clips;
                break;
            case "Basket":
                ToolsHolder.Data.Basket = new Basket();
                ToolsHolder.Data.Basket.Name = "Basket";
                ToolsHolder.Data.Basket.Amount = 0;
                ToolsHolder.Data.Basket.Product = null;
                ToolsHolder.Data.Basket.OnStand = true;
                ToolsHolder.Data.Basket.Model = gameObject;
                Tool = ToolsHolder.Data.Basket;
                Tool.Clips = Clips;
                break;
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }    

    void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) return;
        if (!EventSystem.current.IsPointerOverGameObject() && Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) <= 1.5f)
        {
            if (Inventory.Data.ObjectInHand == null)
            {
                Tool.TakeTool();
                transform.Find("Take").gameObject.SetActive(false);
            }
            else if (Inventory.Data.ObjectInHand is Tool)
            {
                transform.parent.Find(Inventory.Data.ObjectInHand.Name).Find("Leave").gameObject.SetActive(false);
                ((Tool)Inventory.Data.ObjectInHand).LetTool();
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) return;
        if (!EventSystem.current.IsPointerOverGameObject()) 
        {
            if (Inventory.Data.ObjectInHand == null) 
            {
                if (Tool.OnStand) transform.Find("Take").gameObject.SetActive(true);
            }
            else if (Inventory.Data.ObjectInHand is Tool)
            {
                transform.parent.Find(Inventory.Data.ObjectInHand.Name).Find("Leave").gameObject.SetActive(true);
            }
        }
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 1.5f)
        {
            transform.Find("Take").gameObject.SetActive(false);
            if (Inventory.Data.ObjectInHand != null && Inventory.Data.ObjectInHand is Tool)
                transform.parent.Find(Inventory.Data.ObjectInHand.Name).Find("Leave").gameObject.SetActive(false);
        }
    }

    void OnMouseExit()
    {
        if (transform.Find("Take").gameObject.activeSelf) transform.Find("Take").gameObject.SetActive(false);
        if (transform.parent.Find("Hoe").Find("Leave").gameObject.activeSelf) transform.parent.Find("Hoe").Find("Leave").gameObject.SetActive(false);
        if (transform.parent.Find("Shovel").Find("Leave").gameObject.activeSelf) transform.parent.Find("Shovel").Find("Leave").gameObject.SetActive(false);
        if (transform.parent.Find("Watering can").Find("Leave").gameObject.activeSelf) transform.parent.Find("Watering can").Find("Leave").gameObject.SetActive(false);
        if (transform.parent.Find("Basket").Find("Leave").gameObject.activeSelf) transform.parent.Find("Basket").Find("Leave").gameObject.SetActive(false);
    }
}