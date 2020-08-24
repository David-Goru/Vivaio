using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ObjectInfo", order = 1)]
public class ObjectInfo : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public int RotPositions;
    public Sprite[] Sprites;
    public Vector2[] CollOffset;
    public Vector2[] CollSize;
}