using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    new AudioSource audio;
    public AudioClip[] Intro_audioSources;
    public AudioClip[] Walk_audioSources;
    public AudioClip[] Greet_audioSources;
    public AudioClip[] Sword_audioSources;
    private void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.mute = false;
        audio.loop = false;
        audio.playOnAwake = false;
    }

    public void Introduce()
    {
        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0:
                audio.clip = Intro_audioSources[random];
                audio.Play();
                break;
            case 1:
                audio.clip = Intro_audioSources[random];
                audio.Play();
                break;
            case 2:
                audio.clip = Intro_audioSources[random];
                audio.Play();
                break;
            default:
                break;
        }
    }

    public void Walking()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case 0:
                audio.clip = Walk_audioSources[random];
                audio.Play();
                break;
            case 1:
                audio.clip = Walk_audioSources[random];
                audio.Play();
                break;
            default:
                break;
        }
    }

    public void Greeting()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case 0:
                audio.clip = Greet_audioSources[random];
                audio.Play();
                break;
            case 1:
                audio.clip = Greet_audioSources[random];
                audio.Play();
                break;
            default:
                break;
        }
    }

    public void Sword()
    {
        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0:
                audio.clip = Sword_audioSources[random];
                audio.Play();
                break;
            case 1:
                audio.clip = Sword_audioSources[random];
                audio.Play();
                break;
            case 2:
                audio.clip = Sword_audioSources[random];
                audio.Play();
                break;
            default:
                break;
        }
    }
}
