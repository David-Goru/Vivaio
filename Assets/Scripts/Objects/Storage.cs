using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    public bool IsDelivery;
    public StorageSystem.Box BoxInfo;

    void Start()
    {
        if (!IsDelivery)
        {
            BoxInfo = new StorageSystem.Box(gameObject);
            StorageSystem.StorageList.Add(BoxInfo);
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        { 
            (GameObject.Find("UI").transform.Find("Storage").gameObject.GetComponent("StorageHandler") as StorageHandler).Box = gameObject;
            if (IsDelivery) DeliverySystem.DeliveryList.Find(x => x.Box == gameObject).BoxClicked();
            else BoxInfo.BoxClicked();
        }
    }
}