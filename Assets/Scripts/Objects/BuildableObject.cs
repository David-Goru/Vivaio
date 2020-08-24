[System.Serializable]
public class BuildableObject : IObject
{
    public BuildableObject(string name, int stack, int maxStack, bool canRot = false) : base(name, stack, maxStack, canRot)
    { 
        Name = name;
    }
}