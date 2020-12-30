using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class MainMenu : MonoBehaviour
{
    public AudioSource AudioHandler;
    public Animator CreateGameController;
    public Animator LoadGameController;
    public FileInfo[] SavedGames;
    private int savedGamesIndex = 0;
    public GameObject InitialScreenGameType;
    public GameObject LoadScreenGameType;
    public GameObject NewScreenGameType;

    [Header("Player input")]
    public Text GameName;
    public Text PlayerName;

    [Header("Translations")]
    public Text FarmNamePlaceholderText;
    public Text PlayerNamePlaceholderText;
    public Text NewGameLetterTextPart1;
    public Text NewGameLetterTextPart2;
    public Text NewGameLetterTextSign;
    public Text LoadGameLetterTextPart1;
    public Text LoadGameLetterTextPart2;

    [Header("Vanilla translations")]
    public Text VanillaText;
    public Text VanillaUnlockedText;
    public Text VanillaLockedText;
    public Text NewGameText;
    public Text LoadGameText;
    public Text ExitText;
    public Text AlphaDateText;
    public Text CreateFarmText;
    public Text LoadFarmText;

    [Header("Halloween translations")]
    public Text HalloweenText;
    public Text HalloweenUnlockedText;
    public Text HalloweenLockedText;
    public Text HWNewGameText;
    public Text HWLoadGameText;
    public Text HWExitText;
    public Text HWAlphaDateText;
    public Text HWCreateFarmText;
    public Text HWLoadFarmText;

    [Header("Christmas translations")]
    public Text ChristmasText;
    public Text ChristmasUnlockedText;
    public Text ChristmasLockedText;
    public Text CHNewGameText;
    public Text CHLoadGameText;
    public Text CHExitText;
    public Text CHAlphaDateText;
    public Text CHCreateFarmText;
    public Text CHLoadFarmText;

    [Header("Extras")]
    public GameObject GameVersions;

    void Start()
    {
        Master.GameVersion = GameObject.Find("Version Handler").GetComponent<VersionHandler>().GameVersion;
        Master.VersionDate = GameObject.Find("Version Handler").GetComponent<VersionHandler>().VersionDate;
        Master.SpecialEdition = GameObject.Find("Version Handler").GetComponent<VersionHandler>().SpecialEdition;

        // Special Edition
        if (PlayerPrefs.GetString("Edition", "None") != "None")
        {
            Master.GameEdition = PlayerPrefs.GetString("Edition");
            
            InitialScreenGameType.transform.Find("Vanilla").gameObject.SetActive(false);
            LoadScreenGameType.transform.Find("Vanilla").gameObject.SetActive(false);
            NewScreenGameType.transform.Find("Vanilla").gameObject.SetActive(false);
            InitialScreenGameType.transform.Find(Master.GameEdition).gameObject.SetActive(true);
            LoadScreenGameType.transform.Find(Master.GameEdition).gameObject.SetActive(true);
            NewScreenGameType.transform.Find(Master.GameEdition).gameObject.SetActive(true);
        }

        if (Master.SpecialEdition)
        {
            transform.Find("Initial screen").Find("Special Edition").Find("Unlocked").gameObject.SetActive(true);
            transform.Find("Initial screen").Find("Special Edition").Find("Locked").gameObject.SetActive(false);
        }

        // Localization
        Localization.LoadTranslations();

        // Vanilla
        VanillaText.text = Localization.Translations["Vanilla"].ToUpper();
        VanillaUnlockedText.text = Localization.Translations["VanillaUnlocked"];
        VanillaLockedText.text = Localization.Translations["VanillaLocked"];
        NewGameText.text = Localization.Translations["mainMenu_newGame"];
        LoadGameText.text = Localization.Translations["mainMenu_loadGame"];
        ExitText.text = Localization.Translations["mainMenu_exit"];
        AlphaDateText.text = string.Format(Master.GameVersion + " - " + Master.VersionDate);
        CreateFarmText.text = Localization.Translations["mainMenu_createFarm"];
        LoadFarmText.text = Localization.Translations["mainMenu_loadFarm"];

        // Halloween
        HalloweenText.text = Localization.Translations["Halloween"].ToUpper();
        HalloweenUnlockedText.text = Localization.Translations["HalloweenUnlocked"];
        HalloweenLockedText.text = Localization.Translations["HalloweenLocked"];
        HWNewGameText.text = Localization.Translations["mainMenu_newGame"];
        HWLoadGameText.text = Localization.Translations["mainMenu_loadGame"];
        HWExitText.text = Localization.Translations["mainMenu_exit"];
        HWAlphaDateText.text = string.Format(Master.GameVersion + " - " + Master.VersionDate);
        HWCreateFarmText.text = Localization.Translations["mainMenu_createFarm"];
        HWLoadFarmText.text = Localization.Translations["mainMenu_loadFarm"];

        // Christmas
        ChristmasText.text = Localization.Translations["Christmas"].ToUpper();
        ChristmasUnlockedText.text = Localization.Translations["ChristmasUnlocked"];
        ChristmasLockedText.text = Localization.Translations["ChristmasLocked"];
        CHNewGameText.text = Localization.Translations["mainMenu_newGame"];
        CHLoadGameText.text = Localization.Translations["mainMenu_loadGame"];
        CHExitText.text = Localization.Translations["mainMenu_exit"];
        CHAlphaDateText.text = string.Format(Master.GameVersion + " - " + Master.VersionDate);
        CHCreateFarmText.text = Localization.Translations["mainMenu_createFarm"];
        CHLoadFarmText.text = Localization.Translations["mainMenu_loadFarm"];

        // General
        FarmNamePlaceholderText.text = Localization.Translations["mainMenu_farmNamePlaceholder"];
        PlayerNamePlaceholderText.text = Localization.Translations["mainMenu_playerNamePlaceholder"];
        NewGameLetterTextPart1.text = Localization.Translations["mainMenu_newGameLetterTextPart1"];
        NewGameLetterTextPart2.text = Localization.Translations["mainMenu_newGameLetterTextPart2"];
        NewGameLetterTextSign.text = Localization.Translations["mainMenu_newGameLetterTextSign"];
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
            t.Find("Game type").Find(Master.GameEdition).Find("Load").gameObject.SetActive(false);
            t.Find("Date").gameObject.SetActive(false);
            t.Find("Farm name").GetComponent<Text>().text = Localization.Translations["mainMenu_loadGame_no_farms"];
        }
        else
        {
            Transform t = transform.Find("Load screen").Find("Start animation").Find("Loaded stuff");
            t.Find("Previous").gameObject.SetActive(true);
            t.Find("Next").gameObject.SetActive(true);
            t.Find("First part").gameObject.SetActive(true);
            t.Find("Second part").gameObject.SetActive(true);
            t.Find("Game type").Find(Master.GameEdition).Find("Load").gameObject.SetActive(true);
            t.Find("Date").gameObject.SetActive(true);
            t.Find("Farm name").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(SavedGames[savedGamesIndex].Name) + ",";
            t.Find("Date").GetComponent<Text>().text = SavedGames[savedGamesIndex].LastWriteTime.ToString();
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
        LoadGameController.SetTrigger("Load");
    }

    public void LoadGameNow()
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

    public void BuyGame()
    {
        Application.OpenURL("https://vivaio.itch.io/game/purchase");
    }

    public void ChangeEdition(string edition)
    {
        InitialScreenGameType.transform.Find(Master.GameEdition).gameObject.SetActive(false);
        LoadScreenGameType.transform.Find(Master.GameEdition).gameObject.SetActive(false);
        NewScreenGameType.transform.Find(Master.GameEdition).gameObject.SetActive(false);
        InitialScreenGameType.transform.Find(edition).gameObject.SetActive(true);
        LoadScreenGameType.transform.Find(edition).gameObject.SetActive(true);
        NewScreenGameType.transform.Find(edition).gameObject.SetActive(true);

        PlayerPrefs.SetString("Edition", edition);
        Master.GameEdition = edition;
    }

    public void RunSound(AudioClip clip)
    {
        AudioHandler.clip = clip;
        AudioHandler.Play(); 
    }

    public void Exit()
    {
        Application.Quit();
    }
}