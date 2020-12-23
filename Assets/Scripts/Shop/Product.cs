using UnityEngine;

public class Product
{
    public string Name;
    public string Type;
    public Sprite Icon;
    public int MediumValue;

    public Product(string name, string type, Sprite icon, int mediumValue)
    {
        Name = name;
        Type = type;
        Icon = icon;
        MediumValue = mediumValue;
    }
}