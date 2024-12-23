using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : ButtonClick
{
    protected override IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(ButtonClickSound.length);
        SceneManager.LoadScene("MainMenu");
    }
}
