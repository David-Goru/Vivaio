using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class IconsHandler : MonoBehaviour
{
    public static List<Icon> Icons;

    public static bool New()
    {
        try
        {
            Icons = new List<Icon>();

            Icons.Add(new Icon("None", UI.Sprites["None"]));
            GameObject noneIcon = Instantiate(Resources.Load<GameObject>("UI/Icon"), UI.Elements["Sign choose icon content"].transform.position, UI.Elements["Sign choose icon content"].transform.rotation);
            noneIcon.GetComponent<Image>().sprite = Icons[0].Sprite;
            noneIcon.GetComponent<Button>().onClick.AddListener(() => Sign.ChooseIcon(Icons[0].Name));
            noneIcon.transform.SetParent(UI.Elements["Sign choose icon content"].transform, false);

            DirectoryInfo icons = new DirectoryInfo(Application.dataPath + "/Data/Icons/");
            foreach (FileInfo f in icons.GetFiles())
            {
                if (!f.Name.EndsWith(".png")) continue;
                Texture2D iconTexture = new Texture2D(2, 2);
                byte[] FileData; 
                FileData = File.ReadAllBytes(f.FullName);
                iconTexture.LoadImage(FileData);
                iconTexture.filterMode = FilterMode.Point;
                Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, -1.35f), 64);

                Icon i = new Icon(Path.GetFileNameWithoutExtension(f.Name), iconSprite);

                GameObject g = Instantiate(Resources.Load<GameObject>("UI/Icon"), UI.Elements["Sign choose icon content"].transform.position, UI.Elements["Sign choose icon content"].transform.rotation);
                g.GetComponent<Image>().sprite = iconSprite;
                g.GetComponent<Button>().onClick.AddListener(() => Sign.ChooseIcon(i.Name));
                g.transform.SetParent(UI.Elements["Sign choose icon content"].transform, false);

                Icons.Add(i);
            }
        }        
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "IconsHandler", e));
        }

        return true;
    }
}

public class Icon
{
    public string Name;
    public Sprite Sprite;

    public Icon(string name, Sprite sprite)
    {
        Name = name;
        Sprite = sprite;
    }
}