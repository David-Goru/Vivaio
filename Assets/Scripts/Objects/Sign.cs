using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sign : BuildableObject
{
    [SerializeField]
    public string Icon;

    public Sign() : base("Sign", 1, 1)
    {
        Icon = "None";
    }

    public void UpdateIcon(string newIcon)
    {
        Icon = newIcon;
        Model.transform.Find("Icon").gameObject.GetComponent<SpriteRenderer>().sprite = IconsHandler.Icons.Find(x => x.Name == newIcon).Sprite;
    }
}