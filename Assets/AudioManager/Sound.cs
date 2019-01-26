using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string name;

    public List<AudioClip> clips;

    [Range(0, 10)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    [Range(0.0f, 0.5f)]
    public float randomPitchFluctuation;

    public bool Loop;

    [HideInInspector]
    public List<AudioSource> sources;
}

