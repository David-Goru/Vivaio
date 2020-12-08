using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideShowButtons : MonoBehaviour
{
    // We don't like garbage
    Vector3 oldMainPos;
    Vector3 newMainPos;

    public void UpdateMainButton(GameObject newMainButton)
    {
        Options.Data.TopBarButtons.Find(x => x.Button == newMainButton).SetButton(Options.Data.TopBarButtons.Find(x => x.SlotName == "Main slot").Button);
        Options.Data.TopBarButtons.Find(x => x.SlotName == "Main slot").SetButton(newMainButton);
    }

    public void Collapse()
    {
        Transform mainButton = Options.Data.TopBarButtons.Find(x => x.SlotName == "Main slot").Button.transform;
        foreach (Transform t in transform)
        {
            if (t == mainButton) continue;
            t.gameObject.SetActive(false);
        }
        Options.Data.TopBarCollapsed = true;
    }

    public void Expand()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
        Options.Data.TopBarCollapsed = false;
    }
}