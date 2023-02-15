using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTutorialUp : MonoBehaviour
{
    public string textBeforeImage;

    public Sprite spriteKeyKeyBoardRightHanded;
    public Sprite spriteAlternativeKeyKeyBoardRightHanded;

    public Sprite spriteKeyKeyBoardLeftHanded;
    public Sprite spriteAlternativeKeyKeyBoardLeftHanded;

    public Sprite spriteKeyController;
    public Sprite spriteAlternativeKeyController;

    public string textAfterImage;
    public float timeToStayToScreen;

    public bool timer;
    public bool timerWithText;
    public bool alternativeKey;

    public void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            StartCoroutine(TutorialUp.Instance.LaunchTutorial(timeToStayToScreen, textBeforeImage,
                           spriteKeyKeyBoardRightHanded, spriteAlternativeKeyKeyBoardRightHanded,
                           spriteKeyKeyBoardLeftHanded, spriteAlternativeKeyKeyBoardLeftHanded, 
                           spriteKeyController, spriteAlternativeKeyController, textAfterImage, timer, timerWithText, alternativeKey));
        }
    }
}
