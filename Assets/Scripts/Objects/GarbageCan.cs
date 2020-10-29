using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GarbageCan : BuildableObject
{
    [System.NonSerialized]
    public List<IObject> Items;

    public GarbageCan(string translationKey) : base("Garbage can", 1, 1, translationKey)
    {
        Items = new List<IObject>();
    }

    public override void ActionTwo()
    {
        GameObject.Find("UI").transform.Find("Garbage can").gameObject.SetActive(true);
        Model.GetComponent<GarbageCanHandler>().enabled = true;
        Model.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Garbage can open");
        Master.RunSoundStatic(SoundsHandler.OpenGarbageStatic);
    }
}