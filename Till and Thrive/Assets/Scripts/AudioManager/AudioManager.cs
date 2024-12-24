using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{ 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<AudioSource> SFXSources;

    public void StartMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlaySFX(int index, AudioClip audioClip)
    {
        if (index >= 0 && index < SFXSources.Count)
        {
            SFXSources[index].clip = audioClip;
            SFXSources[index].Play();
        }
        else
        {
            Debug.LogWarning("SFX index out of range");
        }
    }
}
