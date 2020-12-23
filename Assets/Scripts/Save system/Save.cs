using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    // Options system
    public OptionsData OptionsData;

    // Game system (Master)
    public GeneralData GeneralData;

    // Management system
    public ManagementData ManagementData;

    // Time system
    public TimeData TimeData;

    // Map system (MapGen)
    public MapData MapData;

    // Vertex system
    public Vertex[,] GridInfo;

    // Farm system
    public FarmData FarmData;

    // Farmazon system
    public FarmazonData FarmazonData;

    // Mailbox system
    public MailboxData MailboxData;

    // AI system
    public AIData AIData;

    // Objects system
    public ObjectsData ObjectsData;

    // Delivery system
    public DeliverySystemData DeliverySystemData;

    // Tools system (Physical)
    public ToolsData ToolsData;

    // Inventory system
    public InventoryData InventoryData;

}