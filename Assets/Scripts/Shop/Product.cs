using UnityEngine;

public class Product
{
    public string Name;
    public Sprite Icon;
    public int MediumValue;

    public Product(string name, Sprite icon, int mediumValue)
    {
        Name = name;
        Icon = icon;
        MediumValue = mediumValue;
    }
}