using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public int musicType;

    
    public AudioClip audioClip;

    void Awake() {


            if (GameObject.Find("music") == null)
            {
                var musicInstance = new GameObject("music");
                DontDestroyOnLoad(musicInstance);
                var audioSource = musicInstance.AddComponent<AudioSource>();
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
            }

    }
}
