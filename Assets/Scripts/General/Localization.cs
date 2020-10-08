﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
    public static Dictionary<string, string> Translations;

    public static void LoadTranslations()
    {
        Translations = new Dictionary<string, string>();

        // Load translations from files depending on the Master.Language.
        // Those are for en_EN (as a test):
        Translations.Add("mainMenu_newGame", "NEW GAME");
        Translations.Add("mainMenu_loadGame", "LOAD GAME");
        Translations.Add("mainMenu_exit", "EXIT");
        Translations.Add("mainMenu_alphaDate", "Alpha {0} - {1}/{2}/{3}");
        Translations.Add("mainMenu_createFarm", "CREATE FARM");
        Translations.Add("mainMenu_farmNamePlaceholder", "Farm name");
        Translations.Add("mainMenu_playerNamePlaceholder", "Player name");
        Translations.Add("mainMenu_newGameLetterTextPart1", "New farm acquisition.\n\nMade and executed by the State Department of Farms and Plants, from now on the SDFP.\n\nThe name chosen to represent this farm, from now on the farm:");
        Translations.Add("mainMenu_newGameLetterTextPart2", "To this day, and with the power of the SDFP, this farm is an entity by itself, managed entirely by the new owner, whose name is:");
        Translations.Add("mainMenu_newGameLetterTextSign", "Signed,\nthe SDFP and the farm's owner");
    }
}