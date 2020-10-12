using UnityEngine;
using UnityEngine.UI;

public class FarmazonItem
{
    public string Name;
    public string Use;
    public int Price;
    public string Description;
    public string Category;
    public string TranslationKey;

    public FarmazonItem(string name, string use, int price, string description, string category, string translationKey)
    {
        Name = name;
        Use = use;
        Price = price;
        Description = description;
        Category = category;
        TranslationKey = translationKey;

        // Add to Farmazon shop list
        GameObject farmazonButton = MonoBehaviour.Instantiate(Resources.Load<GameObject>("UI/Farmazon item"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        farmazonButton.GetComponent<Button>().onClick.AddListener(delegate () { (GameObject.Find("UI").transform.Find("Farmazon").GetComponent("Farmazon") as Farmazon).SelectItem(name); });
        
        if (use == "Seed")
        {
            farmazonButton.transform.Find("Name").GetComponent<Text>().text = name + " seeds";
            farmazonButton.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<ObjectInfo>("Objects info/" + name + " seeds").Icon;
        }
        else
        {
            farmazonButton.transform.Find("Name").GetComponent<Text>().text = name;
            farmazonButton.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<ObjectInfo>("Objects info/" + name).Icon;
        }
        farmazonButton.transform.Find("Price").GetComponent<Text>().text = price + "$/u";
        farmazonButton.transform.SetParent(Farmazon.FarmazonListHandler, false);
        farmazonButton.name = name;
    }
}