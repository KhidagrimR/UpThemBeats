using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTutorialDown : MonoBehaviour {
    public string textBeforeImage;

    public Sprite spriteKeyKeyBoardRightHanded;
    public Sprite spriteAlternativeKeyKeyBoardRightHanded;

    public Sprite spriteKeyKeyBoardLeftHanded;
    public Sprite spriteAlternativeKeyKeyBoardLeftHanded;

    public Sprite spriteKeyController;
    public Sprite spriteAlternativeKeyController;

    public string textAfterImage;
    public float timeToStayToScreen;
    public bool timerWithText;

    public void OnTriggerEnter(Collider other) {
        if (other.name == "Player")
        {
            StartCoroutine(TutorialDown.Instance.LaunchTutorial(timeToStayToScreen, textBeforeImage,
                           spriteKeyKeyBoardRightHanded, spriteAlternativeKeyKeyBoardRightHanded,
                           spriteKeyKeyBoardLeftHanded, spriteAlternativeKeyKeyBoardLeftHanded,
                           spriteKeyController, spriteAlternativeKeyController, textAfterImage, timerWithText));
        }
    }
}
