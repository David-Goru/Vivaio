using UnityEngine;
using System.Collections.Generic;
using CodeTools;

[System.Serializable]
public class Stand : BuildableObject
{    
    [SerializeField]
    public List<Vector2> CustomerPos;
    [SerializeField]
    public string ItemName;
    [System.NonSerialized]
    public Product Item;
    [SerializeField]
    public int ItemValue;
    [SerializeField]
    public bool Available;
    [SerializeField]
    public string DisplayType;
    [SerializeField]
    public int Amount;
    [SerializeField]
    public int MaxAmount;

    public Stand(int amount, int maxAmount, string name, string displayType) : base(name, 1, 1)
    {
        ItemName = "None";
        Amount = amount;
        MaxAmount = maxAmount;
        Available = true;
        DisplayType = displayType;
    }

    public void AddProduct()
    {
        if (Inventory.Data.ObjectInHand == null) return;
        if (!(Inventory.Data.ObjectInHand is Basket) && Inventory.Data.ObjectInHand.Name != "Bread") return;

        Product p = null;
        int amount = 0;
        Basket basket = null;

        if (Inventory.Data.ObjectInHand.Name == "Bread")
        {
            p = Products.ProductsList.Find(x => x.Name == "Bread");
            amount = Inventory.Data.ObjectInHand.Stack;
        }
        else
        {
            basket = (Basket)Inventory.Data.ObjectInHand;

            if (basket.Amount == 0 || basket.Product.Name == "Sticks" || basket.Product.Name == "Wheat") return;

            p = basket.Product;
            amount = basket.Amount;
        }        

        if (Item == null)
        {
            Item = p;
            ItemName = Item.Name;
            ItemValue = Item.MediumValue;
            int amountPlaced;
            if (amount <= MaxAmount)
            {
                amountPlaced = amount;

                if (Inventory.Data.ObjectInHand.Name == "Bread") Inventory.RemoveObject();
                else
                {
                    basket.Amount = 0;
                    basket.Product = null;
                    Inventory.ChangeObject();
                }
            }
            else 
            {
                amountPlaced = MaxAmount;
                
                if (Inventory.Data.ObjectInHand.Name == "Bread") Inventory.Data.ObjectInHand.Stack -= amountPlaced;
                else basket.Amount -= amountPlaced;
                Inventory.ChangeObject();

            }
            Amount = amountPlaced;

            Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/" + DisplayType + "/" + Item.Name);
            Model.transform.Find("Display").gameObject.SetActive(true);
        }
        else if (Item == p)
        {
            int amountPlaced;
            if (amount <= (MaxAmount - Amount))
            {
                amountPlaced = amount;

                if (Inventory.Data.ObjectInHand.Name == "Bread") Inventory.RemoveObject();
                else
                {
                    basket.Amount = 0;
                    basket.Product = null;
                    Inventory.ChangeObject();
                }
            }
            else
            {
                amountPlaced = MaxAmount - Amount;
                if (Inventory.Data.ObjectInHand.Name == "Bread") Inventory.Data.ObjectInHand.Stack -= amountPlaced;
                else basket.Amount -= amountPlaced;
                Inventory.ChangeObject();
            }
            Amount += amountPlaced;
        }
    }

    public void TakeProduct()
    {
        if (Item.Name == "Bread")
        {
            if (Inventory.AddObject(new IObject("Bread", Amount, 10)))
            {
                Model.transform.Find("Display").gameObject.SetActive(false);
                Item = null;
                ItemName = "None";
                Amount = 0;
                return;
            }
        }
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return;
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        if (basket.Amount > 0 && basket.Product != Item) return;
        if (Item == null) return;

        int amount;
        int maxAmount = 20 - basket.Amount;
        if (maxAmount >= Amount)
        {
            amount = Amount;
            Model.transform.Find("Display").gameObject.SetActive(false);
            basket.Amount += amount;
            basket.Product = Item;
            Item = null;
            ItemName = "None";
            Inventory.ChangeObject();
        }
        else
        {
            amount = maxAmount;
            basket.Amount += amount;
            basket.Product = Item;
            Inventory.ChangeObject();
        }
        Amount -= amount;
    }

    public Tuple Take(int amount)
    {        
        if (Amount > amount)
        {
            int cost = amount * ItemValue;
            Amount -= amount;
            return new Tuple(amount.ToString(), cost);
        }
        else if (Amount > 0)
        {
            int cost = Amount * ItemValue;
            Tuple info = new Tuple(Amount.ToString(), cost);
            Amount = 0;
            Item = null;            
            ItemName = "None";
            Model.transform.Find("Display").gameObject.SetActive(false);
            return info;
        }
        return new Tuple("0", 0);
    }

    public override void ActionOne()
    {
        AddProduct();
    }

    public override void ActionTwoHard()
    {
        TakeProduct();
    }

    public override void ActionTwo()
    {
        ObjectUI.OpenUI(this);
    }

    public override void LoadObjectCustom()
    {
        if (Amount > 0)
        {
            Item = Products.ProductsList.Find(x => x.Name == ItemName);
            Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/" + DisplayType + "/" + Item.Name);
            Model.transform.Find("Display").gameObject.SetActive(true);
        }                        

        CustomerPos = new List<Vector2>();
        foreach (Transform t in Model.transform.Find("Customer position"))
        {
            CustomerPos.Add(t.position);
        }
    }
}