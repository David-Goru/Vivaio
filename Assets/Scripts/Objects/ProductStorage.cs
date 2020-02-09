using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeTools;
using UnityEngine.EventSystems;

public class ProductStorage : MonoBehaviour
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
                    int amountPlaced = ProductStorages.PBList.Find(x => x.Model == gameObject).AddProduct(basket.Product.Name, PlayerTools.ToolOnHand.Remaining);
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
                    if (PlayerTools.ToolOnHand.Remaining < 50)
                    {
                        Tuple pickUpStorage = ProductStorages.PBList.Find(x => x.Model == gameObject).PickUp();
                        if (pickUpStorage.Item2 > 0)
                        {
                            basket.Product = Products.ProductsList.Find(x => x.Name == pickUpStorage.Item1);
                            PlayerTools.ToolOnHand.Remaining += pickUpStorage.Item2;
                            Inventory.ChangeSubobject(basket.Product.Name, "Crop");
                        }
                    }
                }
            }
        }
    }
}