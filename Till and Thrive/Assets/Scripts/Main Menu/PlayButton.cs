using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] AudioClip ButtonClick;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void LoadScene()
    {
        audioManager.PlaySFX(ButtonClick);
        StartCoroutine(LoadSceneWithDelay());
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(ButtonClick.length);
        SceneManager.LoadScene("MainGame");
    }
}