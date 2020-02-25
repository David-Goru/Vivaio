using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageHandler : MonoBehaviour
{
    public GameObject Box;

    void Update()
    {
        if (Vector2.Distance(GameObject.Find("Player").transform.position, Box.transform.position) > 1.5f)
        {
            GameObject.Find("UI").transform.Find("Take storage").gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void TakeStorage()
    {
        if (!gameObject.activeSelf || Inventory.ObjectInHand != null) return;
        StorageSystem.StorageList.Find(x => x.BoxObject == Box).RemoveBox();
        BuildableObject bo = new BuildableObject(-1);
        bo.Name = "Storage box";
        Inventory.ObjectInHand = bo;
        Inventory.ChangeObject();
        GameObject.Find("UI").transform.Find("Build button").gameObject.SetActive(true);
        GameObject.Find("UI").transform.Find("Take storage").gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}