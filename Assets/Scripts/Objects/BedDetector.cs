using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedDetector : MonoBehaviour
{
    public void OnTriggerEnter2D()
    {
        if (!TimeSystem.Data.Sleeping) UI.Elements["Bed"].SetActive(true);
    }

    public void OnTriggerExit2D()
    {        
        UI.Elements["Bed"].SetActive(false);
    }
}