using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTutorialDown : MonoBehaviour
{
    public string textBeforeImage;
    public Sprite spriteKeyKeyBoard;
    public Sprite spriteKeyController;
    public string textAfterImage;
    public float timeToStayToScreen;
    public bool timerWithText;

    public void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            StartCoroutine(TutorialDown.Instance.LaunchTutorial(timeToStayToScreen, textBeforeImage, spriteKeyKeyBoard, spriteKeyController, textAfterImage, timerWithText));
        }
    }
}
