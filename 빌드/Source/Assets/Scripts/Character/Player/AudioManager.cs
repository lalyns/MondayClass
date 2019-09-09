using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource musicPlayer;
    AudioClip Music;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        
    }

    public static void playLoopSound(AudioClip clip, AudioSource audioplayer)
    {
        audioplayer.Stop();
        audioplayer.clip = clip;
        audioplayer.loop = true;
        audioplayer.time = 0;
        audioplayer.Play();
    }

    public static void playSound(AudioClip clip, AudioSource audioplayer)
    {
        audioplayer.Stop();
        audioplayer.clip = clip;
        audioplayer.loop = false;
        audioplayer.time = 0;
        audioplayer.Play();
    }
}
