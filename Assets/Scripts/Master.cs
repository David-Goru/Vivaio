using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviour
{
    // General
    public static bool SpecialEdition = true;
    public static string GameEdition = "Vanilla";
    public static string GameVersion = "Version";
    public static string VersionDate = "Date";
    public static string Language = "en_EN";
    public static string GameName = "None";
    public static string PlayerName = "Nobody";
    public static bool LoadingGame = false;
    public static GameObject Player;
    public static AudioSource AudioHandler;

    // In-game
    public static GeneralData Data;

    // When loading a game
    public static bool Load(GeneralData data)
    {
        try
        {
            Data = data;
            Player = GameObject.Find("Player");
            AudioHandler = Player.transform.Find("Camera").GetComponent<AudioSource>();
            GameObject.Find("UI").transform.Find("Money").transform.Find("Text").gameObject.GetComponent<Text>().text = Data.Balance + "€";
            GameObject.Find("Farm handler").GetComponent<VersionHandlerGame>().LoadEditionStuff();
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
        AudioHandler = Player.transform.Find("Camera").GetComponent<AudioSource>();
        GameObject.Find("UI").transform.Find("Money").transform.Find("Text").gameObject.GetComponent<Text>().text = Data.Balance + "€";
        GameObject.Find("Farm handler").GetComponent<VersionHandlerGame>().LoadEditionStuff();

        return true;
    }

    public static int GetBalance()
    {
        return Data.Balance;
    }

    public static void UpdateBalance(int money)
    {
        Data.Balance += money;
        GameObject.Find("UI").transform.Find("Money").transform.Find("Text").gameObject.GetComponent<Text>().text = Data.Balance + "€";

        Transform MoneyHandler = GameObject.Find("UI").transform.Find("Money").Find("Money updates").Find("Viewport").Find("Content");

        GameObject uiElement = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Money " + (money < 0 ? "removed" : "added")), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        uiElement.GetComponent<Text>().text = (money < 0 ? "" : "+") + money + "€";
        uiElement.transform.SetParent(MoneyHandler, false);
        uiElement.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void RunSound(AudioClip clip)
    {
        AudioHandler.clip = clip;
        AudioHandler.Play(); 
    }

    public static void RunSoundStatic(AudioClip clip)
    {
        AudioHandler.clip = clip;
        AudioHandler.Play(); 
    }
}