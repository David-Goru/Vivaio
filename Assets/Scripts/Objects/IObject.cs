using UnityEngine;

[System.Serializable]
public class IObject
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public string UIWindow;
    [SerializeField]
    public int Stack;
    [SerializeField]
    public int MaxStack;
    [SerializeField]
    public Vector2 WorldPosition;
    [SerializeField]
    public bool Placed;
    [System.NonSerialized]
    public GameObject Model;
    [SerializeField]
    public int Rotation;
    [SerializeField]
    public string TranslationKey;

    public IObject(string name, string uiWindow, int stack, int maxStack, string translationKey)
    {
        Name = name;
        UIWindow = uiWindow;
        Stack = stack;
        MaxStack = maxStack;
        TranslationKey = translationKey;
        Placed = false;
        Rotation = 0;
    }

    public virtual void ActionOne() {}

    public virtual void ActionTwo() {}

    public virtual void ActionTwoHard() {}

    public void LoadObject()
    {
        if (Placed) // Object built
        {
            Model = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Objects/" + Name), WorldPosition, Quaternion.Euler(0, 0, 0));                    
            Model.GetComponent<BoxCollider2D>().enabled = true;
        }
        else // Item on the floor
        {                    
            Model = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Item"), WorldPosition, Quaternion.Euler(0, 0, 0));
            Model.GetComponent<SpriteRenderer>().sprite = GetUISprite();
            Model.GetComponent<Item>().ItemObject = this;
        }

        LoadObjectCustom();
    }

    public virtual void LoadObjectCustom() {}

    public virtual void RotateObject() {}

    public virtual void OpenUI() {}

    public virtual void CloseUI() {}

    public virtual void UpdateUI() {}

    public virtual void OnObjectPlaced(GameObject model)
    {
        Model = model;
    }

    public virtual string GetUIName()
    {
        if (MaxStack > 1) return Localization.Translations[TranslationKey] + " (" + Stack + ")";
        return Localization.Translations[TranslationKey];
    }

    public virtual Sprite GetUISprite()
    {
        return UI.Sprites[Name];
    }
    
    public static void TakeObject()
    {  
        if (Inventory.AddObject(UI.ObjectOnUI) == 0) return;
        UI.ObjectOnUI.CloseUI();

        ObjectsHandler.Data.Objects.Remove(UI.ObjectOnUI);
        UI.ObjectOnUI.Placed = false;
        foreach (Transform t in UI.ObjectOnUI.Model.transform.Find("Vertices"))
        {                            
            Vertex v = VertexSystem.VertexFromPosition(t.transform.position);
            if (v != null) v.State = VertexState.Available;
        }
        MonoBehaviour.Destroy(UI.ObjectOnUI.Model);
        UI.ObjectOnUI = null;
    }
}