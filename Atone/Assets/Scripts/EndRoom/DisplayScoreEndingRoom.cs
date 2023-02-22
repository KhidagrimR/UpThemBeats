using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayScoreEndingRoom : MonoBehaviour
{
    public GameObject UI;
    public void DisplayInputName() {
        UI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        PlayerName.Instance.container.SetActive(true);
        EventSystem.current.SetSelectedGameObject(PlayerName.Instance.selectableInputField);
    }
}
