using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public void OnTriggerEnter2D()
    {
        if (TimeSystem.TimeState == "Night")
        {
            GameObject.Find("UI").transform.Find("Bed").gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D()
    {        
        GameObject.Find("UI").transform.Find("Bed").gameObject.SetActive(false);
    }
}