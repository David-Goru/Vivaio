using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsHandler : MonoBehaviour
{
    public AudioClip UseSeeds;
    public static AudioClip UseSeedsStatic;
    public AudioClip UseFertilizer;
    public static AudioClip UseFertilizerStatic;
    public AudioClip PlaceObject;
    public static AudioClip PlaceObjectStatic;
    public AudioClip MoveObject;
    public static AudioClip MoveObjectStatic;
    public AudioClip RotateObject;
    public static AudioClip RotateObjectStatic;

    void Start()
    {
        UseSeedsStatic = UseSeeds;
        UseFertilizerStatic = UseFertilizer;
        PlaceObjectStatic = PlaceObject;
        MoveObjectStatic = MoveObject;
        RotateObjectStatic = RotateObject;
    }
}