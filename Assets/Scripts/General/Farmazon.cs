using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Farmazon : MonoBehaviour
{
    public GameObject ItemUI;
    List<FarmazonItem> farmazonItems;
    public static List<CartItem> CartItems;
    FarmazonItem selectedItem;
    public int Amount;
    int totalPrice;
    public static Transform FarmazonListHandler;
    public GameObject CartCircle;

    public class FarmazonItem
    {
        public string Name;
        public string Use;
        public int Price;
        public string Description;

        public FarmazonItem(string name, string use, int price, string description)
        {
            Name = name;
            Use = use;
            Price = price;
            Description = description;

            // Add to Farmazon shop list
            GameObject farmazonButton = Instantiate(Resources.Load<GameObject>("UI/Farmazon item"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            farmazonButton.GetComponent<Button>().onClick.AddListener(delegate () { (GameObject.Find("UI").transform.Find("Farmazon").GetComponent("Farmazon") as Farmazon).SelectItem(name); });
            farmazonButton.transform.Find("Name").GetComponent<Text>().text = name;
            farmazonButton.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + name);
            farmazonButton.transform.Find("Price").GetComponent<Text>().text = price + "€/u";
            farmazonButton.transform.SetParent(FarmazonListHandler, false);
            farmazonButton.name = name;
        }
    }

    public class CartItem
    {
        public FarmazonItem Item;
        public GameObject UIModel;
        public int Amount;

        public CartItem(FarmazonItem item, int amount)
        {
            Item = item;
            Amount = amount;
        }

        public void RemoveIt()
        {
            Destroy(UIModel);
            CartItems.Remove(this);
        }
    }

    void Start()
    {
        FarmazonListHandler = transform.Find("Items").Find("Viewport").Find("Content");
        
        farmazonItems = new List<FarmazonItem>();
        CartItems = new List<CartItem>();

        LoadFarmazonItems();
    }

    public void CheckState()
    {
        if (CartItems.Count > 0) CartCircle.SetActive(true);
        else CartCircle.SetActive(false);
    }

    public void LoadFarmazonItems()
    {
        XmlDocument itemsDoc = new XmlDocument();
        itemsDoc.Load(Application.dataPath + "/Data/Farmazon items.xml");
        XmlNodeList items = itemsDoc.GetElementsByTagName("FarmazonItem");

        foreach (XmlNode item in items)
        {
            farmazonItems.Add(new FarmazonItem(item["Name"].InnerText, item["Use"].InnerText, int.Parse(item["Price"].InnerText), item["Description"].InnerText));
        }

        // Add all items to items scroll list
    }

    public void SelectItem(string itemName)
    {
        selectedItem = farmazonItems.Find(x => x.Name == itemName);

        Amount = 0;
        ItemUI.transform.Find("Item").Find("Amount info").Find("Amount").GetComponent<Text>().text = "0";
        ItemUI.transform.Find("Item").Find("Amount info").Find("Price").GetComponent<Text>().text = "0€";

        ItemUI.transform.Find("Item").gameObject.SetActive(true);
        ItemUI.transform.Find("Item").Find("Name").GetComponent<Text>().text = itemName;
        ItemUI.transform.Find("Item").Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + itemName);
        ItemUI.transform.Find("Item").Find("Description").GetComponent<Text>().text = selectedItem.Description;
        ItemUI.transform.Find("No selected item").gameObject.SetActive(false);
    }

    public void AddAmount()
    {
        if (Amount >= 99) return;
        Amount++;
        ItemUI.transform.Find("Item").Find("Amount info").Find("Amount").GetComponent<Text>().text = Amount.ToString();
        ItemUI.transform.Find("Item").Find("Amount info").Find("Price").GetComponent<Text>().text = (Amount * selectedItem.Price) + "€";
    }
    public void RemoveAmount()
    {
        if (Amount <= 0) return;
        Amount--;
        ItemUI.transform.Find("Item").Find("Amount info").Find("Amount").GetComponent<Text>().text = Amount.ToString();
        ItemUI.transform.Find("Item").Find("Amount info").Find("Price").GetComponent<Text>().text = (Amount * selectedItem.Price) + "€";
    }

    public void AddToCart()
    {
        if (Amount == 0) return;
        CartItems.Add(new CartItem(selectedItem, Amount));
        totalPrice += Amount * selectedItem.Price;
        Amount = 0;
        ItemUI.transform.Find("Item").gameObject.SetActive(false);
        ItemUI.transform.Find("No selected item").gameObject.SetActive(true);
        CheckState();
    }

    public void OpenCart()
    {
        transform.Find("Chart info").Find("Price").GetComponent<Text>().text = totalPrice + "€";

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
            itemObject.transform.Find("Name").GetComponent<Text>().text = item.Item.Name;
            itemObject.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + item.Item.Name);
            itemObject.transform.Find("Price").GetComponent<Text>().text = item.Amount + "u (" + item.Amount * item.Item.Price + "€)";
            itemObject.transform.SetParent(content, false);
        }
    }

    public void RemoveItem(CartItem item)
    {
        totalPrice -= item.Amount * item.Item.Price;
        transform.Find("Chart info").Find("Price").GetComponent<Text>().text = totalPrice + "€";
        item.RemoveIt();
    }

    public void Buy()
    {
        if (Master.Balance < totalPrice) return;

        foreach (CartItem item in CartItems.ToArray())
        {
            for (int am = 0; am < item.Amount; am++)
            {
                DeliverySystem.DeliveryBox box = DeliverySystem.DeliveryList.Find(x => x.Items[3] == null && x.Placed == false);
                if (box == null)
                {
                    box = new DeliverySystem.DeliveryBox();
                    DeliverySystem.DeliveryList.Add(box);
                    box.Items[0] = ItemsHandler.ItemsList.Find(x => x.Name == item.Item.Name);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (box.Items[i] == null)
                        {
                            box.Items[i] = ItemsHandler.ItemsList.Find(x => x.Name == item.Item.Name);
                            break;
                        }
                    }
                }
            }

            item.RemoveIt();
        }

        CheckState();

        Master.UpdateBalance(-totalPrice);
        totalPrice = 0;
        transform.Find("Chart info").Find("Price").GetComponent<Text>().text = "0€";
    }
}