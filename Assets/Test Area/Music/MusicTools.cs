using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTools : MonoBehaviour
{
    public string Title;

    void Start()
    {
        Time.timeScale = 0.01f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) ScreenCapture.CaptureScreenshot(Title + ".png");
    }
}