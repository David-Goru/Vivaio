using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : Tool
{
    public Product Product;
    public GameObject ProductSprite;

    public override void TakeTool()
    {
        PlayerTools.ToolOnHand = this;
        Inventory.ChangeObject(Name, "Tool");
        if (Product != null) Inventory.ChangeSubobject(Product.Name, "Crop");
        ChangeVisual();
    }
}