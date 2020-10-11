using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ObjectInfo", order = 1)]
public class ObjectInfo : ScriptableObject
{
    public string Name;
    public Sprite Icon;
}