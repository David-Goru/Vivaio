using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lamp : BuildableObject
{
    [SerializeField]
    public bool On;

    public Lamp(string name, string translationKey) : base(name, 1, 1, translationKey)
    {
        On = false;
    }

    public override void ActionTwo()
    {
        On = !On;
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Model.transform.Find(On ? "On" : "Off").GetComponent<SpriteRenderer>().sprite;
    }

    public override void LoadObjectCustom()
    {
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Model.transform.Find(On ? "On" : "Off").GetComponent<SpriteRenderer>().sprite;
    }
}