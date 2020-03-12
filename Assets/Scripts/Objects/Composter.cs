using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Composter : MonoBehaviour
{
    public string State;
    public int Amount;

    void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            if (State == "Available")
            {
                if (Inventory.ObjectInHand is Basket && !EventSystem.current.IsPointerOverGameObject())
                {
                    Basket basket = (Basket)Inventory.ObjectInHand;
                    if (basket.Product != null && basket.Product.Name == "Sticks")
                    {
                        int needs = 10 - Amount;
                        if (needs > basket.Amount)
                        {
                            Amount += basket.Amount;
                            basket.Amount = 0;
                            basket.Product = null;
                            Inventory.ChangeObject();
                        }
                        else
                        {
                            if (needs == basket.Amount)
                            {
                                basket.Amount = 0;
                                basket.Product = null;
                            }
                            else basket.Amount -= needs;
                            Inventory.ChangeObject();

                            Amount = 10;
                            State = "Working";
                            transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Working");
                            StartCoroutine(FertilizeIt());
                        }
                    }
                }
            }
            else if (State == "Finished")
            {
                if (!Inventory.InventorySlot.activeSelf && !EventSystem.current.IsPointerOverGameObject())
                {
                    transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Available");
                    Amount = 0;
                    State = "Available";
                    Inventory.ObjectInHand = new Fertilizer(10);
                    Inventory.ObjectInHand.Name = "Fertilizer";
                    Inventory.ChangeObject();
                }
            }
        }
    }

    IEnumerator FertilizeIt()
    {
        yield return new WaitForSeconds(20);
        transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/Composter/Finished");
        State = "Finished";
    }
}