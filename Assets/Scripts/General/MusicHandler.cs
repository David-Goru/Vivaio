using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioClip[] Clips;
    public static List<Song> Songs;
    public double FadeRate;
    public AudioSource AudioSource;
    public static MusicHandler Handler;

    void Start()
    {
        AudioSource = gameObject.GetComponent<AudioSource>();
        Songs = new List<Song>();

        for (int i = 0; i < Clips.Length; i++)
        {
            Songs.Add(new Song((SongType)i, Clips[i]));
        }

        Handler = GameObject.Find("Music handler").GetComponent<MusicHandler>();
    }

    public static void StartTransition(SongType newSongType)
    {
        Handler.StopAllCoroutines();
        Handler.StartCoroutine(Handler.SongsTransition(newSongType));
    }

    public IEnumerator SongsTransition(SongType newSongType)
    {
        // Fade out
        while(AudioSource.volume > 0.1f)
        {
            AudioSource.volume -= (float)(FadeRate * Time.deltaTime);
            yield return new WaitForSeconds(0.05f);
        }
        AudioSource.volume = 0.0f;

        // Change clip
        AudioSource.clip = Songs.Find(x => x.Type == newSongType).Track;
        yield return new WaitForSeconds(1);
        if (newSongType == SongType.Night) AudioSource.loop = false;
        else AudioSource.loop = true;
        AudioSource.Play();

        // Fade in
        while(AudioSource.volume < Options.Data.Volume)
        {
            AudioSource.volume += (float)(FadeRate * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        AudioSource.volume = Options.Data.Volume;
    }
}

public class Song
{
    public SongType Type;
    public AudioClip Track;

    public Song(SongType type, AudioClip track)
    {
        Type = type;
        Track = track;
    }
}

public enum SongType
{
    Morning = 0,
    ShopOpen = 1,
    Evening = 2,
    Night = 3
}