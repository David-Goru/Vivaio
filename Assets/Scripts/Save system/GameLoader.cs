using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class GameLoader : MonoBehaviour
{
    public static List<string> Log;
    Text loadingText;

    Save saveFile;

    void Start()
    {
        if (Localization.Translations == null) Localization.LoadTranslations();

        Log = new List<string>();
        loadingText = GameObject.Find("UI").transform.Find("Load screen").Find("Text").gameObject.GetComponent<Text>();

        if (Master.LoadingGame) StartCoroutine(LoadGame());
        else StartCoroutine(NewGame());
    }

    IEnumerator NewGame()
    {        
        // Options system
        loadingText.text = string.Format("Creating {0}...", "options");
        yield return new WaitUntil(() => Options.New());
        yield return new WaitForSeconds(0.05f);

        // Icons system
        loadingText.text = string.Format("Creating {0}...", "icons");
        yield return new WaitUntil(() => IconsHandler.New());
        yield return new WaitForSeconds(0.05f);

        // Game system (Master)
        loadingText.text = string.Format("Creating {0}...", "master");
        yield return new WaitUntil(() => Master.New());
        yield return new WaitForSeconds(0.05f);

        // Management system
        loadingText.text = string.Format("Creating {0}...", "management");
        yield return new WaitUntil(() => Management.New());
        yield return new WaitForSeconds(0.05f);

        // Time system
        loadingText.text = string.Format("Creating {0}...", "time");
        yield return new WaitUntil(() => TimeSystem.New());
        yield return new WaitForSeconds(0.05f);

        // Map system (MapGen)
        loadingText.text = string.Format("Creating {0}...", "map");
        yield return new WaitUntil(() => MapSystem.New());
        yield return new WaitForSeconds(0.05f);

        // Vertex system
        loadingText.text = string.Format("Creating {0}...", "vertex grid");
        yield return new WaitUntil(() => VertexSystem.New());
        yield return new WaitForSeconds(0.05f);

        // Farm system
        loadingText.text = string.Format("Creating {0}...", "farm");
        yield return new WaitUntil(() => Farm.New());
        yield return new WaitForSeconds(0.05f);

        // Farmazon system
        loadingText.text = string.Format("Creating {0}...", "farmazon");
        yield return new WaitUntil(() => Farmazon.New());
        yield return new WaitForSeconds(0.05f);

        // Mailbox system
        loadingText.text = string.Format("Creating {0}...", "mailbox");
        yield return new WaitUntil(() => Mailbox.New());
        yield return new WaitForSeconds(0.05f);

        // AI system
        loadingText.text = string.Format("Creating {0}...", "AI");
        yield return new WaitUntil(() => AI.New());
        yield return new WaitForSeconds(0.05f);

        // Objects system
        loadingText.text = string.Format("Creating {0}...", "objects");
        yield return new WaitUntil(() => ObjectsHandler.New());
        yield return new WaitForSeconds(0.05f);

        // Delivery system
        loadingText.text = string.Format("Creating {0}...", "delivery system");
        yield return new WaitUntil(() => DeliverySystem.New());
        yield return new WaitForSeconds(0.05f);

        // Tools system (Physical)
        loadingText.text = string.Format("Creating {0}...", "tools");
        yield return new WaitUntil(() => ToolsHolder.New());
        yield return new WaitForSeconds(0.05f);

        // Inventory system
        loadingText.text = string.Format("Creating {0}...", "inventory");
        yield return new WaitUntil(() => Inventory.New());
        yield return new WaitForSeconds(0.05f);

        UI.Elements["Tutorial"].SetActive(true);
        UI.Elements["Load screen"].SetActive(false);

        // Initialize time ticks
        TimeSystem ts = GameObject.Find("Farm handler").GetComponent<TimeSystem>();
        ts.StartCoroutine(ts.TimeTick());

        // Set texts translations
        Localization.UpdateTexts();

        Destroy(gameObject);
    }

    IEnumerator LoadGame()
    {
        // Save file
        loadingText.text = string.Format("Loading {0}...", "data file");
        yield return new WaitUntil(() => 
            {
                BinaryFormatter bf = new BinaryFormatter();
                SurrogateSelector surrogateSelector = new SurrogateSelector();
                Vector2SerializationSurrogate vector2SS = new Vector2SerializationSurrogate();
                surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2SS);
                bf.SurrogateSelector = surrogateSelector;

                FileStream file = File.Open(string.Format("{0}/Saves/{1}.save", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Vivaio", Master.GameName), FileMode.Open);
                saveFile = (Save)bf.Deserialize(file);
                file.Close();
                return true;
            }
        );

        // Options system
        loadingText.text = string.Format("Loading {0}...", "options");
        yield return new WaitUntil(() => Options.Load(saveFile.OptionsData));
        yield return new WaitForSeconds(0.05f);

        // Icons system
        loadingText.text = string.Format("Loading {0}...", "icons");
        yield return new WaitUntil(() => IconsHandler.New());
        yield return new WaitForSeconds(0.05f);

        // Game system (Master)
        loadingText.text = string.Format("Loading {0}...", "master");
        yield return new WaitUntil(() => Master.Load(saveFile.GeneralData));
        yield return new WaitForSeconds(0.05f);

        // Management system
        loadingText.text = string.Format("Loading {0}...", "management");
        yield return new WaitUntil(() => Management.Load(saveFile.ManagementData));
        yield return new WaitForSeconds(0.05f);

        // Time system
        loadingText.text = string.Format("Loading {0}...", "time");
        yield return new WaitUntil(() => TimeSystem.Load(saveFile.TimeData));
        yield return new WaitForSeconds(0.05f);

        // Map system (MapGen)
        loadingText.text = string.Format("Loading {0}...", "map");
        yield return new WaitUntil(() => MapSystem.Load(saveFile.MapData));
        yield return new WaitForSeconds(0.05f);

        // Vertex system
        loadingText.text = string.Format("Loading {0}...", "vertex grid");
        yield return new WaitUntil(() => VertexSystem.Load(saveFile.GridInfo));
        yield return new WaitForSeconds(0.05f);

        // Farm system
        loadingText.text = string.Format("Loading {0}...", "farm");
        yield return new WaitUntil(() => Farm.Load(saveFile.FarmData));
        yield return new WaitForSeconds(0.05f);

        // Farmazon system
        loadingText.text = string.Format("Loading {0}...", "farmazon");
        yield return new WaitUntil(() => Farmazon.Load(saveFile.FarmazonData));
        yield return new WaitForSeconds(0.05f);

        // Mailbox system
        loadingText.text = string.Format("Loading {0}...", "mailbox");
        yield return new WaitUntil(() => Mailbox.Load(saveFile.MailboxData));
        yield return new WaitForSeconds(0.05f);

        // AI system
        loadingText.text = string.Format("Loading {0}...", "AI");
        yield return new WaitUntil(() => AI.Load(saveFile.AIData));
        yield return new WaitForSeconds(0.05f);

        // Objects system
        loadingText.text = string.Format("Loading {0}...", "objects");
        yield return new WaitUntil(() => ObjectsHandler.Load(saveFile.ObjectsData));
        yield return new WaitForSeconds(0.05f);

        // Delivery system
        loadingText.text = string.Format("Loading {0}...", "delivery system");
        yield return new WaitUntil(() => DeliverySystem.Load(saveFile.DeliverySystemData));
        yield return new WaitForSeconds(0.05f);

        // Tools system (Physical)
        loadingText.text = string.Format("Loading {0}...", "tools");
        yield return new WaitUntil(() => ToolsHolder.Load(saveFile.ToolsData));
        yield return new WaitForSeconds(0.05f);

        // Inventory system
        loadingText.text = string.Format("Loading {0}...", "inventory");
        yield return new WaitUntil(() => Inventory.Load(saveFile.InventoryData));
        yield return new WaitForSeconds(0.05f);

        // Initialize time ticks
        TimeSystem ts = GameObject.Find("Farm handler").GetComponent<TimeSystem>();
        ts.StartCoroutine(ts.TimeTick());

        // Set texts translations
        Localization.UpdateTexts();

        // Finished loading
        if (Log.Count > 0)
        {
            loadingText.text = Localization.Translations["failed_to_load_game"];
            string errorText = Localization.Translations["load_error_text_1"];
            errorText += Localization.Translations["load_error_text_2"];
            errorText += string.Format(Localization.Translations["load_error_text_3"], Master.GameVersion); 
            foreach (string s in Log)
            {
                errorText += s;
                errorText += "\n";
            }
            UI.Elements["Load screen error log"].GetComponent<Text>().text = errorText;
        }
        else
        {
            UI.Elements["Load screen"].SetActive(false);
            Destroy(gameObject);
        }
    }
}