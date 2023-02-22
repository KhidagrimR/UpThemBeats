using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaunchEndGame : MonoBehaviour
{
    public GameObject UI;
    public void OnTriggerEnter(Collider other) {
        //Launch Ending Animation
        DisplayInputName();
    }

    public void DisplayInputName() {
        UI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        PlayerName.Instance.container.SetActive(true);
        EventSystem.current.SetSelectedGameObject(PlayerName.Instance.selectableInputField);
    }
}
