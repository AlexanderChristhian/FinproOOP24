using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialButton : ButtonClick
{
    private Image TutorialImage;
    private TextMeshProUGUI TutorialText;

    protected override IEnumerator LoadUIWithDelay()
    {
        yield return new WaitForSeconds(ButtonClickSound.length);
        TutorialImage = GameObject.Find("GameTutorial").GetComponent<Image>();
        TutorialText = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>();

        if (TutorialImage.enabled)
        {
            TutorialImage.enabled = false;
            TutorialText.enabled = false;
        }
        else
        {
            TutorialImage.enabled = true;
            TutorialText.enabled = true;
        }
    }
}
