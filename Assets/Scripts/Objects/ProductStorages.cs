using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductStorages : MonoBehaviour
{
    public static List<ProductBox> PBList;

    void Start()
    {
        PBList = new List<ProductBox>();
    }
}