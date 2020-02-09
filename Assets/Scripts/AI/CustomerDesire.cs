using UnityEngine;

public class CustomerDesire
{
    public Product Item;
    public int MaxPrice;
    public int Amount;

    public CustomerDesire(Product item, int mediumAmount)
    {
        Item = item;
        int mediumValue = item.MediumValue;
        MaxPrice = Random.Range((int)(mediumValue + mediumValue / 10), 2 * mediumValue);
        Amount = Random.Range(mediumAmount - 3, mediumAmount + 3);
        if (Amount < 1) Amount = 1;
    }
}