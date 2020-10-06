using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviour
{
    // General
    public static string GameVersion = "Alpha 8"; // Update every version?
    public static string GameName = "None";
    public static string PlayerName = "Nobody";
    public static bool LoadingGame = false;
    public static string Language = "en_EN";
    public static GameObject Player;

    // In-game
    public static GeneralData Data;

    // When loading a game
    public static bool Load(GeneralData data)
    {
        try
        {
            Data = data;
            Player = GameObject.Find("Player");
            GameObject.Find("UI").transform.Find("Money").transform.Find("Text").gameObject.GetComponent<Text>().text = Data.Balance + "$";
        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "Master", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Data = new GeneralData(PlayerName, 2000, 0, 3000, 0, 100, 0, 0, 1);
        Player = GameObject.Find("Player");
        GameObject.Find("UI").transform.Find("Money").transform.Find("Text").gameObject.GetComponent<Text>().text = Data.Balance + "$";

        return true;
    }

    public static void UpdateBalance(int money)
    {
        Data.Balance += money;
        GameObject.Find("UI").transform.Find("Money").transform.Find("Text").gameObject.GetComponent<Text>().text = Data.Balance + "$";

        Transform MoneyHandler = GameObject.Find("UI").transform.Find("Money").Find("Money updates").Find("Viewport").Find("Content");

        GameObject uiElement = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Money " + (money < 0 ? "removed" : "added")), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        uiElement.GetComponent<Text>().text = (money < 0 ? "" : "+") + money + "$";
        uiElement.transform.SetParent(MoneyHandler);
        uiElement.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}