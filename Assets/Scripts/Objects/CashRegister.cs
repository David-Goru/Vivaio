using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CashRegister : BuildableObject
{
    public CashRegister(string translationKey) : base("Cash register", 1, 1, translationKey) {}

    public override void LoadObjectCustom()
    {        
        CashRegisterHandler.CashRegisterModel = Model;

        CashRegisterHandler.CustomerPos = new List<Vector2>();
        foreach (Transform t in Model.transform.Find("Customer position"))
        {
            CashRegisterHandler.CustomerPos.Add(t.position);
        }
    }

    public override void ActionTwo()
    {
        CashRegisterHandler.OpenCashLog();
    }
}