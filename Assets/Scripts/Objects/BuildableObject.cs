﻿[System.Serializable]
public class BuildableObject : IObject
{
    public BuildableObject(string name, int stack, int maxStack, string translationKey) : base(name, stack, maxStack, translationKey)
    { 
        Name = name;
    }
}