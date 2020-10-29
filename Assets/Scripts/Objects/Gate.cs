using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gate : BuildableObject
{
    [SerializeField]
    public bool Opened;

    public Gate(string name, string translationKey) : base(name, 1, 1, translationKey)
    {
        Opened = false;
    }

    public override void ActionTwo()
    {
        Model.transform.Find("Rotation " + Rotation).Find(Opened ? "Open" : "Closed").gameObject.SetActive(false);
        Opened = !Opened;

        GameObject gateState = Model.transform.Find("Rotation " + Rotation).Find(Opened ? "Open" : "Closed").gameObject;        
        gateState.SetActive(true);
        
        Model.GetComponent<BoxCollider2D>().offset = gateState.GetComponent<BoxCollider2D>().offset;
        Model.GetComponent<BoxCollider2D>().size = gateState.GetComponent<BoxCollider2D>().size;
    }

    public override void LoadObjectCustom()
    {
        Model.transform.Find("Rotation 0").Find("Closed").gameObject.SetActive(false);
        GameObject gateState = Model.transform.Find("Rotation " + Rotation).Find(Opened ? "Open" : "Closed").gameObject;        
        gateState.SetActive(true);

        Model.GetComponent<BoxCollider2D>().offset = gateState.GetComponent<BoxCollider2D>().offset;
        Model.GetComponent<BoxCollider2D>().size = gateState.GetComponent<BoxCollider2D>().size;
    }

    public override void RotateObject()
    {
        Master.RunSoundStatic(SoundsHandler.RotateObjectStatic);

        Model.transform.Find("Rotation " + Rotation).Find(Opened ? "Open" : "Closed").gameObject.SetActive(false);
        Model.transform.Find("Rotation " + Rotation).Find(Opened ? "Open" : "Closed").gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        Rotation++;
        if (Rotation == 4) Rotation = 0;

        GameObject gateState = Model.transform.Find("Rotation " + Rotation).Find(Opened ? "Open" : "Closed").gameObject;
        gateState.SetActive(true);

        Model.GetComponent<BoxCollider2D>().offset = gateState.GetComponent<BoxCollider2D>().offset;
        Model.GetComponent<BoxCollider2D>().size = gateState.GetComponent<BoxCollider2D>().size;
    }
}