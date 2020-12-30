using UnityEngine;
using UnityEngine.UI;

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

    public ProductBox(int amount, int maxAmount, string translationKey) : base("Product box", 1, 1, translationKey)
    {
        Amount = amount;
        MaxAmount = maxAmount;
        ItemName = "None";
    }

    public void AddProduct()
    {
        if (Inventory.Data.ObjectInHand == null) return;

        if (Inventory.Data.ObjectInHand is Basket)
        {
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
                }
                else
                { 
                    amountPlaced = MaxAmount;
                    basket.Amount -= amountPlaced;
                }
                Amount += amountPlaced;

                Inventory.ChangeObject();
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
                if (basket.Amount == 0) basket.Product = null;
                Inventory.ChangeObject();
            }
        }
        else if (Products.ProductsList.Exists(x => x.Name == Inventory.Data.ObjectInHand.Name))
        {        
            if (Item == null)
            {
                Item = Products.ProductsList.Find(x => x.Name == Inventory.Data.ObjectInHand.Name);
                ItemName = Inventory.Data.ObjectInHand.Name;
                int amountPlaced;
                if (Inventory.Data.ObjectInHand.Stack <= MaxAmount) amountPlaced = Inventory.Data.ObjectInHand.Stack;
                else amountPlaced = MaxAmount;                
                Amount += amountPlaced;

                Inventory.RemoveObject(amountPlaced);
                Model.transform.Find("Product").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Big display/" + Item.Name);
                Model.transform.Find("Product").gameObject.SetActive(true);
            }
            else if (Item.Name == Inventory.Data.ObjectInHand.Name)
            {
                int amountPlaced;
                if (Inventory.Data.ObjectInHand.Stack <= (MaxAmount - Amount)) amountPlaced = Inventory.Data.ObjectInHand.Stack;
                else amountPlaced = MaxAmount - Amount;

                Amount += amountPlaced;
                Inventory.RemoveObject(amountPlaced);
            }
        }
        else return;

        UI.Elements["Product box product amount"].GetComponent<Text>().text = string.Format("{0}/{1}", Amount, MaxAmount);
        UI.Elements["Product box product"].GetComponent<Image>().sprite = UI.Sprites[ItemName];
    }

    public void TakeProduct()
    {
        if (Item == null) return;

        if (Item.Type != "Vegetables")
        {
            int amountAdded = Inventory.AddObject(new IObject(Item.Name, Amount > 10 ? 10 : Amount, 10, Item.TranslationKey));            

            Amount -= amountAdded;
            if (Amount == 0)
            {
                Item = null;
                ItemName = "None";
                Model.transform.Find("Product").gameObject.SetActive(false);
            }
        }
        else if (Inventory.Data.ObjectInHand != null && Inventory.Data.ObjectInHand is Basket)
        {
            Basket basket = (Basket)Inventory.Data.ObjectInHand;
            if (basket.Amount > 0 && basket.Product != Item) return;

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

        UI.Elements["Product box product amount"].GetComponent<Text>().text = string.Format("{0}/{1}", Amount, MaxAmount);
        UI.Elements["Product box product"].GetComponent<Image>().sprite = UI.Sprites[ItemName];
    }

    public override void ActionOne()
    {
        AddProduct();
        if (UI.ObjectOnUI == this && UI.Elements["Product box"].activeSelf) OpenUI();
    }

    public override void ActionTwoHard()
    {
        TakeProduct();
        if (UI.ObjectOnUI == this && UI.Elements["Product box"].activeSelf) OpenUI();
    }

    public override void ActionTwo()
    {
        UI.OpenNewObjectUI(this);
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

    // UI stuff
    public override void OpenUI()
    {
        UI.Elements["Product box product amount"].GetComponent<Text>().text = string.Format("{0}/{1}", Amount, MaxAmount);
        UI.Elements["Product box product"].GetComponent<Image>().sprite = UI.Sprites[ItemName];
        UI.Elements["Product box"].SetActive(true); 
    }

    public override void CloseUI()
    {
        UI.Elements["Product box"].SetActive(false);
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Product box take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Product box add product"].GetComponent<Button>().onClick.AddListener(() => AddProductButton());
        UI.Elements["Product box take product"].GetComponent<Button>().onClick.AddListener(() => TakeProductButton());
    }
    
    public static void AddProductButton()
    {
        ((ProductBox)UI.ObjectOnUI).AddProduct();
    }
    
    public static void TakeProductButton()
    {
        ((ProductBox)UI.ObjectOnUI).TakeProduct();
    }
}