using UnityEngine;

[System.Serializable]
public class IObject
{
    [SerializeField]
    public string Name;
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

    public IObject(string name, int stack, int maxStack)
    {
        Name = name;
        Stack = stack;
        MaxStack = maxStack;
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
            Sprite sprite = ObjectsHandler.ObjectInfo[Name].Icon;

            Model.GetComponent<SpriteRenderer>().sprite = sprite;
            Model.GetComponent<Item>().ItemObject = this;
        }

        LoadObjectCustom();
    }

    public virtual void LoadObjectCustom() {}

    public virtual void RotateObject() {}
}