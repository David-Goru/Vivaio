using UnityEngine;
using CodeTools;

public class ProductBox
{
    public GameObject Model;
    public Product Item;
    int itemCount;
    int maxItems;

    public ProductBox(GameObject model, int maxItems)
    {
        Model = model;
        this.maxItems = maxItems;
        itemCount = 0;
    }

    public int AddProduct(string item, int amount)
    {
        if (Item == null)
        {
            Item = Products.ProductsList.Find(x => x.Name == item);
            int amountPlaced;
            if (amount <= maxItems)
            {
                amountPlaced = amount;
                Inventory.ChangeSubobject("None", "None");
            }
            else
            { 
                amountPlaced = maxItems;
            }
            itemCount = amountPlaced;

            Model.transform.Find("Item").gameObject.SetActive(true);
            Model.transform.Find("Item").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/" + Item.Name);
            
            return amountPlaced;
        }
        else if (Item.Name == item)
        {
            int amountPlaced;
            if (amount <= (maxItems - itemCount)) amountPlaced = amount;
            else amountPlaced = maxItems - itemCount;

            itemCount += amountPlaced;
            return amountPlaced; 
        }
        return 0;
    }

    public Tuple PickUp()
    {
        if (Item == null) return new Tuple("None", 0);
        Basket basket = GameObject.Find("Tools").transform.Find("Basket").GetComponent("Basket") as Basket;
        if (PlayerTools.ToolOnHand.Remaining > 0 && basket.Product.Name != Item.Name) return new Tuple("None", 0);

        int amount;
        int maxAmount = 20 - PlayerTools.ToolOnHand.Remaining;
        string itemName = Item.Name;
        if (maxAmount > itemCount)
        {
            amount = itemCount;
            Item = null;
            Model.transform.Find("Item").gameObject.SetActive(false);
        }
        else amount = maxAmount;
        itemCount -= amount;

        return new Tuple(itemName, amount);
    }

    public bool IsEmpty()
    {
        if (itemCount <= 0) return true;
        return false;
    }
}