using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    public static OptionsData Data;
    public static GameObject OptionsUI;

    // When loading a game
    public static bool Load(OptionsData data)
    {
        try
        {
            Data = data;   

            OptionsUI = GameObject.Find("UI").transform.Find("Options").gameObject;  
            SetOptions();

            foreach (ISlot slot in Data.TopBarButtons)
            {
                slot.Slot = GameObject.Find("UI").transform.Find("Top buttons bar").Find("Slots").Find(slot.SlotName).gameObject;
                slot.Initialize();
            }

            if (Data.TopBarCollapsed)
            {
                GameObject.Find("UI").transform.Find("Top buttons bar").Find("Buttons").GetComponent<HideShowButtons>().Collapse();
                GameObject.Find("UI").transform.Find("Top buttons bar").Find("Extended").gameObject.SetActive(false);
                GameObject.Find("UI").transform.Find("Top buttons bar").Find("Collapsed").gameObject.SetActive(true);
            }
        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "Options", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    { 
        Data = new OptionsData();
        Data.Width = Screen.currentResolution.width;
        Data.Height = Screen.currentResolution.height;
        Data.DataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Vivaio";
        Data.FPS = 60;  
        Data.MinuteValue = 1;
        Data.FullScreen = Screen.fullScreen;
        Data.MusicVolume = 0.25f;
        Data.SoundsVolume = 0.5f;
        Data.TopBarCollapsed = false;

        OptionsUI = GameObject.Find("UI").transform.Find("Options").gameObject;
        SetOptions();

        // Top bar buttons
        Data.TopBarButtons = new List<ISlot>();
        string[] buttons = new string[5] {"Options button", "Farmazon button", "Management button", "Tutorial button", "Shop button"}; 
        int buttonNumber = 0;
        foreach (Transform t in GameObject.Find("UI").transform.Find("Top buttons bar").Find("Slots"))
        {
            ISlot slot = new ISlot(buttons[buttonNumber], t.gameObject);
            buttonNumber++;
            Data.TopBarButtons.Add(slot);
            slot.Initialize();
        }

        return true;
    }

    public void CheckCloseOptions()
    {
        if (OptionsUI.activeSelf)
        {
            ShowOptions(false);
            Time.timeScale = 1;
        }
    }

    public void OpenOptions()
    {
        ShowOptions(true);
    }

    public static void ShowOptions(bool state)
    {
        OptionsUI.SetActive(state);
    }

    public static void SetOptions()
    {
        Application.targetFrameRate = Data.FPS;
        
        Screen.SetResolution(Data.Width, Data.Height, Data.FullScreen);
        OptionsUI.transform.Find("FullScreen").GetComponent<Toggle>().isOn = Data.FullScreen;

        GameObject.Find("Player").transform.Find("Camera").GetComponent<Camera>().orthographicSize = (float)Screen.currentResolution.height / 64 / 8;
        
        OptionsUI.transform.Find("Day duration").Find("Explanation").GetComponent<Text>().text = string.Format(Localization.Translations["options_current_day_duration"], Data.MinuteValue); 
        
        if(!Directory.Exists(Options.Data.DataPath + "/Saves/")) 
        {
            Directory.CreateDirectory(Options.Data.DataPath + "/Saves/");
            Directory.CreateDirectory(Options.Data.DataPath + "/Screenshots/");
        }

        OptionsUI.transform.Find("Volume").Find("Music volume").GetComponent<Slider>().value = Data.MusicVolume;
        OptionsUI.transform.Find("Volume").Find("Sounds volume").GetComponent<Slider>().value = Data.SoundsVolume;
    }

    public void ChangeMinuteValue(float value)
    {
        Data.MinuteValue = value;
        OptionsUI.transform.Find("Day duration").Find("Explanation").GetComponent<Text>().text = string.Format(Localization.Translations["options_current_day_duration"], Data.MinuteValue);
    } 

    public void ChangeFullScreen()
    {
        bool updateFS = !Screen.fullScreen;
        Screen.fullScreen = updateFS;
        Data.FullScreen = updateFS;
    }

    public void ChangeResolution()
    {
        int res = OptionsUI.transform.Find("Resolution").GetComponent<Dropdown>().value;

        if (res == 0) return;

        switch(res)
        {
            case 1:
                Data.Width = 800;
                Data.Height = 600;
                break;
            case 2:
                Data.Width = 1024;
                Data.Height = 768;
                break;
            case 3:
                Data.Width = 1280;
                Data.Height = 720;
                break;
            case 4:
                Data.Width = 1280;
                Data.Height = 800;
                break;
            case 5:
                Data.Width = 1280;
                Data.Height = 1024;
                break;
            case 6:
                Data.Width = 1366;
                Data.Height = 768;
                break;
            case 7:
                Data.Width = 1440;
                Data.Height = 900;
                break;
            case 8:
                Data.Width = 1600;
                Data.Height = 900;
                break;
            case 9:
                Data.Width = 1680;
                Data.Height = 1050;
                break;
            case 10:
                Data.Width = 1920;
                Data.Height = 1080;
                break;
            case 11:
                Data.Width = 1920;
                Data.Height = 1200;
                break;
            case 12:
                Data.Width = 2048;
                Data.Height = 1080;
                break;
            case 13:
                Data.Width = 3840;
                Data.Height = 2160;
                break;
        }
        Screen.SetResolution(Data.Width, Data.Height, Data.FullScreen);
    }

    public void ChangeMusicVolume()
    {
        Data.MusicVolume = OptionsUI.transform.Find("Volume").Find("Music volume").GetComponent<Slider>().value;
        GameObject.Find("Music handler").GetComponent<AudioSource>().volume = Data.MusicVolume;
    }

    public void ChangeSoundsVolume()
    {
        Data.SoundsVolume = OptionsUI.transform.Find("Volume").Find("Sounds volume").GetComponent<Slider>().value;
        GameObject.Find("Player").transform.Find("Camera").GetComponent<AudioSource>().volume = Data.SoundsVolume;
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main menu");
    }
}