using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class GameSaver : MonoBehaviour
{
    public static void SaveGame()
    {
        Save save = new Save();

        // Options system
        save.OptionsData = Options.Data;

        // Game system (Master)
        save.GeneralData = Master.Data;

        // Management system
        save.ManagementData = Management.Data;

        // Time system
        save.TimeData = TimeSystem.Data;

        // Map system (MapGen)
        save.MapData = MapSystem.Data;

        // Vertex system
        save.Vertices = VertexSystem.Vertices;

        // Farm system
        save.FarmData = Farm.Save();

        // Farmazon system
        save.FarmazonData = Farmazon.Save();

        // Mailbox system
        save.MailboxData = Mailbox.Data;

        // AI system
        save.AIData = AI.Save();

        // Objects system
        save.ObjectsData = ObjectsHandler.Data;

        // Cash register system
        save.CashRegisterData = CashRegisterHandler.Data;

        // Delivery system
        save.DeliverySystemData = DeliverySystem.Data;

        // Tools system (Physical)
        save.ToolsData = ToolsHolder.Data;

        // Inventory system
        save.InventoryData = Inventory.Data;

        BinaryFormatter bf = new BinaryFormatter();
        SurrogateSelector surrogateSelector = new SurrogateSelector();
        Vector2SerializationSurrogate vector2SS = new Vector2SerializationSurrogate();
        surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2SS);
        bf.SurrogateSelector = surrogateSelector;

        FileStream file = File.Create(string.Format("{0}/Saves/{1}.save", Options.Data.DataPath, Master.GameName));
        bf.Serialize(file, save);
        file.Close();

        GameObject uiElement = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Game saved"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        uiElement.transform.SetParent(GameObject.Find("UI").transform);
        uiElement.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        uiElement.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}