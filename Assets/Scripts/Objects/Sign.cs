using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sign : BuildableObject
{
    [SerializeField]
    public string Icon;

    public Sign(string translationKey) : base("Sign", 1, 1, translationKey)
    {
        Icon = "None";
    }

    public void UpdateIcon(string newIcon)
    {
        Icon = newIcon;
        Model.transform.Find("Icon").gameObject.GetComponent<SpriteRenderer>().sprite = IconsHandler.Icons.Find(x => x.Name == newIcon).Sprite;
    }

    public override void ActionTwo()
    {
        ObjectUI.OpenUI(this);
    }

    public override void LoadObjectCustom()
    {
        UpdateIcon(Icon);
    }
}