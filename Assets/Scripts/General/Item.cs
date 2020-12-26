using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    public IObject ItemObject;

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) StartCoroutine(takeItem());
    }
    
    IEnumerator takeItem()
    {
        yield return new WaitForSeconds(0.01f);

        int amountTaken = Inventory.AddObject(ItemObject);
        if (amountTaken == ItemObject.Stack)
        {
            ObjectsHandler.Data.Objects.Remove(ItemObject);
            Destroy(gameObject);
        }
        else ItemObject.Stack -= amountTaken;
    }

    void OnMouseEnter()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 255);
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}