using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeTools;
using UnityEngine.EventSystems;

public class ProductStorage : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Inventory.ObjectInHand is Basket && !EventSystem.current.IsPointerOverGameObject())
        {
            Basket basket = (Basket)Inventory.ObjectInHand;
            if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
            {
                if (basket.Product != null)
                {
                    int amountPlaced = ProductStorages.PBList.Find(x => x.Model == gameObject).AddProduct(basket.Product.Name, basket.Amount);
                    basket.Amount -= amountPlaced;
                    if (basket.Amount == 0) basket.Product = null;                    
                    Inventory.ChangeObject();
                }
                else
                {
                    if (basket.Amount < 50)
                    {
                        Tuple pickUpStorage = ProductStorages.PBList.Find(x => x.Model == gameObject).PickUp();
                        if (pickUpStorage.Item2 > 0)
                        {
                            basket.Product = Products.ProductsList.Find(x => x.Name == pickUpStorage.Item1);
                            basket.Amount += pickUpStorage.Item2;
                            Inventory.ChangeObject();
                        }
                    }
                }
            }
        }
    }
}