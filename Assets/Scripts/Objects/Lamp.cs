using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lamp : BuildableObject
{
    [SerializeField]
    public bool On;

    public Lamp(string name) : base(name, 1, 1)
    {
        On = false;
    }

    public override void ActionTwo()
    {
        On = !On;
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<ObjectInfo>("Objects info/" + Name).Sprites[On ? 1 : 0];
    }
}