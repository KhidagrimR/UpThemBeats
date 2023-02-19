using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableObjectButtonReturn : MonoBehaviour
{
    public GameObject playButton;
    public GameObject continueButton;

    public void SelectObject() {
        if (GameManager.Instance.isGameCurrentlyPaused)
            EventSystem.current.SetSelectedGameObject(continueButton);
        else
            EventSystem.current.SetSelectedGameObject(playButton);
    }
}
