using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    public IObject ItemObject;

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Inventory.AddObject(ItemObject))
        {
            ObjectsHandler.Data.Objects.Remove(ItemObject);
            Destroy(gameObject);
        }
    }
}