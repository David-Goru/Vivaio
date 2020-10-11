using UnityEngine;

[System.Serializable]
public class ProductBox : BuildableObject
{
    [SerializeField]
    public string ItemName;
    [System.NonSerialized]
    public Product Item;
    [SerializeField]
    public int Amount;
    [SerializeField]
    public int MaxAmount;

    public ProductBox(int amount, int maxAmount) : base("Product box", 1, 1)
    {
        Amount = amount;
        MaxAmount = maxAmount;
        ItemName = "None";
    }

    public void AddProduct()
    {
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return;
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        if (basket.Amount == 0) return;
        if (Item == null)
        {
            Item = basket.Product;
            ItemName = basket.Product.Name;
            int amountPlaced;
            if (basket.Amount <= MaxAmount)
            {
                amountPlaced = basket.Amount;
                basket.Amount = 0;
                basket.Product = null;
                Inventory.ChangeObject();
            }
            else
            { 
                amountPlaced = MaxAmount;
                basket.Amount -= amountPlaced;
            }
            Amount += amountPlaced;

            Model.transform.Find("Product").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Big display/" + Item.Name);
            Model.transform.Find("Product").gameObject.SetActive(true);
        }
        else if (Item.Name == basket.Product.Name)
        {
            int amountPlaced;
            if (basket.Amount <= (MaxAmount - Amount)) amountPlaced = basket.Amount;
            else amountPlaced = MaxAmount - Amount;

            Amount += amountPlaced;
            basket.Amount -= amountPlaced;
            if (basket.Amount == 0)
            {
                basket.Product = null;
                Inventory.ChangeObject();
            }
        }
    }

    public void TakeProduct()
    {
        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return;
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        if (basket.Amount > 0 && basket.Product != Item) return;
        if (Item == null) return;

        int amount;
        int maxAmount = 20 - basket.Amount;
        if (maxAmount >= Amount)
        {
            amount = Amount;
            if (amount > 0) basket.Product = Item;
            Item = null;
            ItemName = "None";
            Model.transform.Find("Product").gameObject.SetActive(false);
        }
        else
        {
            amount = maxAmount;
            if (amount > 0) basket.Product = Item;
        }
        Amount -= amount;
        basket.Amount += amount;
        Inventory.ChangeObject();
    }

    public override void ActionOne()
    {
        AddProduct();
        if (ObjectUI.ObjectHandling == this && ObjectUI.ProductBoxUI.activeSelf) ObjectUI.OpenUI(this);
    }

    public override void ActionTwoHard()
    {
        TakeProduct();
        if (ObjectUI.ObjectHandling == this && ObjectUI.ProductBoxUI.activeSelf) ObjectUI.OpenUI(this);
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
            Model.transform.Find("Product").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Big display/" + Item.Name);
            Model.transform.Find("Product").gameObject.SetActive(true);
        }
    }
}