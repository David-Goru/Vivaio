using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public void New()
    {
        transform.Find("Initial screen").gameObject.SetActive(false);
        transform.Find("Create screen").gameObject.SetActive(true);
    }

    public void Load()
    {
        transform.Find("Initial screen").gameObject.SetActive(false);
        transform.Find("Load screen").gameObject.SetActive(true);

        Transform content = transform.Find("Load screen").Find("Games").Find("Viewport").Find("Content");

        foreach(Transform t in content)
        {
            Destroy(t.gameObject);
        }

        DirectoryInfo saves = new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Vivaio/Saves");
        foreach (FileInfo f in saves.GetFiles())
        {
            GameObject b = Instantiate(Resources.Load<GameObject>("Game button"), transform.position, transform.rotation);
            b.transform.Find("Text").GetComponent<Text>().text = Path.GetFileNameWithoutExtension(f.Name);
            b.GetComponent<Button>().onClick.AddListener(() => LoadGame(Path.GetFileNameWithoutExtension(f.Name)));
            b.transform.SetParent(content);
        }
    }

    public void Back()
    {
        transform.Find("Load screen").gameObject.SetActive(false);
        transform.Find("Create screen").gameObject.SetActive(false);
        transform.Find("Initial screen").gameObject.SetActive(true);
    }

    public void CreateGame()
    {
        string name = transform.Find("Create screen").Find("Game input").Find("Text").GetComponent<Text>().text;
        if (name != "")
        {
            Master.LoadingGame = false;
            Master.GameName = name;
            SceneManager.LoadScene("Game");
        }
    }

    public void LoadGame(string name)
    {
        Master.LoadingGame = true;
        Master.GameName = name;
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}