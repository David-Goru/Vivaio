using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIData
{
    [SerializeField]
    public List<CustomerData> Customers;
    [SerializeField]
    public Vector2 CashRegisterPos;
}

[System.Serializable]
public class CustomerData
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public int Trust;
    [SerializeField]
    public bool LetterSent;
    [SerializeField]
    public int GlassBottles;

    public CustomerData(string name, int trust, bool letterSent, int glassBottles)
    {
        Name = name;
        Trust = trust;
        LetterSent = letterSent;
        GlassBottles = glassBottles;
    }
}