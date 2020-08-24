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

    public CustomerData(string name, int trust, bool letterSent)
    {
        Name = name;
        Trust = trust;
        LetterSent = letterSent;
    }
}