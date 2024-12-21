using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public GameObject MusicPlayer;
    public void LoadScene()
    {
        SceneManager.LoadScene("MainGame");
        DontDestroyOnLoad(MusicPlayer);
    }
}