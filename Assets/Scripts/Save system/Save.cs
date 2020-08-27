﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    // Options system
    public OptionsData OptionsData;

    // Game system (Master)
    public GeneralData GeneralData;

    // Time system
    public TimeData TimeData;

    // Map system (MapGen)
    public MapData MapData;

    // Vertex system
    public List<Vertex> Vertices;

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

    // Cash register system
    public CashRegisterData CashRegisterData;

    // Delivery system
    public DeliverySystemData DeliverySystemData;

    // Tools system (Physical)
    public ToolsData ToolsData;

    // Inventory system
    public InventoryData InventoryData;

}