using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] AudioClip menuMusic;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.StartMusic(menuMusic);
    }
}