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
        farmazonButton.GetComponent<Button>().onClick.AddListener(delegate () { UI.Elements["Farmazon"].GetComponent<Farmazon>().SelectItem(name); });
        
        farmazonButton.transform.Find("Name").GetComponent<Text>().text = Localization.Translations[TranslationKey];
        if (use == "Seed") farmazonButton.transform.Find("Image").GetComponent<Image>().sprite = UI.Sprites[name + " seeds"];
        else farmazonButton.transform.Find("Image").GetComponent<Image>().sprite = UI.Sprites[name];
        
        farmazonButton.transform.Find("Price").GetComponent<Text>().text = price + "€/u";
        farmazonButton.transform.SetParent(Farmazon.FarmazonListHandler, false);
        farmazonButton.name = name;
    }
}