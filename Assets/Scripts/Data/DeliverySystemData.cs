using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeliverySystemData
{
    [SerializeField]    
    public List<Box> DeliveryList;
    [SerializeField]  
    public List<DeliveryPoint> DeliveryPoints;
}