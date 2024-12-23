using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsButton : ButtonClick
{
    private Image SettingsCanvas;
    private Image QuitGameImage;
    private Image MainMenuImage;
    private TextMeshProUGUI QuitGameText;
    private TextMeshProUGUI MainMenuText;

    protected override IEnumerator LoadUIWithDelay()
    {
        yield return new WaitForSeconds(ButtonClickSound.length);
        SettingsCanvas = GameObject.Find("SettingsCanvas").GetComponent<Image>();
        QuitGameImage = GameObject.Find("QuitGame").GetComponent<Image>();
        MainMenuImage = GameObject.Find("ReturnToMainMenu").GetComponent<Image>();
        QuitGameText = GameObject.Find("QuitGameText").GetComponent<TextMeshProUGUI>();
        MainMenuText = GameObject.Find("ReturnToMainMenuText").GetComponent<TextMeshProUGUI>();

        if (SettingsCanvas.enabled)
        {
            SettingsCanvas.enabled = false;
            QuitGameImage.enabled = false;
            MainMenuImage.enabled = false;
            QuitGameText.enabled = false;
            MainMenuText.enabled = false;
        }
        else
        {
            SettingsCanvas.enabled = true;
            QuitGameImage.enabled = true;
            MainMenuImage.enabled = true;
            QuitGameText.enabled = true;
            MainMenuText.enabled = true;
        }
    }
}
