using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
    public static Dictionary<string, string> Translations;

    void Start()
    {
        Translations = new Dictionary<string, string>();

        // Load translations from files depending on the Master.Language.

        Translations.Add("mainMenu_newGame", "NEW GAME");
        Translations.Add("mainMenu_loadGame", "LOAD GAME");
        Translations.Add("mainMenu_exit", "EXIT");
        Translations.Add("mainMenu_alphaDate", "Alpha {0} - {1}/{2}/{3}");
    }
}