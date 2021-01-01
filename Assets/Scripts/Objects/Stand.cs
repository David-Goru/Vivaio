using UnityEngine;
using System.Collections.Generic;
using CodeTools;
using UnityEngine.UI;

[System.Serializable]
public class Stand : BuildableObject
{    
    [SerializeField]
    public string StandType;
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

    public Stand(string standType, int amount, int maxAmount, string name, string displayType, string translationKey) : base(name, 1, 1, translationKey)
    {
        StandType = standType;
        ItemName = "None";
        Amount = amount;
        MaxAmount = maxAmount;
        Available = true;
        DisplayType = displayType;
    }

    public void AddProduct()
    {
        if (Inventory.Data.ObjectInHand == null) return;

        Product p = null;
        int amount = 0;
        Basket basket = null;

        if (Products.ProductsList.Exists(x => x.Name == Inventory.Data.ObjectInHand.Name))
        {
            p = Products.ProductsList.Find(x => x.Name == Inventory.Data.ObjectInHand.Name);
            amount = Inventory.Data.ObjectInHand.Stack;
        }
        else if (Inventory.Data.ObjectInHand is Basket)
        {
            basket = (Basket)Inventory.Data.ObjectInHand;

            if (basket.Amount == 0 || basket.Product == null || basket.Product.Name == "Sticks" || basket.Product.Name == "Wheat") return;

            p = basket.Product;
            amount = basket.Amount;
        }

        if (p == null || p.Type != StandType) return;

        if (Item == null)
        {
            Item = p;
            ItemName = Item.Name;
            ItemValue = Item.MediumValue;
            int amountPlaced;
            if (amount <= MaxAmount)
            {
                amountPlaced = amount;

                if (basket == null) Inventory.RemoveObject();
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
                
                if (basket == null) Inventory.Data.ObjectInHand.Stack -= amountPlaced;
                else basket.Amount -= amountPlaced;
                Inventory.ChangeObject();

            }
            Amount = amountPlaced;

            int spriteAmount = (int)Mathf.Ceil(((float)Amount / (float)MaxAmount) * 5.0f) - 1;
            if (Item.Name == "Water bottle") spriteAmount = Amount - 1;
            Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/" + DisplayType + "/" + Item.Name + " " + spriteAmount);
            Model.transform.Find("Display").gameObject.SetActive(true);
        }
        else if (Item == p)
        {
            int amountPlaced;
            if (amount <= (MaxAmount - Amount))
            {
                amountPlaced = amount;

                if (basket == null) Inventory.RemoveObject();
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
                if (basket == null) Inventory.Data.ObjectInHand.Stack -= amountPlaced;
                else basket.Amount -= amountPlaced;
                Inventory.ChangeObject();
            }
            Amount += amountPlaced;

            int spriteAmount = (int)Mathf.Ceil(((float)Amount / (float)MaxAmount) * 5.0f) - 1;
            if (Item.Name == "Water bottle") spriteAmount = Amount - 1;
            Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/" + DisplayType + "/" + Item.Name + " " + spriteAmount);
        }

        if (UI.ObjectOnUI == this)
        {
            UI.Elements["Stand product amount"].GetComponent<Text>().text = string.Format("{0}/{1}", Amount, MaxAmount);
            UI.Elements["Stand product price placeholder"].GetComponent<Text>().text = ItemValue.ToString();
            UI.Elements["Stand recommended price"].GetComponent<Text>().text = string.Format(Localization.Translations["stand_recommended_price"], Item != null ? Item.MediumValue : 0);
            UI.Elements["Stand product"].GetComponent<Image>().sprite = UI.Sprites[ItemName];
        }
    }

    public void TakeProduct()
    {
        if (Item == null) return;
        if (Item.Type != "Vegetables")
        {
            int amountTaken = Inventory.AddObject(new IObject(Item.Name, Amount, 10, Item.TranslationKey));
            if (amountTaken > 0)
            {
                Amount -= amountTaken;
                if (Amount == 0)
                {
                    Model.transform.Find("Display").gameObject.SetActive(false);
                    Item = null;
                    ItemName = "None";
                    Amount = 0;
                }
                else
                {
                    int spriteAmount = (int)Mathf.Ceil(((float)Amount / (float)MaxAmount) * 5.0f) - 1;
                    if (Item.Name == "Water bottle") spriteAmount = Amount - 1;
                    Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/" + DisplayType + "/" + Item.Name + " " + spriteAmount);
                }
            }
            return;
        }

        if (Inventory.Data.ObjectInHand == null || !(Inventory.Data.ObjectInHand is Basket)) return;
        Basket basket = (Basket)Inventory.Data.ObjectInHand;
        
        if (basket.Amount > 0 && basket.Product != Item) return;

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

            int spriteAmount = (int)Mathf.Ceil(((float)Amount / (float)MaxAmount) * 5.0f) - 1;
            Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/" + DisplayType + "/" + Item.Name + " " + spriteAmount);
        }
        Amount -= amount;

        if (UI.ObjectOnUI == this)
        {
            UI.Elements["Stand product amount"].GetComponent<Text>().text = string.Format("{0}/{1}", Amount, MaxAmount);
            UI.Elements["Stand product"].GetComponent<Image>().sprite = UI.Sprites[ItemName];
        }
    }

    public void ChangeState()
    {
        Available = !Available;
        UI.Elements["Stand change state text"].GetComponent<Text>().text = Available ? Localization.Translations["stand_disable"] : Localization.Translations["stand_enable"];
    }

    public Tuple Take(int amount)
    {        
        if (Amount > amount)
        {
            int cost = amount * ItemValue;
            Amount -= amount;
            int spriteAmount = (int)Mathf.Ceil(((float)Amount / (float)MaxAmount) * 5.0f) - 1;
            if (Item.Name == "Water bottle") spriteAmount = Amount - 1;
            Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/" + DisplayType + "/" + Item.Name + " " + spriteAmount);
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
        UI.OpenNewObjectUI(this);
    }

    public override void LoadObjectCustom()
    {
        if (Amount > 0)
        {
            Item = Products.ProductsList.Find(x => x.Name == ItemName);

            int spriteAmount = (int)Mathf.Ceil(((float)Amount / (float)MaxAmount) * 5.0f) - 1;
            if (Item.Name == "Water bottle") spriteAmount = Amount - 1;
            Model.transform.Find("Display").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shop/Stands/" + DisplayType + "/" + Item.Name + " " + spriteAmount);
            Model.transform.Find("Display").gameObject.SetActive(true);
        }                        

        CustomerPos = new List<Vector2>();
        foreach (Transform t in Model.transform.Find("Customer position"))
        {
            CustomerPos.Add(t.position);
        }
    }

    // UI stuff
    public override void OpenUI()
    {
        UI.Elements["Stand title"].GetComponent<Text>().text = Localization.Translations[TranslationKey]; 
        UI.Elements["Stand product amount"].GetComponent<Text>().text = string.Format("{0}/{1}", Amount, MaxAmount); 
        UI.Elements["Stand product price placeholder"].GetComponent<Text>().text = ItemValue.ToString();
        UI.Elements["Stand recommended price"].GetComponent<Text>().text = string.Format(Localization.Translations["stand_recommended_price"], Item != null ? Item.MediumValue : 0);
        UI.Elements["Stand change state text"].GetComponent<Text>().text = Available ? Localization.Translations["stand_disable"] : Localization.Translations["stand_enable"];
        UI.Elements["Stand"].SetActive(true); 
    }

    public override void CloseUI()
    {
        UI.Elements["Stand"].SetActive(false);
    }

    public override void UpdateUI()
    {
        UI.Elements["Stand product amount"].GetComponent<Text>().text = string.Format("{0}/{1}", Amount, MaxAmount);
        UI.Elements["Stand product price placeholder"].GetComponent<Text>().text = ItemValue.ToString();
        UI.Elements["Stand recommended price"].GetComponent<Text>().text = string.Format(Localization.Translations["stand_recommended_price"], Item != null ? Item.MediumValue : 0);
        UI.Elements["Stand product"].GetComponent<Image>().sprite = UI.Sprites[ItemName];
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Stand take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());
        UI.Elements["Stand add product"].GetComponent<Button>().onClick.AddListener(() => AddProductButton());
        UI.Elements["Stand take product"].GetComponent<Button>().onClick.AddListener(() => TakeProductButton());
        UI.Elements["Stand change state"].GetComponent<Button>().onClick.AddListener(() => ChangeStateButton());
        UI.Elements["Stand product price"].GetComponent<InputField>().onEndEdit.AddListener(delegate{ChangePrice(UI.Elements["Stand product price"].GetComponent<InputField>());});
    }
    
    public static void AddProductButton()
    {
        ((Stand)UI.ObjectOnUI).AddProduct();
    }
    
    public static void TakeProductButton()
    {
        ((Stand)UI.ObjectOnUI).TakeProduct();
    }

    public static void ChangeStateButton()
    {
        ((Stand)UI.ObjectOnUI).ChangeState();
    }
    
    public static void ChangePrice(InputField input)
    {
        if (input.text != "")
        {
            ((Stand)UI.ObjectOnUI).ItemValue = int.Parse(input.text);
            input.text = "";
        }
    }
}