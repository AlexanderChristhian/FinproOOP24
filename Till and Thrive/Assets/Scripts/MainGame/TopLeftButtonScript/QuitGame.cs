using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : ButtonClick
{

    public void ExitGame()
    {
        audioManager.PlaySFX(0, ButtonClickSound);
        StartCoroutine(ExitGameWithDelay());
    }

    private IEnumerator ExitGameWithDelay()
    {
        yield return new WaitForSeconds(ButtonClickSound.length);
        Application.Quit();
    }
}
