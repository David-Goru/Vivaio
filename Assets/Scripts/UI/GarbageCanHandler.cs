using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageCanHandler : MonoBehaviour
{
    void Update()
    {
        if (Vector2.Distance(GameObject.Find("Player").transform.position, transform.position) > 2)
        {
            GameObject.Find("UI").transform.Find("Garbage can").gameObject.SetActive(false);
            transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Garbage can closed");
            enabled = false;
        }
    }
}