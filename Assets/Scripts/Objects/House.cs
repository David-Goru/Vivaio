using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class House : BuildableObject
{
    public House() : base("House", 1, 1, "House") {}

    public override void OnObjectPlaced(GameObject model)
    {
        Model = model;       

        // Tools
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Tool"))
        {
            g.GetComponent<ToolPhysical>().New();
        } 

        // Delivery system
        foreach (Transform t in model.transform.Find("Delivery points"))
        {
            DeliverySystem.Data.DeliveryPoints.Add(new DeliveryPoint(t.position));
        }

        // First grandma present
        Box box = new Box("Present box", "PresentBox");
        DeliveryPoint point = DeliverySystem.Data.DeliveryPoints[1];
        point.Available = false;
        box.Point = point;
        box.Model = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Objects/Present box"), point.Pos, Quaternion.Euler(0, 0, 0));
        box.Model.name = "Present box";
        string type = "Family";
        string title = Localization.Translations["grandma_first_letter_title"];
        string body = Localization.Translations["grandma_first_letter_body"];
        string signature = Localization.Translations["grandma_signature"];
        box.Items[0] = new Letter(type, title, body, signature);
        box.Items[1] = new Seed("Carrot", 10, 10, "CarrotSeeds");
        box.Placed = true;
        box.WorldPosition = point.Pos;
        ObjectsHandler.Data.Objects.Add(box);

        // Colliders
        model.transform.Find("Mailbox").Find("Obstacle").GetComponent<BoxCollider2D>().enabled = true;
        model.transform.Find("First floor entry").GetComponent<BoxCollider2D>().enabled = true;
        model.transform.Find("Basement entry").GetComponent<BoxCollider2D>().enabled = true;
        model.transform.Find("Tap").GetComponent<BoxCollider2D>().enabled = true;

        // Doors
        model.transform.Find("First floor entry").GetComponent<Door>().Teleport = GameObject.Find("Environment").transform.Find("First floor").Find("Entry");
        GameObject.Find("Environment").transform.Find("First floor").Find("Exit").GetComponent<Door>().Teleport = model.transform.Find("First floor entry").Find("First floor exit");
        model.transform.Find("Basement entry").GetComponent<Door>().Teleport = GameObject.Find("Environment").transform.Find("Basement ground").Find("Entry");
        GameObject.Find("Environment").transform.Find("Basement ground").Find("Exit").GetComponent<Door>().Teleport = model.transform.Find("Basement entry").Find("Basement floor exit");
    }

    public override void LoadObjectCustom()
    {
        Model.transform.Find("Mailbox").Find("Obstacle").GetComponent<BoxCollider2D>().enabled = true;
        Model.transform.Find("First floor entry").GetComponent<BoxCollider2D>().enabled = true;
        Model.transform.Find("Basement entry").GetComponent<BoxCollider2D>().enabled = true;
        Model.transform.Find("Tap").GetComponent<BoxCollider2D>().enabled = true;

        Model.transform.Find("First floor entry").GetComponent<Door>().Teleport = GameObject.Find("Environment").transform.Find("First floor").Find("Entry");
        GameObject.Find("Environment").transform.Find("First floor").Find("Exit").GetComponent<Door>().Teleport = Model.transform.Find("First floor entry").Find("First floor exit");
        Model.transform.Find("Basement entry").GetComponent<Door>().Teleport = GameObject.Find("Environment").transform.Find("Basement ground").Find("Entry");
        GameObject.Find("Environment").transform.Find("Basement ground").Find("Exit").GetComponent<Door>().Teleport = Model.transform.Find("Basement entry").Find("Basement floor exit");
    }

    public override void ActionOne()
    {
        GameObject.Find("Farm handler").GetComponent<Build>().StartBuild(Model);
    }
}