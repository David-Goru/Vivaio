using UnityEngine;

[System.Serializable]
public class OptionsData
{
    [SerializeField]
    public int Width;
    [SerializeField]
    public int Height;
    [SerializeField]
    public string DataPath;
    [SerializeField]
    public int FPS;
    [SerializeField]
    public float MinuteValue;
    [SerializeField]
    public bool FullScreen;
    [SerializeField]
    public float MusicVolume;
    [SerializeField]
    public float SoundsVolume;
}