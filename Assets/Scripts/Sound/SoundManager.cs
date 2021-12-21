using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float globalVolume;
    public Sound[] sounds;
    public Sound[] songs;

    public void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * globalVolume;
            s.source.loop = s.loop;
        }
        foreach (Sound s in songs)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * globalVolume;
            s.source.loop = s.loop;
        }

        int r = UnityEngine.Random.Range(0, songs.Length);
        string rname = songs[r].name;
        PlaySong(rname);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            int r = UnityEngine.Random.Range(0, songs.Length);
            string rname = songs[r].name;
            PlaySong(rname);
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (!s.source.loop && !s.multiShot)
            s.source.PlayOneShot(s.clip);
        else if (!s.source.isPlaying)
            s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        if (s.source.isPlaying)
            s.source.Stop();
    }

    public void PlaySong(string name)
    {
        foreach (Sound song in songs)
        {
            song.source.Stop();
        }
        Sound s = Array.Find(songs, song => song.name == name);
        if (s == null)
        {
            //Debug.LogWarning("Song: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
