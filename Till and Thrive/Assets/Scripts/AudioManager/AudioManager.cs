using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{ 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    public void StartMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        SFXSource.clip = audioClip;
        SFXSource.Play();
    }
}
