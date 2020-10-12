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

        // Main menu
        Translations.Add("mainMenu_newGame", "NEW GAME");
        Translations.Add("mainMenu_loadGame", "LOAD GAME");
        Translations.Add("mainMenu_exit", "EXIT");
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

        // General
        Translations.Add("Day", "Day {0}");

        // Vegetables
        Translations.Add("Sticks", "Sticks");
        Translations.Add("Carrot", "Carrot");
        Translations.Add("Potato", "Potato");
        Translations.Add("Tomato", "Tomato");
        Translations.Add("Pumpkin", "Pumpkin");
        Translations.Add("Onion", "Onion");
        Translations.Add("Lettuce", "Lettuce");
        Translations.Add("Wheat", "Wheat");

        // Food
        Translations.Add("Flour", "Flour");
        Translations.Add("BreadDough", "Bread dough");
        Translations.Add("Bread", "Bread");

        // Tools
        Translations.Add("Hoe", "Hoe");
        Translations.Add("Shovel", "Shovel");
        Translations.Add("WateringCan", "Watering can");
        Translations.Add("Basket", "Basket");

        // Objects
        Translations.Add("CarrotSeeds", "Carrot seeds");
        Translations.Add("PotatoSeeds", "Potato seeds");
        Translations.Add("TomatoSeeds", "Tomato seeds");
        Translations.Add("PumpkinSeeds", "Pumpkin seeds");
        Translations.Add("OnionSeeds", "Onion seeds");
        Translations.Add("LettuceSeeds", "Lettuce seeds");
        Translations.Add("WheatSeeds", "Wheat seeds");
        Translations.Add("GrassSeeds", "Grass seeds");
        Translations.Add("Sign", "Sign");
        Translations.Add("FenceGate", "Fence gate");
        Translations.Add("Fence", "Fence");
        Translations.Add("ShopBox", "Shop box");
        Translations.Add("ShopTable", "Shop table");
        Translations.Add("StorageBox", "Storage box");
        Translations.Add("ProductBox", "Product box");
        Translations.Add("RockTile", "Rock tile");
        Translations.Add("ConcreteTile", "Concrete tile");
        Translations.Add("CompositeTile", "Composite tile");
        Translations.Add("BrickTile", "Brick tile");
        Translations.Add("WoodTile", "Wood tile");
        Translations.Add("DripBottle", "Drip bottle");
        Translations.Add("DripIrrigationKit", "Drip irrigation kit");
        Translations.Add("Fertilizer", "Fertilizer");
        Translations.Add("Composter", "Composter");
        Translations.Add("SeedBox", "Seed box");
        Translations.Add("DeseedingMachine", "Deseeding machine");
        Translations.Add("FlourMachine", "Flour machine");
        Translations.Add("BreadMachine", "Bread machine");
        Translations.Add("Furnace", "Furnace");
        Translations.Add("WaterPump", "Water pump");
        Translations.Add("DeliveryBox", "Delivery box");
        Translations.Add("PresentBox", "Present box");
        Translations.Add("LetterOpen", "Open letter");
        Translations.Add("LetterClosed", "Closed letter");
        Translations.Add("HouseLamp", "House lamp");
        Translations.Add("CashRegister", "Cash register");
        Translations.Add("Bed", "Bed");
    }
}