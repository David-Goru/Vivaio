using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    int currentpage;
    public Sprite[] Texts;

    public void JumpToPage(int newpage)
    {
        if (newpage == -1)
            currentpage++;
        else if (newpage == -2)
            currentpage--;
        else currentpage = newpage;
        
        transform.Find("Text").gameObject.GetComponent<Image>().sprite = Texts[currentpage];

        transform.Find("Next").gameObject.SetActive(true);
        transform.Find("Previous").gameObject.SetActive(true);

        if (currentpage == (Texts.Length - 1)) transform.Find("Next").gameObject.SetActive(false);
        else if (currentpage == 0) transform.Find("Previous").gameObject.SetActive(false);
    }
}