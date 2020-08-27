﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeedBox : BuildableObject
{    
    [SerializeField]
    public Seed[] Seeds;

    public SeedBox() : base("Seed box", 1, 1)
    {
        Seeds = new Seed[8];
    }

    public void ClickSlot(int pos)
    {
        if (Inventory.Data.ObjectInHand is Seed)
        {
            Seed s = (Seed)Inventory.Data.ObjectInHand;
            if (Seeds[pos] == null)
            {
                Seeds[pos] = s;
                Seeds[pos].MaxStack = 25;
                Model.transform.Find("Slots").Find("Slot " + pos).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Farm/Seeds box/" + s.Type + " bag");
                Model.transform.Find("Slots").Find("Slot " + pos).gameObject.SetActive(true);
                Inventory.RemoveObject();

            }
            else if (Seeds[pos].Type == s.Type)
            {
                if (Seeds[pos].Stack < Seeds[pos].MaxStack)
                {
                    int amount = s.Stack;
                    if (Seeds[pos].Stack + amount <= Seeds[pos].MaxStack)
                    {
                        Seeds[pos].Stack += amount;
                        Inventory.RemoveObject();
                    }
                    else
                    {
                        amount = Seeds[pos].MaxStack - Seeds[pos].Stack;
                        Seeds[pos].Stack = Seeds[pos].MaxStack;
                        s.Stack -= amount;
                        Inventory.ChangeObject();
                    }
                }
                else if (s.Stack < s.MaxStack)
                {
                    int amount = s.MaxStack - s.Stack;
                    if (amount < Seeds[pos].Stack)
                    {
                        s.Stack += amount;
                        Seeds[pos].Stack -= amount;
                    }
                    else
                    {
                        s.Stack += Seeds[pos].Stack;
                        Seeds[pos] = null;
                        Model.transform.Find("Slots").Find("Slot " + pos).gameObject.SetActive(false);
                    }
                    Inventory.ChangeObject();
                }
            }
        }
        else if (Inventory.Data.ObjectInHand == null && Seeds[pos] != null)
        {
            Seed s;
            if (Seeds[pos].Stack > 10)
            {
                Seeds[pos].Stack -= 10;
                s = new Seed(Seeds[pos].Type, 10, 10);
            }
            else
            {
                s = new Seed(Seeds[pos].Type, Seeds[pos].Stack, 10);
                Seeds[pos] = null;
                Model.transform.Find("Slots").Find("Slot " + pos).gameObject.SetActive(false);
            }
            Inventory.AddObject(s);
        }
    }
}