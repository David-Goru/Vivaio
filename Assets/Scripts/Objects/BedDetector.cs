using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedDetector : MonoBehaviour
{
    public void OnTriggerEnter2D()
    {
        if (!TimeSystem.Data.Sleeping) GameObject.Find("UI").transform.Find("Bed").gameObject.SetActive(true);
    }

    public void OnTriggerExit2D()
    {        
        GameObject.Find("UI").transform.Find("Bed").gameObject.SetActive(false);
    }
}