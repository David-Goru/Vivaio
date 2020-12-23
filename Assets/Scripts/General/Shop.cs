using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Shop : MonoBehaviour
{
    public static int TodayEarnings;

    // We don't like garbage
    GameObject uiElement;

    public void InaugurateShop()
    {
        if (Master.Data.ShopInaugurated) return;

        UI.Elements["Shop inauguration"].SetActive(false);
        UI.Elements["Shop management"].SetActive(true);

        OpenShopUI();
    }

    public void OpenShopUI()
    {
        if (!Master.Data.ShopInaugurated)
        {
            UI.Elements["Shop"].SetActive(true);
            return;
        }

        // Earnings
        int days = Master.Data.ShopEarnings.Count;
        int yesterdayEarnings = days >= 1 ? Master.Data.ShopEarnings[days - 1] : 0;
        UI.Elements["Shop daily earnings yesterday"].GetComponent<Text>().text = string.Format(Localization.Translations["shop_earnings_yesterday_value"], yesterdayEarnings);

        if (days >= 7)
        {
            int last7DaysEarnings = Master.Data.ShopEarnings.GetRange(0, 7).Sum();
            UI.Elements["Shop earnings last 7 days"].GetComponent<Text>().text = string.Format(Localization.Translations["shop_earnings_last_7_days_value"], last7DaysEarnings, Mathf.Floor(last7DaysEarnings / 7));
        }
        else UI.Elements["Shop earnings last 7 days"].GetComponent<Text>().text = Localization.Translations["shop_earnings_no_data"];

        if (days >= 14)
        {
            int last14DaysEarnings = Master.Data.ShopEarnings.GetRange(0, 14).Sum();
            UI.Elements["Shop earnings last 14 days"].GetComponent<Text>().text = string.Format(Localization.Translations["shop_earnings_last_14_days_value"], last14DaysEarnings, Mathf.Floor(last14DaysEarnings / 14));
        }
        else UI.Elements["Shop earnings last 14 days"].GetComponent<Text>().text = Localization.Translations["shop_earnings_no_data"];

        // Stock
        Dictionary<string, int> productsStock = new Dictionary<string, int>();
        Dictionary<string, int> productsMaxStock = new Dictionary<string, int>();

        foreach (Stand s in Master.Data.Stands)
        {
            if (s.ItemName != "None")
            {
                if (productsStock.ContainsKey(s.ItemName)) productsStock[s.ItemName] += s.Amount;
                else productsStock.Add(s.ItemName, s.Amount);
                if (productsMaxStock.ContainsKey(s.ItemName)) productsMaxStock[s.ItemName] += s.MaxAmount;
                else productsMaxStock.Add(s.ItemName, s.MaxAmount);
            }
        }

        foreach (KeyValuePair<string, int> product in productsStock)
        {            
            uiElement = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Product stock"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            uiElement.GetComponent<Text>().text = string.Format(Localization.Translations["shop_product_stock"], product.Key, product.Value, productsMaxStock[product.Key]);
            uiElement.transform.SetParent(UI.Elements["Products list"].transform, false);
            uiElement.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        
        UI.Elements["Shop"].SetActive(true);
    }
}