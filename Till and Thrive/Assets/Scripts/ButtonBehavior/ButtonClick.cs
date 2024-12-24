using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    protected AudioManager audioManager;
    [SerializeField] protected AudioClip ButtonClickSound;
    protected void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void LoadScene()
    {
        audioManager.PlaySFX(0, ButtonClickSound);
        StartCoroutine(LoadSceneWithDelay());
    }

    public void LoadUI()
    {
        audioManager.PlaySFX(0, ButtonClickSound);
        StartCoroutine(LoadUIWithDelay());
    }

    protected virtual IEnumerator LoadSceneWithDelay()
    {
        yield return null;
    }

    protected virtual IEnumerator LoadUIWithDelay()
    {
        yield return null;
    }
}
