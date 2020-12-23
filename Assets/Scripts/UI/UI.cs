using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static Dictionary<string, Sprite> Sprites;
    public static Dictionary<string, GameObject> Elements;
    public static List<GameObject> LocalizableTexts;

    // Objects UI stuff
    [SerializeField] float objectRange = 2.5f;
    public static IObject ObjectOnUI;

    // We don't like garbage
    UIElement element;

    void Awake()
    {
        Sprites = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI sprites"))
        {
            Sprites.Add(sprite.name, sprite);
        }

        Elements = new Dictionary<string, GameObject>();
        LocalizableTexts = new List<GameObject>();

        addAllChildrenToUI(transform);
        initializeObjectsButtons();
    }

    void Update()
    {
        if (ObjectOnUI == null) return;        

        if (Vector2.Distance(Master.Player.transform.position, ObjectOnUI.Model.transform.position) > objectRange)
        {            
            CloseUI();
            return;
        }

        ObjectOnUI.UpdateUI();
    }

    void addAllChildrenToUI(Transform p)
    {
        foreach (Transform c in p)
        {
            element = c.GetComponent<UIElement>();
            if (element != null)
            {
                if (element.IsLocalizable) LocalizableTexts.Add(c.gameObject);
                else Elements.Add(c.gameObject.name, c.gameObject);
            }

            addAllChildrenToUI(c);
        }
    }

    void initializeObjectsButtons()
    {
        Box.InitializeUIButtons();
        BreadMachine.InitializeUIButtons();
        CashRegister.InitializeUIButtons();
        Composter.InitializeUIButtons();
        DeseedingMachine.InitializeUIButtons();
        FlourMachine.InitializeUIButtons();
        Furnace.InitializeUIButtons();
        GarbageCan.InitializeUIButtons();
        ProductBox.InitializeUIButtons();
        SeedBox.InitializeUIButtons();
        Sign.InitializeUIButtons();
        Stand.InitializeUIButtons();
        WaterBottlingMachine.InitializeUIButtons();
    }

    public static void OpenNewObjectUI(IObject newObjectForUI)
    {
        if (ObjectOnUI != null) ObjectOnUI.CloseUI();
        ObjectOnUI = newObjectForUI;
        ObjectOnUI.OpenUI();
    }

    public void CloseUI()
    {
        if (ObjectOnUI != null) ObjectOnUI.CloseUI();
        ObjectOnUI = null;
    }
}