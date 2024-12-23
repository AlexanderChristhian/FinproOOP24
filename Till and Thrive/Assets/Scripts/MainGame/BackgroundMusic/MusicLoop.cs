using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] AudioClip GameMusic1;
    [SerializeField] AudioClip GameMusic2;
    [SerializeField] AudioClip GameMusic3;
    [SerializeField] AudioClip GameMusic4;
    int currentTrackIndex = 0;
    AudioClip[] gameMusic;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        gameMusic = new AudioClip[] { GameMusic1, GameMusic2, GameMusic3, GameMusic4 };
        PlayNextTrack();
    }

    void PlayNextTrack()
    {
        audioManager.StartMusic(gameMusic[currentTrackIndex]);
        currentTrackIndex = (currentTrackIndex + 1) % gameMusic.Length;
        Invoke("PlayNextTrack", gameMusic[currentTrackIndex].length + 10);
    }
}
