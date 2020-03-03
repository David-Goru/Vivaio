using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeTools;
using UnityEngine.EventSystems;

public class StandObject : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Inventory.ObjectInHand is Basket && !EventSystem.current.IsPointerOverGameObject())
        {
            Basket basket = (Basket)Inventory.ObjectInHand;
            if (basket.Product != null && basket.Product.Name == "Sticks") return;
            if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
            {
                if (basket.Product != null)
                {
                    int amountPlaced = Stands.StandsList.Find(x => x.Model == gameObject).AddProduct(basket.Product.Name, basket.Amount);
                    basket.Amount -= amountPlaced;
                    if (basket.Amount == 0) basket.Product = null;  
                    Inventory.ChangeObject();
                }
                else
                {
                    if (basket.Amount < 20)
                    {
                        Tuple pickUpStand = Stands.StandsList.Find(x => x.Model == gameObject).PickUp();
                        if (pickUpStand.Item2 > 0)
                        {
                            basket.Product = Products.ProductsList.Find(x => x.Name == pickUpStand.Item1);
                            basket.Amount += pickUpStand.Item2;
                            Inventory.ChangeObject();
                        }
                    }
                }
            }
        }
    }
}