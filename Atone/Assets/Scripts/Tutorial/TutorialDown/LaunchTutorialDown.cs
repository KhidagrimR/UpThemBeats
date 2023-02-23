using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTutorialDown : MonoBehaviour {
    public string textBeforeImage;

    [Header("Sprite Key Right Handed")]
    public Sprite spriteKeyKeyBoardRightHanded;
    public Sprite spriteAlternativeKeyKeyBoardRightHanded;

    [Header("Sprite Key Left Handed")]
    public Sprite spriteKeyKeyBoardLeftHanded;
    public Sprite spriteAlternativeKeyKeyBoardLeftHanded;

    [Header("Sprite Key Controller")]
    public Sprite spriteKeyController;
    public Sprite spriteAlternativeKeyController;

    public string textAfterImage;

    public float timeToStayToScreen;
    public bool timerWithText;
    public bool alternativeKey;

    public float timeBetweenTwoCount;

    public void OnTriggerEnter(Collider other) {
        if (other.name == "Player")
        {
            StartCoroutine(TutorialDown.Instance.LaunchTutorial(timeToStayToScreen, textBeforeImage,
                           spriteKeyKeyBoardRightHanded, spriteAlternativeKeyKeyBoardRightHanded,
                           spriteKeyKeyBoardLeftHanded, spriteAlternativeKeyKeyBoardLeftHanded,
                           spriteKeyController, spriteAlternativeKeyController, textAfterImage, timerWithText, alternativeKey, timeBetweenTwoCount));
        }
    }
}
