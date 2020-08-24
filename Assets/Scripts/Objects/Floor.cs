using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Floor : BuildableObject
{

    public Floor(string name, int stack, int maxStack, bool canRot = false) : base(name, stack, maxStack, canRot) { }
}