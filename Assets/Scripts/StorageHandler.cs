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
            gameObject.SetActive(false);
        }
    }
}
