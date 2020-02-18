using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ItemsHandler : MonoBehaviour
{
    public static List<Item> ItemsList;

    public class Item : IObject
    {
        public string Name;
        public string Use;
        public int Amount; // Only seeds

        public Item(string name, string use)
        {
            Name = name;
            Use = use;
            Amount = 0;
        }
    }

    void Start()
    {
        ItemsList = new List<Item>();

        XmlDocument itemsDoc = new XmlDocument();
        itemsDoc.Load(Application.dataPath + "/Data/Items.xml");
        XmlNodeList items = itemsDoc.GetElementsByTagName("Item");

        foreach (XmlNode item in items)
        {
            ItemsList.Add(new Item(item["Name"].InnerText, item["Use"].InnerText));
        }
    }
}