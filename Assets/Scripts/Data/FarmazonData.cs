using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FarmazonData
{
    [SerializeField]
    public List<CartItem> CartItems;
    [SerializeField]
    public int TotalPrice;
}