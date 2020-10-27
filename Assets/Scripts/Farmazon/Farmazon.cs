using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Farmazon : MonoBehaviour
{
    public GameObject ItemUI;
    public static Transform FarmazonListHandler;
    public static Dictionary<string, FarmazonItem> FarmazonItems;
    public static List<CartItem> CartItems;
    FarmazonItem selectedItem;
    int amount;
    
    public static int TotalPrice;
    public GameObject CartCircle;

    // When loading a game
    public static bool Load(FarmazonData data)
    {
        try
        {
            FarmazonListHandler = GameObject.Find("UI").transform.Find("Farmazon").Find("Items").Find("Viewport").Find("Content");
        
            FarmazonItems = new Dictionary<string, FarmazonItem>();
            LoadFarmazonItems();

            CartItems = data.CartItems;
            if (CartItems.Count > 0) GameObject.Find("UI").transform.Find("Farmazon button").Find("Circle").gameObject.SetActive(true);
        }
        catch (Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "Farmazon", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {        
        FarmazonListHandler = GameObject.Find("UI").transform.Find("Farmazon").Find("Items").Find("Viewport").Find("Content");
        
        FarmazonItems = new Dictionary<string, FarmazonItem>();
        CartItems = new List<CartItem>();

        LoadFarmazonItems();

        return true;
    }

    // When saving the game
    public static FarmazonData Save()
    {
        FarmazonData data = new FarmazonData();
        data.CartItems = CartItems;
        data.TotalPrice = TotalPrice;
        return data;
    }

    public void CheckState()
    {
        if (CartItems.Count > 0) CartCircle.SetActive(true);
        else CartCircle.SetActive(false);
    }

    public static void LoadFarmazonItems()
    {
        XmlDocument itemsDoc = new XmlDocument();
        itemsDoc.Load(Application.dataPath + "/Data/Farmazon items.xml");
        XmlNodeList items = itemsDoc.GetElementsByTagName("FarmazonItem");

        foreach (XmlNode item in items)
        {
            FarmazonItems.Add(item["Name"].InnerText, new FarmazonItem(item["Name"].InnerText, item["Use"].InnerText, int.Parse(item["Price"].InnerText), item["Description"].InnerText, item["Category"].InnerText, item["TranslationKey"].InnerText));
        }
    }

    public void SelectItem(string itemName)
    {
        selectedItem = FarmazonItems[itemName];

        amount = 0;
        ItemUI.transform.Find("Item").Find("Amount info").Find("Amount").GetComponent<Text>().text = "0";
        ItemUI.transform.Find("Item").Find("Amount info").Find("Price").GetComponent<Text>().text = "0€";

        ItemUI.transform.Find("Item").gameObject.SetActive(true);
        if (selectedItem.Use == "Seed")
        {
            ItemUI.transform.Find("Item").Find("Name").GetComponent<Text>().text = Localization.Translations[FarmazonItems[itemName].TranslationKey]; 
            ItemUI.transform.Find("Item").Find("Image").GetComponent<Image>().sprite = Resources.Load<ObjectInfo>("Objects info/" + itemName + " seeds").Icon;
        }
        else 
        {
            ItemUI.transform.Find("Item").Find("Name").GetComponent<Text>().text = Localization.Translations[FarmazonItems[itemName].TranslationKey];
            ItemUI.transform.Find("Item").Find("Image").GetComponent<Image>().sprite = Resources.Load<ObjectInfo>("Objects info/" + itemName).Icon;
        }
        ItemUI.transform.Find("Item").Find("Description").GetComponent<Text>().text = selectedItem.Description;
        ItemUI.transform.Find("No selected item").gameObject.SetActive(false);
    }

    public void AddAmount()
    {
        if (amount >= 99) return;
        amount++;
        ItemUI.transform.Find("Item").Find("Amount info").Find("Amount").GetComponent<Text>().text = amount.ToString();
        ItemUI.transform.Find("Item").Find("Amount info").Find("Price").GetComponent<Text>().text = (amount * selectedItem.Price) + "€";
    }
    public void RemoveAmount()
    {
        if (amount <= 0) return;
        amount--;
        ItemUI.transform.Find("Item").Find("Amount info").Find("Amount").GetComponent<Text>().text = amount.ToString();
        ItemUI.transform.Find("Item").Find("Amount info").Find("Price").GetComponent<Text>().text = (amount * selectedItem.Price) + "€";
    }

    public void AddToCart()
    {
        if (amount == 0) return;
        CartItems.Add(new CartItem(selectedItem.Name, amount));
        TotalPrice += amount * selectedItem.Price;
        amount = 0;
        ItemUI.transform.Find("Item").gameObject.SetActive(false);
        ItemUI.transform.Find("No selected item").gameObject.SetActive(true);
        CheckState();
    }

    public void OpenCart()
    {
        transform.Find("Chart info").Find("Price").GetComponent<Text>().text = TotalPrice + "€";

        Transform content = transform.Find("Chart").Find("Viewport").Find("Content");

        // Clean all UI buttons from the cart
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }

        // Add UI buttons to the cart
        foreach (CartItem item in CartItems)
        {
            GameObject itemObject = Instantiate(Resources.Load<GameObject>("UI/Cart item"), transform.position, transform.rotation);
            item.UIModel = itemObject;
            itemObject.transform.Find("Remove").GetComponent<Button>().onClick.AddListener(delegate () { RemoveItem(item); });
            FarmazonItem fi = FarmazonItems[item.Item];
            if (fi.Use == "Seed")
            {
                itemObject.transform.Find("Name").GetComponent<Text>().text = Localization.Translations[fi.TranslationKey];
                itemObject.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<ObjectInfo>("Objects info/" + fi.Name + " seeds").Icon;
            }
            else
            {
                itemObject.transform.Find("Name").GetComponent<Text>().text = Localization.Translations[fi.TranslationKey];
                itemObject.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<ObjectInfo>("Objects info/" + fi.Name).Icon;
            }
            itemObject.transform.Find("Price").GetComponent<Text>().text = item.Amount + "u (" + item.Amount * fi.Price + "€)";
            itemObject.transform.SetParent(content, false);
        }
    }

    public void RemoveItem(CartItem item)
    {
        TotalPrice -= item.Amount * FarmazonItems[item.Item].Price;
        transform.Find("Chart info").Find("Price").GetComponent<Text>().text = TotalPrice + "€";
        item.RemoveIt();
        CheckState();
    }

    public void Buy()
    {
        if (Master.Data.Balance < TotalPrice || CartItems.Count == 0) return;

        int slotCounter = 0;
        Box box = new Box("Delivery box", "DeliveryBox");
        DeliverySystem.Data.DeliveryList.Add(box);

        foreach (CartItem item in CartItems.ToArray())
        {
            for (int am = 0; am < item.Amount; am++)
            {
                FarmazonItem fi = FarmazonItems[item.Item];
                switch (fi.Use)
                {
                    case "Build":
                        if (fi.Name == "Composter") box.Items[slotCounter] = new Composter(fi.TranslationKey);
                        else if (fi.Name == "Product box") box.Items[slotCounter] = new ProductBox(0, 100, fi.TranslationKey);
                        else if (fi.Name == "Shop table") box.Items[slotCounter] = new Stand(0, 50, "Shop table", "Big display", fi.TranslationKey);
                        else if (fi.Name == "Shop box") box.Items[slotCounter] = new Stand(0, 10, "Shop box", "Small display", fi.TranslationKey);
                        else if (fi.Name == "Storage box") box.Items[slotCounter] = new Box("Storage box", fi.TranslationKey);
                        else if (fi.Name == "Seed box") box.Items[slotCounter] = new SeedBox(fi.TranslationKey);
                        else if (fi.Name == "Deseeding machine") box.Items[slotCounter] = new DeseedingMachine(fi.TranslationKey);
                        else if (fi.Name == "Flour machine") box.Items[slotCounter] = new FlourMachine(fi.TranslationKey);
                        else if (fi.Name == "Bread machine") box.Items[slotCounter] = new BreadMachine(fi.TranslationKey);
                        else if (fi.Name == "Furnace") box.Items[slotCounter] = new Furnace(fi.TranslationKey);
                        else if (fi.Name == "Sign") box.Items[slotCounter] = new Sign(fi.TranslationKey);
                        else if (fi.Name == "Fence gate") box.Items[slotCounter] = new Gate("Fence gate", fi.TranslationKey);
                        else if (fi.Name == "Fence") box.Items[slotCounter] = new Wall(fi.Name, 5, 10, fi.TranslationKey);
                        else if (fi.Name == "Water pump") box.Items[slotCounter] = new WaterPump(fi.TranslationKey);
                        else box.Items[slotCounter] = new BuildableObject(fi.Name, 1, 1, fi.TranslationKey);
                        break;
                    case "Floor":
                        if (fi.Name == "Composite tile" || fi.Name == "Wood tile") box.Items[slotCounter] = new Floor(fi.Name, 10, 10, fi.TranslationKey);
                        else box.Items[slotCounter] = new Floor(fi.Name, 10, 10, fi.TranslationKey);
                        break;
                    case "Farm":
                        if (fi.Name == "Drip bottle") box.Items[slotCounter] = new DripBottle(0, fi.TranslationKey);
                        else if (fi.Name == "Drip irrigation kit") box.Items[slotCounter] = new DripIrrigationKit(fi.TranslationKey);
                        else box.Items[slotCounter] = new IObject(fi.Name, 1, 1, fi.TranslationKey);
                        break;
                    case "Seed":
                        box.Items[slotCounter] = new Seed(fi.Name, 10, 10, fi.TranslationKey);
                        break;                            
                    case "Fertilizer":
                        box.Items[slotCounter] = new Fertilizer(10, 10, fi.TranslationKey);
                        break;
                }

                if (slotCounter == 3)
                {
                    box = new Box("Delivery box", fi.TranslationKey);
                    DeliverySystem.Data.DeliveryList.Add(box);
                    slotCounter = 0;
                }
                else slotCounter++;
            }
            item.RemoveIt();
        }

        if (DeliverySystem.Data.DeliveryList[DeliverySystem.Data.DeliveryList.Count - 1].Items[0] == null) DeliverySystem.Data.DeliveryList.Remove(DeliverySystem.Data.DeliveryList[DeliverySystem.Data.DeliveryList.Count - 1]);

        CheckState();

        Master.UpdateBalance(-TotalPrice);
        TotalPrice = 0;
        transform.Find("Chart info").Find("Price").GetComponent<Text>().text = "0€";
    }
}