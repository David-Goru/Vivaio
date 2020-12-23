using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        UI.OpenNewObjectUI(this);
    }

    public override void LoadObjectCustom()
    {
        UpdateIcon(Icon);
    }

    // UI stuff
    public override void OpenUI()
    {            
        if (!IconsHandler.Icons.Exists(x => x.Name == Icon)) Icon = "None";

        UI.Elements["Sign selected icon"].GetComponent<Image>().sprite = IconsHandler.Icons.Find(x => x.Name == Icon).Sprite;
        UI.Elements["Sign"].SetActive(true);
    }

    public override void CloseUI()
    {
        UI.Elements["Sign"].SetActive(false);
    }

    public static void InitializeUIButtons()
    {
        UI.Elements["Sign take object button"].GetComponent<Button>().onClick.AddListener(() => TakeObject());  
    }

    public static void ChooseIcon(string icon)
    {
        if (UI.ObjectOnUI is Sign)
        {
            Sign s = (Sign)UI.ObjectOnUI;
            s.UpdateIcon(icon);
            UI.Elements["Sign selected icon"].GetComponent<Image>().sprite = IconsHandler.Icons.Find(x => x.Name == s.Icon).Sprite;
        }
    }
}