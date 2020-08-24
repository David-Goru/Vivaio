using UnityEngine;

[System.Serializable]
public class CartItem
{
    [SerializeField]
    public string Item;
    [SerializeField]
    public int Amount;
    
    [System.NonSerialized]
    public GameObject UIModel;

    public CartItem(string item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public void RemoveIt()
    {
        MonoBehaviour.Destroy(UIModel);
        Farmazon.CartItems.Remove(this);
    }
}