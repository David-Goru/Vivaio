using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public void CallLoadGame()
    {
        GameObject.Find("UI").transform.GetComponent<MainMenu>().LoadGameNow();
    }
}
