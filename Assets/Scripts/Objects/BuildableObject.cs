[System.Serializable]
public class BuildableObject : IObject
{
    public BuildableObject(string name, int stack, int maxStack) : base(name, stack, maxStack)
    { 
        Name = name;
    }
}