using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class MainMenu : MonoBehaviour
{
    public Animator CreateGameController;
    public Animator LoadGameController;
    public FileInfo[] SavedGames;
    private int savedGamesIndex = 0;

    [Header("Player input")]
    public Text GameName;
    public Text PlayerName;

    [Header("Translations")]
    public Text NewGameText;
    public Text LoadGameText;
    public Text ExitText;
    public Text AlphaDateText;
    public Text CreateFarmText;
    public Text FarmNamePlaceholderText;
    public Text PlayerNamePlaceholderText;
    public Text NewGameLetterTextPart1;
    public Text NewGameLetterTextPart2;
    public Text NewGameLetterTextSign;
    public Text LoadFarmText;
    public Text LoadGameLetterTextPart1;
    public Text LoadGameLetterTextPart2;

    [Header("Extras")]
    public GameObject GameVersions;

    void Start()
    {
        Localization.LoadTranslations();
        NewGameText.text = Localization.Translations["mainMenu_newGame"];
        LoadGameText.text = Localization.Translations["mainMenu_loadGame"];
        ExitText.text = Localization.Translations["mainMenu_exit"];
        AlphaDateText.text = string.Format(Localization.Translations["mainMenu_alphaDate"], 8, 29, 10, 2020); // Update every version?
        CreateFarmText.text = Localization.Translations["mainMenu_createFarm"];
        FarmNamePlaceholderText.text = Localization.Translations["mainMenu_farmNamePlaceholder"];
        PlayerNamePlaceholderText.text = Localization.Translations["mainMenu_playerNamePlaceholder"];
        NewGameLetterTextPart1.text = Localization.Translations["mainMenu_newGameLetterTextPart1"];
        NewGameLetterTextPart2.text = Localization.Translations["mainMenu_newGameLetterTextPart2"];
        NewGameLetterTextSign.text = Localization.Translations["mainMenu_newGameLetterTextSign"];
        LoadFarmText.text = Localization.Translations["mainMenu_loadFarm"];
        LoadGameLetterTextPart1.text = Localization.Translations["mainMenu_loadGameLetterTextPart1"];
        LoadGameLetterTextPart2.text = Localization.Translations["mainMenu_loadGameLetterTextPart2"];
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

        DirectoryInfo saves = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Vivaio/Saves");
        SavedGames = saves.GetFiles();
        savedGamesIndex = 0;

        if (SavedGames.Length == 0)
        {
            Transform t = transform.Find("Load screen").Find("Start animation").Find("Loaded stuff");
            t.Find("Previous").gameObject.SetActive(false);
            t.Find("Next").gameObject.SetActive(false);
            t.Find("First part").gameObject.SetActive(false);
            t.Find("Second part").gameObject.SetActive(false);
            t.Find("Load").gameObject.SetActive(false);
            t.Find("Date").gameObject.SetActive(false);
            t.Find("Farm name").GetComponent<Text>().text = "No farms found";
        }
        else
        {
            transform.Find("Load screen").Find("Start animation").Find("Loaded stuff").Find("Farm name").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(SavedGames[savedGamesIndex].Name) + ",";
            transform.Find("Load screen").Find("Start animation").Find("Loaded stuff").Find("Date").GetComponent<Text>().text = SavedGames[savedGamesIndex].LastWriteTime.ToString();
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

    public void LoadGame()
    {
        string name = Path.GetFileNameWithoutExtension(SavedGames[savedGamesIndex].Name);
        Master.LoadingGame = true;
        Master.GameName = name;
        SceneManager.LoadScene("Game");
    }

    public void ChangeSaveGame(bool next)
    {
        if (next) // To the left
        {            
            LoadGameController.SetTrigger("Next");
            
            savedGamesIndex--;
            if (savedGamesIndex == -1)
                savedGamesIndex = SavedGames.Length - 1;

            transform.Find("Load screen").Find("Start animation").Find("Loaded stuff").Find("Farm name").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(SavedGames[savedGamesIndex].Name) + ",";
            transform.Find("Load screen").Find("Start animation").Find("Loaded stuff").Find("Date").GetComponent<Text>().text = SavedGames[savedGamesIndex].LastWriteTime.ToString();
        }
        else // To the right
        {
            LoadGameController.SetTrigger("Previous");

            savedGamesIndex++;
            if (savedGamesIndex == SavedGames.Length)
                savedGamesIndex = 0;

            transform.Find("Load screen").Find("Start animation").Find("Loaded stuff").Find("Farm name").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(SavedGames[savedGamesIndex].Name) + ",";
            transform.Find("Load screen").Find("Start animation").Find("Loaded stuff").Find("Date").GetComponent<Text>().text = SavedGames[savedGamesIndex].LastWriteTime.ToString();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}