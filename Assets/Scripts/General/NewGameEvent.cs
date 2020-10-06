using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameEvent : MonoBehaviour
{
    public void NewGameAnimEnd()
    {
        SceneManager.LoadScene("Game");
    }
}