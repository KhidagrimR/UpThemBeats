using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTutorial : MonoBehaviour
{
    public string textBeforeImage;
    public Sprite spriteKeyKeyBoard;
    public Sprite spriteKeyController;
    public string textAfterImage;
    public float timeToStayToScreen;

    public void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            StartCoroutine(Tutorial.Instance.LaunchTutorial(timeToStayToScreen, textBeforeImage, spriteKeyKeyBoard, spriteKeyController, textAfterImage));
        }
    }
}
