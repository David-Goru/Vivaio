using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public Animator CreateGameController;
    public Text GameName;
    public Text PlayerName;

    // Texts
    public Text NewGameText;
    public Text LoadGameText;
    public Text ExitText;
    public Text AlphaDateText;

    void Start()
    {
        NewGameText.text = Localization.Translations["mainMenu_newGame"];
        LoadGameText.text = Localization.Translations["mainMenu_loadGame"];
        ExitText.text = Localization.Translations["mainMenu_exit"];
        AlphaDateText.text = string.Format(Localization.Translations["mainMenu_alphaDate"], 8, 29, 10, 2020); // Update every version?
    }

    public void New()
    {
        transform.Find("Initial screen").gameObject.SetActive(false);
        transform.Find("Create screen").gameObject.SetActive(true);
    }

    public void Load()
    {
        transform.Find("Initial screen").gameObject.SetActive(false);
        transform.Find("Load screen").gameObject.SetActive(true);

        Transform content = transform.Find("Load screen").Find("Games").Find("Viewport").Find("Content");

        foreach(Transform t in content)
        {
            Destroy(t.gameObject);
        }

        DirectoryInfo saves = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Vivaio/Saves");
        foreach (FileInfo f in saves.GetFiles())
        {
            GameObject b = Instantiate(Resources.Load<GameObject>("Game button"), transform.position, transform.rotation);
            b.transform.Find("Text").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(f.Name);
            b.GetComponent<Button>().onClick.AddListener(() => LoadGame(Path.GetFileNameWithoutExtension(f.Name)));
            b.transform.SetParent(content);
        }
    }

    public void Back()
    {
        transform.Find("Load screen").gameObject.SetActive(false);
        transform.Find("Create screen").gameObject.SetActive(false);
        transform.Find("Initial screen").gameObject.SetActive(true);
    }

    public void CreateGame()
    {
        if (GameName.text != "" && PlayerName.text != "")
        {
            Master.LoadingGame = false;
            Master.GameName = GameName.text;
            Master.PlayerName = PlayerName.text;

            CreateGameController.SetTrigger("StartGame");
        }
    }

    public void LoadGame(string name)
    {
        Master.LoadingGame = true;
        Master.GameName = name;
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}