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

        // General UI
        Translations.Add("Day", "Day {0}");
        Translations.Add("Empty", "Empty");
        Translations.Add("throw_object_button", "Throw object");
        Translations.Add("build_button", "Build object");
        Translations.Add("cancel_build_button", "Cancel");
        Translations.Add("open_letter_button", "Open letter");
        Translations.Add("bed_text", "Comfy sheets await you after a hard farm day...");
        Translations.Add("bed_button", "Sleep");
        Translations.Add("cash_log_total_amount", "Total amount: {0}€");
        Translations.Add("cash_register_log_title", "Cash register log");
        Translations.Add("storage_title", "Storage");
        Translations.Add("take_storage_button", "Take storage");
        Translations.Add("garbage_can_title", "Trash can");
        Translations.Add("garbage_can_throw_object_button", "Throw object");

        // Options UI
        Translations.Add("Options", "Options");
        Translations.Add("options_set_day_duration", "Set day duration");
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
        Translations.Add("Management", "Management");
        Translations.Add("management_remaining_debt", "Remaining debt: {0}€.");
        Translations.Add("management_pay_debt", "Pay debt now");
        Translations.Add("management_pay_debt_warning", "-{0}€");
        Translations.Add("management_debt_paid", "Debt paid!");
        Translations.Add("management_expansion_text", "You can make your farm 5 blocks bigger for {0}€.");
        Translations.Add("management_expand", "Expand");
        Translations.Add("management_expand_warning", "New debt: {0}€");
        Translations.Add("management_expand_announcement", "Your farm will be expanded 5 blocks tomorrow.");

        // Farmazon UI
        Translations.Add("Farmazon", "Farmazon");
        Translations.Add("farmazon_select_item", "Select an item");
        Translations.Add("farmazon_show_cart", "Show cart");
        Translations.Add("farmazon_total_price", "Total price");
        Translations.Add("farmazon_terms_and_conditions", "By buying, you agree to Farmazon's Conditions of Use and Privacy Notice.\nTaxes are calculated depending on your location.\nFarmazon does not support refunds. If you find any problems with the item(s) bought, just buy them again!");
        Translations.Add("farmazon_buy", "Buy");
        Translations.Add("farmazon_add_to_cart", "Add to cart");
        Translations.Add("farmazon_back_to_shop", "Back to shop");

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
        Translations.Add("GarbageCan", "Garbage can");

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
        Translations.Add("compsoter_take", "Take composter");
        Translations.Add("composter_add_compost", "Add compost");
        Translations.Add("composter_take_fertilizer", "Take fertilizer");
        Translations.Add("composter_working_text", "Producing compost.\nTime remaining: {0:D2} hours {1:D2} minutes");

        Translations.Add("product_box_title", "Product box");
        Translations.Add("product_box_take", "Take product box");
        Translations.Add("product_box_add_product", "Add product");
        Translations.Add("product_box_take_product", "Take product");
        
        Translations.Add("shop_stand_title", "Shop stand");
        Translations.Add("shop_stand_take", "Take shop stand");
        Translations.Add("shop_stand_add_product", "Add product");
        Translations.Add("shop_stand_take_product", "Take product");
        Translations.Add("stand_disable", "Disable stand");
        Translations.Add("stand_enable", "Enable stand");
        Translations.Add("stand_recommended_price", "Recommended price: {0}");
        Translations.Add("stand_price", "Price:");
        
        Translations.Add("seed_box_title", "Seed box");
        Translations.Add("seed_box_take", "Take seed box");
        
        Translations.Add("deseeding_machine_title", "Deseeding machine");
        Translations.Add("deseeding_machine_working", "Producing {0} seeds.\nTime remaining: {1:D2} hours {2:D2} minutes");
        Translations.Add("deseeding_machine_take", "Take deseeding machine");
        Translations.Add("deseeding_machine_add_product", "Add product to deseed");
        Translations.Add("deseeding_machine_take_seeds", "Take seeds");
        Translations.Add("deseeding_machine_take_compost", "Take compost");
        
        Translations.Add("flour_machine_title", "Flour machine");
        Translations.Add("flour_machine_working", "Producing flour.\nTime remaining: {0:D2} minutes");
        Translations.Add("flour_machine_take", "Take flour machine");
        Translations.Add("flour_machine_add_wheat", "Add wheat");
        Translations.Add("flour_machine_take_compost", "Take compost");
        Translations.Add("flour_machine_take_flour", "Take flour");
        
        Translations.Add("bread_machine_title", "Bread machine");
        Translations.Add("bread_machine_working", "Producing bread dough.\nTime remaining: {0:D2} minutes");
        Translations.Add("bread_machine_take", "Take bread machine");
        Translations.Add("bread_machine_add_flour", "Add flour");
        Translations.Add("bread_machine_add_water", "Add water");
        Translations.Add("bread_machine_take_bread_dough", "Take bread dough");
        
        Translations.Add("sign_title", "Sign");
        Translations.Add("sign_take", "Take sign");
        Translations.Add("sign_selected_icon", "Selected icon");
        
        Translations.Add("furnace_title", "Furnace");
        Translations.Add("furnace_working", "Producing {0}.\nTime remaining: {1:D2} minutes");
        Translations.Add("furnace_take", "Take furnace");
        Translations.Add("furnace_add_product", "Add product to bake");
        Translations.Add("furnace_turn_on", "Turn on");
        Translations.Add("furnace_take_product", "Take product");
    }

    [Header("Buttons")]
    public Text ThrowObjectButton;
    public Text BuildButton;
    public Text CancelBuildButton;
    public Text OpenLetterButton;
    public Text BedText;
    public Text BedButton;
    public Text CashRegisterLogTitle;
    public Text GarbageCanTitle;
    public Text GarbageCanThrowButton;

    [Header("Options")]
    public Text OptionsTitle;
    public Text OptionsSetDayDuration;
    public Text OptionsFullscreen;
    public Text OptionsChangeRes;
    public Text OptionsChooseRes;
    public Text OptionsVolume;
    public Text OptionsVolumeMusic;
    public Text OptionsVolumeSounds;
    public Text OptionsBackMenu;
    public Text OptionsBackMenuConf;
    public Text OptionsLeaveGame;
    public Text OptionsLeaveGameConf;
    public Text OptionsUnsavedDataWarning1;
    public Text OptionsUnsavedDataWarning2;
    public Text OptionsUnsavedDataWarning3;
    public Text OptionsUnsavedDataWarning4;

    [Header("Management")]
    public Text ManagementTitle;
    public Text ManagementPayDebt;
    public Text ManagementExpand;

    [Header("Farmazon")]
    public Text FarmazonTitle;
    public Text FarmazonSelectItem;
    public Text FarmazonShowCart;
    public Text FarmazonTotalPrice;
    public Text FarmazonTermsAndConditions;
    public Text FarmazonBuy;
    public Text FarmazonAddToCart;
    public Text FarmazonBackToShop;
    
    [Header("Storage")]
    public Text StorageTitle;
    public Text TakeStorageButton;

    [Header("Composter")]
    public Text ComposterTitle;
    public Text ComposterTake;
    public Text ComposterAddCompost;
    public Text ComposterTakeFertilizer;

    [Header("Product box")]
    public Text ProductBoxTitle;
    public Text ProductBoxTake;
    public Text ProductBoxAddProduct;
    public Text ProductBoxTakeProduct;

    [Header("Shop stand")]
    public Text ShopStandTitle;
    public Text ShopStandTake;
    public Text ShopStandAddProduct;
    public Text ShopStandTakeProduct;
    public Text ShopStandPrice;

    [Header("Seed box")]
    public Text SeedBoxTitle;
    public Text SeedBoxTake;

    [Header("Deseeding machine")]
    public Text DeseedingMachineTitle;
    public Text DeseedingMachineTake;
    public Text DeseedingMachineAddProduct;
    public Text DeseedingMachineTakeSeeds;
    public Text DeseedingMachineTakeCompost;

    [Header("Flour machine")]
    public Text FlourMachineTitle;
    public Text FlourMachineTake;
    public Text FlourMachineAddWheat;
    public Text FlourMachineTakeCompost;
    public Text FlourMachineTakeFlour;

    [Header("Bread machine")]
    public Text BreadTitle;
    public Text BreadMachineTake;
    public Text BreadMachineAddFlour;
    public Text BreadMachineAddWater;
    public Text BreadMachineTakeBreadDough;

    [Header("Sign")]
    public Text SignTitle;
    public Text SignTake;
    public Text SignSelectedIcon;

    [Header("Furnace")]
    public Text FurnaceTitle;
    public Text FurnaceTake;
    public Text FurnaceAddProduct;
    public Text FurnaceTurnOn;
    public Text FurnaceTakeProduct;

    public void UpdateTexts()
    {
        BedText.text = Translations["bed_text"];
        BedButton.text = Translations["bed_button"];
        CashRegisterLogTitle.text = Translations["cash_register_log_title"];
        StorageTitle.text = Translations["storage_title"];
        TakeStorageButton.text = Translations["take_storage_button"];
        ThrowObjectButton.text = Translations["throw_object_button"];
        BuildButton.text = Translations["build_button"];
        CancelBuildButton.text = Translations["cancel_build_button"];
        OpenLetterButton.text = Translations["open_letter_button"];
        GarbageCanTitle.text = Translations["garbage_can_title"];
        GarbageCanThrowButton.text = Translations["garbage_can_throw_object_button"];

        OptionsTitle.text = Translations["Options"];
        OptionsSetDayDuration.text = Translations["options_set_day_duration"];
        OptionsFullscreen.text = Translations["options_fullscreen"];
        OptionsChangeRes.text = Translations["options_change_res"];
        OptionsChooseRes.text = Translations["options_choose_res"];
        OptionsVolume.text = Translations["options_volume"];
        OptionsVolumeMusic.text = Translations["options_volume_music"];
        OptionsVolumeSounds.text = Translations["options_volume_sounds"];
        OptionsBackMenu.text = Translations["options_back_main_menu"];
        OptionsBackMenuConf.text = Translations["options_back_main_menu_conf"];
        OptionsLeaveGame.text = Translations["options_leave_game"];
        OptionsLeaveGameConf.text = Translations["options_leave_game_conf"];
        OptionsUnsavedDataWarning1.text = Translations["options_unsaved_data_warning"];
        OptionsUnsavedDataWarning2.text = Translations["options_unsaved_data_warning"];
        OptionsUnsavedDataWarning3.text = Translations["options_are_you_sure_warning"];
        OptionsUnsavedDataWarning4.text = Translations["options_are_you_sure_warning"];
        
        ManagementTitle.text = Translations["Management"];
        ManagementPayDebt.text = Translations["management_pay_debt"];
        ManagementExpand.text = Translations["management_expand"];

        FarmazonTitle.text = Translations["Farmazon"];
        FarmazonSelectItem.text = Translations["farmazon_select_item"];
        FarmazonShowCart.text = Translations["farmazon_show_cart"];
        FarmazonTotalPrice.text = Translations["farmazon_total_price"];
        FarmazonTermsAndConditions.text = Translations["farmazon_terms_and_conditions"];
        FarmazonBuy.text = Translations["farmazon_buy"];
        FarmazonAddToCart.text = Translations["farmazon_add_to_cart"];
        FarmazonBackToShop.text = Translations["farmazon_back_to_shop"];

        ComposterTitle.text = Translations["composter_title"];
        ComposterTake.text = Translations["compsoter_take"];
        ComposterAddCompost.text = Translations["composter_add_compost"];
        ComposterTakeFertilizer.text = Translations["composter_take_fertilizer"];

        ProductBoxTitle.text = Translations["product_box_title"];
        ProductBoxTake.text = Translations["product_box_take"];
        ProductBoxAddProduct.text = Translations["product_box_add_product"];
        ProductBoxTakeProduct.text = Translations["product_box_take_product"];

        ShopStandTitle.text = Translations["shop_stand_title"];
        ShopStandTake.text = Translations["shop_stand_take"];
        ShopStandAddProduct.text = Translations["shop_stand_add_product"];
        ShopStandTakeProduct.text = Translations["shop_stand_take_product"];
        ShopStandPrice.text = Translations["stand_price"];

        SeedBoxTitle.text = Translations["seed_box_title"];
        SeedBoxTake.text = Translations["seed_box_take"];

        DeseedingMachineTitle.text = Translations["deseeding_machine_title"];
        DeseedingMachineTake.text = Translations["deseeding_machine_take"];
        DeseedingMachineAddProduct.text = Translations["deseeding_machine_add_product"];
        DeseedingMachineTakeSeeds.text = Translations["deseeding_machine_take_seeds"];
        DeseedingMachineTakeCompost.text = Translations["deseeding_machine_take_compost"];

        FlourMachineTitle.text = Translations["flour_machine_title"];
        FlourMachineTake.text = Translations["flour_machine_take"];
        FlourMachineAddWheat.text = Translations["flour_machine_add_wheat"];
        FlourMachineTakeCompost.text = Translations["flour_machine_take_compost"];
        FlourMachineTakeFlour.text = Translations["flour_machine_take_flour"];

        BreadTitle.text = Translations["bread_machine_title"];
        BreadMachineTake.text = Translations["bread_machine_take"];
        BreadMachineAddFlour.text = Translations["bread_machine_add_flour"];
        BreadMachineAddWater.text = Translations["bread_machine_add_water"];
        BreadMachineTakeBreadDough.text = Translations["bread_machine_take_bread_dough"];

        SignTitle.text = Translations["sign_title"];
        SignTake.text = Translations["sign_take"];
        SignSelectedIcon.text = Translations["sign_selected_icon"];

        FurnaceTitle.text = Translations["furnace_title"];
        FurnaceTake.text = Translations["furnace_take"];
        FurnaceAddProduct.text = Translations["furnace_add_product"];
        FurnaceTurnOn.text = Translations["furnace_turn_on"];
        FurnaceTakeProduct.text = Translations["furnace_take_product"];
    }
}