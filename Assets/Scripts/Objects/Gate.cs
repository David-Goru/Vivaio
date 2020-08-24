using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gate : BuildableObject
{
    [SerializeField]
    public bool Opened;

    public Gate(string name) : base(name, 1, 1, true)
    {
        Opened = false;
    }

    public override void ActionTwo()
    {
        Opened = !Opened;
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<ObjectInfo>("Objects info/" + Name).Sprites[Rotation + (Opened ? 4 : 0)];
        Model.transform.Find("Obstacle " + Rotation).gameObject.SetActive(!Opened);
        Model.transform.Find("Obstacle " + (Rotation + 4)).gameObject.SetActive(Opened);
        
        ObjectInfo oi = Resources.Load<ObjectInfo>("Objects info/" + Name);
        Model.GetComponent<BoxCollider2D>().offset = oi.CollOffset[Rotation + (Opened ? 4 : 0)];
        Model.GetComponent<BoxCollider2D>().size = oi.CollSize[Rotation + (Opened ? 4 : 0)];
    }
}