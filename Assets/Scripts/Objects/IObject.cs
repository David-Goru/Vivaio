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
    public bool CanRot;
    [SerializeField]
    public int Rotation;

    public IObject(string name, int stack, int maxStack, bool canRot = false)
    {
        Name = name;
        Stack = stack;
        MaxStack = maxStack;
        Placed = false;
        Rotation = 0;
        CanRot = canRot;
    }

    public virtual void ActionOne() {}

    public virtual void ActionTwo() {}
}