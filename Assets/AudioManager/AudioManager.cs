using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static int soundHold;

    public static string prevSound;

    // Use this for initialization
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            foreach (AudioClip clip in s.clips)
            {
                var source = gameObject.AddComponent<AudioSource>();
                s.sources.Add(source);

                source.clip = clip;

                source.volume = s.volume;
                source.pitch = s.pitch;
                source.loop = s.Loop;
            }
        }
    }

    public void Play(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        var i = UnityEngine.Random.Range(0, s.sources.Count);

        if (!(s.sources[i].isPlaying == true && soundHold > 0))
        {

            s.sources[i].pitch = s.pitch * (1f + UnityEngine.Random.Range(-1f, 1f) * s.randomPitchFluctuation);
            s.sources[i].Play();

            prevSound = name;
            print(prevSound);
        }

        soundHold = 2;
    }

    public void Stop(string name, int i)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.sources[i].Stop();
    }

    public void FadeOut(string name, int i)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);


        if (s.sources[i].volume > 0.1)
        {
            s.sources[i].volume -= 0.1f;
        }
    }
}