using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    int currentPage;
    [SerializeField] int totalPages = 0;

    public void JumpToPage(int newpage)
    {
        UI.Elements["Tutorial page " + currentPage].SetActive(false);
        if (newpage == -1)
            currentPage++;
        else if (newpage == -2)
            currentPage--;
        else currentPage = newpage;
        
        UI.Elements["Tutorial page " + currentPage].SetActive(true);
        UI.Elements["Tutorial next"].SetActive(true);
        UI.Elements["Tutorial previous"].SetActive(true);

        if (currentPage == (totalPages - 1)) UI.Elements["Tutorial next"].SetActive(false);
        else if (currentPage == 0) UI.Elements["Tutorial previous"].SetActive(false);
    }
}