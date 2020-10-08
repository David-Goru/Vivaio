using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
    public static Dictionary<string, string> Translations;

    public static void LoadTranslations()
    {
        Translations = new Dictionary<string, string>();

        if (PlayerPrefs.GetString("Language") != null)
            Master.Language = PlayerPrefs.GetString("Language");
        else
            PlayerPrefs.SetString("Language", "en_EN");

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
        Translations.Add("mainMenu_loadFarm", "LOAD FARM");
        Translations.Add("mainMenu_loadGameLetterTextPart1", "Dear farm, I have decided that is time to come back and work harder than ever. I'm sorry for abandoning you,");
        Translations.Add("mainMenu_loadGameLetterTextPart2", "but mistakes where made. I'm a human, you know... I just needed a break. But now, I'm stronger... Just let me help you. Again.");
        Translations.Add("Vanilla", "Vanilla");
        Translations.Add("VanillaUnlocked", "Vanilla Edition");
        Translations.Add("VanillaLocked", "Vanilla Edition");
        Translations.Add("Halloween", "Halloween");
        Translations.Add("HalloweenUnlocked", "Halloween Edition");
        Translations.Add("HalloweenLocked", "Click to buy");
    }
}