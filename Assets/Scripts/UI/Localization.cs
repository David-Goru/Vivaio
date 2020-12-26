using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Translations.Add("mainMenu_loadGame_no_farms", "No farms found");
        Translations.Add("Vanilla", "Vanilla");
        Translations.Add("VanillaUnlocked", "Vanilla Edition");
        Translations.Add("VanillaLocked", "Vanilla Edition");
        Translations.Add("Halloween", "Halloween");
        Translations.Add("HalloweenUnlocked", "Halloween Edition");
        Translations.Add("HalloweenLocked", "Click to buy");

        // Loading screen
        Translations.Add("failed_to_load_game", "Oups! We failed to load your game... Sorry!");
        Translations.Add("load_error_text_1", "We work hard to avoid this, but it can happen. You can report the error just pressing the Report error button and we'll try to fix it for the next version. If you want, you can also send us a DM at @VivaioGame (twitter) or an email at vivaio.gamedev@gmail.com and we'll help you.\n\n");
        Translations.Add("load_error_text_2", "Version {0} \n");
        Translations.Add("load_error_text_3", "We couldn't load the game due to the following error(s):\n");
        Translations.Add("back_to_main_menu_button", "Back to main menu");
        Translations.Add("report_error_button", "Report error");
        Translations.Add("error_reported_text", "Error reported, thank you!");

        // General UI
        Translations.Add("Day", "Day {0}");
        Translations.Add("Empty", "Empty");
        Translations.Add("throw_object_button_tooltip", "Throw object");
        Translations.Add("build_object_button_tooltip", "Build object");
        Translations.Add("cancel_build_button_tooltip", "Cancel");
        Translations.Add("open_letter_button_tooltip", "Open letter");
        Translations.Add("close_window_button_tooltip", "Close");
        Translations.Add("bed_text", "Comfy sheets await you after a hard farm day...");
        Translations.Add("bed_button", "Sleep");
        Translations.Add("cash_log_total_amount", "Total amount: {0}€");
        Translations.Add("cash_register_title", "Cash register");
        Translations.Add("cash_register_take_button_tooltip", "Take cash register");
        Translations.Add("storage_box_title", "Storage");
        Translations.Add("storage_take_button_tooltip", "Take storage");
        Translations.Add("garbage_can_title", "Trash can");
        Translations.Add("garbage_can_throw_object_button", "Throw object");
        Translations.Add("garbage_can_take_button_tooltip", "Take garbage can");

        // Top bar buttons
        Translations.Add("options_tooltip", "Options");
        Translations.Add("farmazon_tooltip", "Farmazon");
        Translations.Add("management_tooltip", "Management");
        Translations.Add("tutorial_tooltip", "Tutorial");
        Translations.Add("shop_tooltip", "Shop");

        // Options UI
        Translations.Add("options_title", "Options");
        Translations.Add("options_set_day_duration", "Set day speed");
        Translations.Add("options_current_day_duration", "Current: {0} (1 in-game minute lasts {0} real seconds)");
        Translations.Add("options_fullscreen", "Fullscreen");
        Translations.Add("options_change_res", "Change resolution");
        Translations.Add("options_choose_res", "Choose resolution");
        Translations.Add("options_volume", "Volume");
        Translations.Add("options_volume_music", "Music");
        Translations.Add("options_volume_sounds", "Sounds");
        Translations.Add("options_back_main_menu", "Back to main menu");
        Translations.Add("options_back_main_menu_conf", "Leave");
        Translations.Add("options_leave_game", "Leave game");
        Translations.Add("options_leave_game_conf", "Leave");
        Translations.Add("options_unsaved_data_warning", "Unsaved data will be lost");
        Translations.Add("options_are_you_sure_warning", "Are you sure?");

        // Management UI
        Translations.Add("management_title", "Management");
        Translations.Add("management_remaining_debt", "Remaining debt: {0}€.");
        Translations.Add("management_pay_debt", "Pay debt now");
        Translations.Add("management_pay_debt_warning", "-{0}€");
        Translations.Add("management_debt_paid", "Debt paid!");
        Translations.Add("management_expansion_text", "You can make your farm 5 blocks bigger for {0}€.");
        Translations.Add("management_expand", "Expand");
        Translations.Add("management_expand_warning", "New debt: {0}€");
        Translations.Add("management_expand_announcement", "Your farm will be expanded 5 blocks tomorrow.");

        // Farmazon UI
        Translations.Add("farmazon_title", "Farmazon");
        Translations.Add("farmazon_select_item", "Select an item");
        Translations.Add("farmazon_show_cart", "Show cart");
        Translations.Add("farmazon_total_price", "Total price");
        Translations.Add("farmazon_terms_and_conditions", "By buying, you agree to Farmazon's Conditions of Use and Privacy Notice.\nTaxes are calculated depending on your location.\nFarmazon does not support refunds. If you find any problems with the item(s) bought, just buy them again!");
        Translations.Add("farmazon_buy", "Buy");
        Translations.Add("farmazon_add_to_cart", "Add to cart");
        Translations.Add("farmazon_back_to_shop", "Back to shop");

        // Shop UI
        Translations.Add("shop_title", "Shop");
        Translations.Add("shop_inauguration_text", "Once the shop is inaugurated, customers will start buying from your shop. Be careful: inaugurating a shop without products to sell will make customers angry.");
        Translations.Add("shop_inauguration_button_text", "Inaugurate");
        Translations.Add("shop_earnings_yesterday", "Yesterday earnings:");
        Translations.Add("shop_earnings_yesterday_value", "{0}€");
        Translations.Add("shop_earnings_last_7_days", "Last 7 days earnings:");
        Translations.Add("shop_earnings_last_7_days_value", "{0}€ ({1}€/day)");
        Translations.Add("shop_earnings_last_14_days", "Last 14 days earnings:");
        Translations.Add("shop_earnings_last_14_days_value", "{0}€ ({1}€/day)");
        Translations.Add("shop_earnings_no_data", "No data");
        Translations.Add("shop_product_stock", "{0}: {1}/{2}");

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
        Translations.Add("SmallVegetablesStand", "Small vegetables stand");
        Translations.Add("BigVegetablesStand", "Big vegetables stand");
        Translations.Add("BeveragesStand", "Beverages stand");
        Translations.Add("FoodStand", "Food stand");
        Translations.Add("StorageBox", "Storage box");
        Translations.Add("ProductBox", "Product box");
        Translations.Add("RockTile", "Rock tile");
        Translations.Add("ConcreteTile", "Concrete tile");
        Translations.Add("CompositeTile", "Composite tile");
        Translations.Add("BrickTile", "Brick tile");
        Translations.Add("WoodTile", "Wood tile");
        Translations.Add("DripBottle", "Drip bottle");
        Translations.Add("DripIrrigationKit", "Drip irrigation kit");
        Translations.Add("WaterBottlingMachine", "Water bottling machine");
        Translations.Add("BottlesRecycler", "Bottles recycler");
        Translations.Add("Fertilizer", "Fertilizer");
        Translations.Add("fertilizer_title", "Fertilizer");
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
        Translations.Add("GarbageCan", "Garbage can");
        Translations.Add("WaterBottle", "Water bottle");
        Translations.Add("GlassBottle", "Glass bottle");
        Translations.Add("House", "House");

        // Letters
        Translations.Add("grandma_signature", "Grandma");
        Translations.Add("grandma_first_letter_title", "I'm glad");
        Translations.Add("grandma_first_letter_body", "Dear grandson, \n\n        I'm glad you've finally decided to take this opportunity. Now, you're all on your own, and will have the power to build the future you've ever wanted.\n\n        I want you to make your best, so here is my bit: carrot seeds. Treat them well, and make them glad of their children, just as you make me glad. Good luck!");
        Translations.Add("SDFP_signature", "SDFP");
        Translations.Add("last_day_expenses_title", "Last day expenses");
        Translations.Add("last_day_expenses_letter_body_start", "From the <b>State Department of Farms and Plants</b> we inform you that the day {0} expenses are as follows:\n        Energy usage: -{1}€\n        Water usage: -{2}€\n");
        Translations.Add("last_day_expenses_letter_body_debt", "        Debt: -{0}€\n        ------------------\n        Total: -{1}€\n        Remaining debt: {2}€");
        Translations.Add("last_day_expenses_letter_body_nodebt", "        ------------------\n        Total: -{0}€");
        Translations.Add("debt_paid_title", "Debt paid");
        Translations.Add("debt_paid_letter_body", "Hello sir,\n        from the <b>State Department of Farms and Plants</b>, we thank you for paying properly the last debt, and so, we inform you that you are able to request a new upgrade for your farm.");
        Translations.Add("debt_information_title", "Debt information");
        Translations.Add("debt_information_letter_body", "Hello sir,\n        from the <b>State Department of Farms and Plants</b>, we inform you that a new debt has been created in your farm, due to:\n       {0}\nThe total debt ascends to <b>{1}€</b>. The debt will be paid in amounts of <b>{2}€</b> every day if possible. You won’t be able to request a new upgrade until the current debt is completely paid.");
        Translations.Add("debt_information_topic_new_farm", "New farm bought");
        Translations.Add("debt_information_topic_farm_expansion", "Farm expansion");
        Translations.Add("happy_customer_signature", "Your happy customer, {0}");
        Translations.Add("happy_customer_title", "Happy customer");
        Translations.Add("happy_customer_letter_body", "Hello,\n        I am {0}, one of your closest customers. I would like to tell you that your shop is just… amazing. You have had me coming over and over, and always had what I needed. Please, keep doing what you do. I would love to see how far your shop goes.");
        Translations.Add("unhappy_customer_signature", "Your unhappy customer, {0}");
        Translations.Add("unhappy_customer_title", "Unhappy customer");
        Translations.Add("unhappy_customer_letter_body", "Hello,\n        I am {0}, and I am writing this as a warning. After coming to your shop several days, I have realized that your shop is not supplying your customers with the right products. I would love to see you improving and learning from your mistakes. Please, focus on what you do. Ask for help if you need, there are a lot of farmers that would love to teach you how to build a good shop. Do not hate me, this is a constructive critic. And, as so, please, take into consideration what I have said. Success comes from knowing that you did your best to become the best that you are capable of becoming.");
    
        // Machines and others
        Translations.Add("composter_title", "Composter");
        Translations.Add("composter_take_button_tooltip", "Take composter");
        Translations.Add("composter_add_compost_button", "Add compost");
        Translations.Add("composter_take_fertilizer_button", "Take fertilizer");
        Translations.Add("composter_working_text", "Producing compost.\nTime remaining: {0:D2} hours {1:D2} minutes");

        Translations.Add("product_box_title", "Product box");
        Translations.Add("product_box_take_button_tooltip", "Take product box");
        Translations.Add("product_box_add_product_button", "Add product");
        Translations.Add("product_box_take_product_button", "Take product");
        
        Translations.Add("food_stand_title", "Food stand");
        Translations.Add("beverages_stand_title", "Beverages stand");
        Translations.Add("vegetables_stand_title", "Vegetables stand");
        Translations.Add("stand_take_button_tooltip", "Take stand");
        Translations.Add("stand_add_product_button", "Add product");
        Translations.Add("stand_take_product_button", "Take product");
        Translations.Add("stand_disable", "Disable stand");
        Translations.Add("stand_enable", "Enable stand");
        Translations.Add("stand_recommended_price", "Recommended price: {0}");
        Translations.Add("stand_price", "Price:");
        
        Translations.Add("seed_box_title", "Seed box");
        Translations.Add("seed_box_take_button_tooltip", "Take seed box");
        
        Translations.Add("deseeding_machine_title", "Deseeding machine");
        Translations.Add("deseeding_machine_working", "Producing {0} seeds.\nTime remaining: {1:D2} hours {2:D2} minutes");
        Translations.Add("deseeding_machine_take_button_tooltip", "Take deseeding machine");
        Translations.Add("deseeding_machine_add_product_button", "Add product to deseed");
        Translations.Add("deseeding_machine_take_seeds_button", "Take seeds");
        Translations.Add("deseeding_machine_take_compost_button", "Take compost");
        
        Translations.Add("flour_machine_title", "Flour machine");
        Translations.Add("flour_machine_working", "Producing flour.\nTime remaining: {0:D2} minutes");
        Translations.Add("flour_machine_take_button_tooltip", "Take flour machine");
        Translations.Add("flour_machine_add_wheat_button", "Add wheat");
        Translations.Add("flour_machine_take_compost_button", "Take compost");
        Translations.Add("flour_machine_take_flour_button", "Take flour");
        
        Translations.Add("bread_machine_title", "Bread machine");
        Translations.Add("bread_machine_working", "Producing bread dough.\nTime remaining: {0:D2} minutes");
        Translations.Add("bread_machine_take_button_tooltip", "Take bread machine");
        Translations.Add("bread_machine_add_flour_button", "Add flour");
        Translations.Add("bread_machine_add_water_button", "Add water");
        Translations.Add("bread_machine_take_bread_dough_button", "Take bread dough");

        Translations.Add("water_bottling_machine_title", "Water bottling machine");
        Translations.Add("water_bottling_machine_working", "Filling a bottle with water.\nTime remaining: {0:D2} minutes");
        Translations.Add("water_bottling_machine_take_button_tooltip", "Take water bottling machine");
        Translations.Add("water_bottling_machine_add_bottle_button", "Add bottle");
        Translations.Add("water_bottling_machine_take_water_bottle_button", "Take water bottle");
        
        Translations.Add("sign_title", "Sign");
        Translations.Add("sign_take_button_tooltip", "Take sign");
        Translations.Add("sign_selected_icon", "Selected icon");
        
        Translations.Add("furnace_title", "Furnace");
        Translations.Add("furnace_working", "Producing {0}.\nTime remaining: {1:D2} minutes");
        Translations.Add("furnace_take_button_tooltip", "Take furnace");
        Translations.Add("furnace_add_product_button", "Add product to bake");
        Translations.Add("furnace_turn_on_button", "Turn on");
        Translations.Add("furnace_take_product_button", "Take product");

        Translations.Add("bottles_recycler_title", "Bottles recycler");
        Translations.Add("bottles_recycler_take_button_tooltip", "Take bottles recycler");
        Translations.Add("bottles_recycler_take_bottles_button", "Take bottles");

        // Tutorial
        Translations.Add("tutorial_chapter_1_name", "Introduction");
        Translations.Add("tutorial_chapter_2_name", "A day at the farm");
        Translations.Add("tutorial_chapter_3_name", "How to use Farmazon");
        Translations.Add("tutorial_chapter_4_name", "How to use the tools");
        Translations.Add("tutorial_chapter_5_name", "Getting started with the crops");
        Translations.Add("tutorial_chapter_6_name", "How to build a shop");
        Translations.Add("tutorial_chapter_7_name", "End");

        Translations.Add("tutorial_chapter_1_title", "INTRODUCTION");
        Translations.Add("tutorial_chapter_1_text", "Hello fellow farmer! Thank you for purchasing the first version of 'Life at the farm'. With this guide you'll learn the basics of running a farm and how to sell the crops produced.");
        
        Translations.Add("tutorial_chapter_2_title", "A DAY AT THE FARM");
        Translations.Add("tutorial_chapter_2_text_1", "Mornings start at 6AM. As a farmer, you should know that you have the power to control your life.");
        Translations.Add("tutorial_chapter_2_text_2", "That means that nights end when you go to sleep. Even if you want to go to sleep soon, you can.");
        Translations.Add("tutorial_chapter_2_text_3", "But you must know that once you touch the bed, a new day will come no matter what. To sleep, just find a bed - obvious, right? - and get close to it. You'll know what to do next.");
        Translations.Add("tutorial_chapter_2_text_4", "Every day, your mailbox may have new letters for you. Remember to check your expenses, which can include your farm debt - if any.");
        Translations.Add("tutorial_chapter_2_text_5", "The SDFP won't charge you for your farm debt if you don't have enough money to guarantee your subsistence, so don't worry.");
        Translations.Add("tutorial_chapter_2_text_6", "At any time, you may want to build. Remember: you're strong enough to carry one - and only one - object at your hands. If it can be built, just press the magical B button or the build icon which will appear close to the object in the inventory. Once on build mode, choose a place to build it and just click it. Green means that it can be built, red means that it can't. Easy. Right click at a placed object and it will let you move it.");

        Translations.Add("tutorial_chapter_3_title", "HOW TO USE FARMAZON");
        Translations.Add("tutorial_chapter_3_text_1", "Farmazon is the world's biggest online shop for farmers. It has all the things you could dream of.");
        Translations.Add("tutorial_chapter_3_text_2", "Open your Farmazon app and buy whatever you want (if you have enough money, of course!).");
        Translations.Add("tutorial_chapter_3_text_3", "After making your to-buy list, go to 'Show cart' and check that everything is correct. If so, press the 'Buy' button and... Woosh! You have completed your purchase. Now wait until you receive your products.");
        Translations.Add("tutorial_chapter_3_text_4", "Farmazon deliverers can bring you up to 4 boxes of items every day, with 4 items inside each box. This means that if you buy more than 16 items (that's a lot!), you'll need more than one deliver.");
        Translations.Add("tutorial_chapter_3_text_5", "Farmazon boxes are made of a highly biodegradable material. This means that once you have taken everything from the box, it will disappear. Awesome!");
        Translations.Add("tutorial_chapter_3_text_6", "To end the Farmazon chapter, I would like to emphasize that you have to remember, always, to confirm your purchase. If you don't, you won't receive anything at all. That's how Farmazon works.");

        Translations.Add("tutorial_chapter_4_title", "HOW TO USE THE TOOLS");
        Translations.Add("tutorial_chapter_4_text_1", "Tools! The key of the farm. Without them, you would be hopeless.\nEvery farmer should have available four tools: a hoe, a shovel, a watering can and a basket.\nLet's start explaining one by one why they are so important.");
        Translations.Add("tutorial_chapter_4_text_2", "The hoe");
        Translations.Add("tutorial_chapter_4_text_3", "The tool that will let you start working on the farm. Take it with your hands, choose a spot of grass and click it! Is the way the land is plowed. Pretty easy.");
        Translations.Add("tutorial_chapter_4_text_4", "The shovel");
        Translations.Add("tutorial_chapter_4_text_5", "This is the tool for those who regret doing something like planting or building floor. Click the thing you want to remove (remember: plant or floor!) and it will disappear.");
        Translations.Add("tutorial_chapter_4_text_6", "The watering can");
        Translations.Add("tutorial_chapter_4_text_7", "This is also called the happiness source for your plants. They love it! It is used to water your plants or fill the drip bottles. Click at the plant or drip bottle with the watering can filled with water and watch sad life becoming happy once again. A common watering can will have up to 10 units of water of space.");
        Translations.Add("tutorial_chapter_4_text_8", "Every plant needs 1 unit of water, and a drip bottle can store up to 5 of them. You can fill the watering can at any water source. They should be near the tools stand! Just get close and click it.");
        Translations.Add("tutorial_chapter_4_text_9", "By the way, drip bottles will make watering faster and cheaper. They are available at Farmazon!");
        Translations.Add("tutorial_chapter_4_text_10", "The basket");
        Translations.Add("tutorial_chapter_4_text_11", "Every good farmer needs a basket to harvest the products of the hard work. A common basket will store up to 20 products. To harvest the plants, just click on them with the basket in your hand once they are mature.");
        Translations.Add("tutorial_chapter_4_text_12", "After harvesting, you can store the products in a product box (Farmazon!) or place them on a vegetables stand.");

        Translations.Add("tutorial_chapter_5_title", "GETTING STARTED WITH THE CROPS");
        Translations.Add("tutorial_chapter_5_text_1", "As a farmer, you'll have the power to plant whatever you want. If the seeds are available at Farmazon, of course.");
        Translations.Add("tutorial_chapter_5_text_2", "With seeds on hand and plowed soil, click the soil to start planting. Every plant is unique. Some plants will need 3 days to get mature, some will need 7. Some will produce 1 product, some will produce a lot more. A bit random, I know... Let nature be nature.");
        Translations.Add("tutorial_chapter_5_text_3", "A good farmer will make sure that every plant is watered at the end of the day. Even though some plants last longer than others, all of them will get dry if they don't get enough water. And we don't want that! So... water them!");
        Translations.Add("tutorial_chapter_5_text_4", "Every unit of water costs 1€, and the SDFP will still let you use water sources even if you don't have money to pay for them. Another important thing is that if you forget to harvest a plant for several days, it will also dry.");
        Translations.Add("tutorial_chapter_5_text_5", "There's a way to get more products from a single plant: fertilizer! It can be bought from Farmazon (obviously) or can be produced using your own composter (Farmazon, too...). Every composter will need 10 units of dried sticks to produce 5 units of fertilizer. To use the fertilizer, just put it on your hand and click a plowed soil without plants.");
        Translations.Add("tutorial_chapter_5_text_6", "One last thing about seeds: Farmazon (heh) has grass seeds available (they're pretty cheap). Using them is the only way to grow grass back on a plowed soil.");

        Translations.Add("tutorial_chapter_6_title", "HOW TO BUILD A SHOP");
        Translations.Add("tutorial_chapter_6_text_1", "If you want to keep your farm, you'll need to earn money. And, the only way to get money as a farmer, is selling your products. This is like... The most obvious thing on this book.");
        Translations.Add("tutorial_chapter_6_text_2", "To sell your products, all you need is a stand. One for each type of product. A small vegetables stand can keep up to 10 vegetables, and a big one can keep up to 50.");
        Translations.Add("tutorial_chapter_6_text_3", "There's an option to change the price of the product at one stand. If the price is too high, customers won't like it. But, if the price is too  low...");
        Translations.Add("tutorial_chapter_6_text_4", "Every customer is different, and they'll only come one time per day if they want to. The more happy they're, the more options there are that the next day they'll come.");
        Translations.Add("tutorial_chapter_6_text_5", "Every shop has the same schedule: it will open at 9AM and close at 9PM. It will remain open even if you are sleeping, what means that customers will be buying at your shop during that time.");

        Translations.Add("tutorial_chapter_7_title", "END");
        Translations.Add("tutorial_chapter_7_text_1", "That's everything I can explain you as a former farmer (heh). At least, that's everything... In this book. I hope you have learnt the things I would love to have learnt when I started in this world.");
        Translations.Add("tutorial_chapter_7_text_2", "See you in my next book:\n'Getting specialized with the plants'\n\n:)");
    }

    public static void UpdateTexts()
    {
        foreach(GameObject g in UI.LocalizableTexts)
        {
            try
            {
                g.GetComponent<Text>().text = Translations[g.name];
            }
            catch(System.Exception e)
            {
                Master.AddLog("Error loading translation for " + g.name + " (Localization): " + e);
            }
        }

        /*FarmazonSelectItem.text = Translations["farmazon_select_item"];
        FarmazonShowCart.text = Translations["farmazon_show_cart"];
        FarmazonTotalPrice.text = Translations["farmazon_total_price"];
        FarmazonTermsAndConditions.text = Translations["farmazon_terms_and_conditions"];
        FarmazonBuy.text = Translations["farmazon_buy"];
        FarmazonAddToCart.text = Translations["farmazon_add_to_cart"];
        FarmazonBackToShop.text = Translations["farmazon_back_to_shop"];*/
    }
}