using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeTools;
using UnityEngine.EventSystems;

public class StandObject : MonoBehaviour
{
    void OnMouseDown()
    {
        if (PlayerTools.ToolOnHand != null && PlayerTools.ToolOnHand.Name == "Basket" && !EventSystem.current.IsPointerOverGameObject())
        {
            Basket basket = GameObject.Find("Tools").transform.Find("Basket").GetComponent("Basket") as Basket;
            if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
            {
                if (basket.Product != null)
                {
                    int amountPlaced = Stands.StandsList.Find(x => x.Model == gameObject).AddProduct(basket.Product.Name, PlayerTools.ToolOnHand.Remaining);
                    PlayerTools.ToolOnHand.Remaining -= amountPlaced;
                    if (PlayerTools.ToolOnHand.Remaining == 0)
                    { 
                        basket.Product = null;  
                        Inventory.ChangeSubobject("None", "None");
                    }  
                    else Inventory.ChangeSubobject(basket.Product.Name, "Crop");
                }
                else
                {
                    if (PlayerTools.ToolOnHand.Remaining < 20)
                    {
                        Tuple pickUpStand = Stands.StandsList.Find(x => x.Model == gameObject).PickUp();
                        if (pickUpStand.Item2 > 0)
                        {
                            basket.Product = Products.ProductsList.Find(x => x.Name == pickUpStand.Item1);
                            PlayerTools.ToolOnHand.Remaining += pickUpStand.Item2;
                            Inventory.ChangeSubobject(basket.Product.Name, "Crop");
                        }
                    }
                }
            }
        }
    }
}