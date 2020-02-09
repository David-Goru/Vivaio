using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class Products : MonoBehaviour
{
    public static List<Product> ProductsList;

    void Start()
    {
        ProductsList = new List<Product>();
        GetProducts();
    }

    public void GetProducts()
    {
        XmlDocument productsDoc = new XmlDocument();
        productsDoc.Load(Application.dataPath + "/Data/Products.xml");
        XmlNodeList products = productsDoc.GetElementsByTagName("Product");

        foreach (XmlNode product in products)
        {
            ProductsList.Add(new Product(product["Name"].InnerText, Resources.Load<Sprite>("Crops/" + product["Name"].InnerText), int.Parse(product["MediumValue"].InnerText)));
        }
    }
}