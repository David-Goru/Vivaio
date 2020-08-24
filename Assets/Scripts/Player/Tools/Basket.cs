using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Basket : Tool
{
    [System.NonSerialized]
    public Product Product;
    [SerializeField]
    public int Amount;
}