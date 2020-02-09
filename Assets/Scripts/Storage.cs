using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) <= 1.5f)
        {
            GameObject StorageUI = GameObject.Find("UI").transform.Find("Storage").gameObject;
            (StorageUI.GetComponent("StorageHandler") as StorageHandler).Box = gameObject;
            StorageUI.SetActive(true);
        }
    }
}
