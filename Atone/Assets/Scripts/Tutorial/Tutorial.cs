using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : Singleton<Tutorial>
{
    public TextMeshProUGUI m_textBeforeImage;
    public Image m_imageKey;
    private Sprite m_spriteKeyKeyBoard;
    private Sprite m_spriteKeyController;
    public TextMeshProUGUI m_textAfterImage;
    public GameObject container;
    public IEnumerator LaunchTutorial(float timeToStayToScreen, string textBeforeImage, Sprite spriteKeyKeyBoard, Sprite spriteKeyController, string textAfterImage) {
        container.SetActive(true);
        m_textBeforeImage.text = textBeforeImage;
        m_spriteKeyKeyBoard = spriteKeyKeyBoard;
        m_spriteKeyController = spriteKeyController;
        if (InputManager.onController)
            m_imageKey.sprite = m_spriteKeyController;
        else
            m_imageKey.sprite = m_spriteKeyKeyBoard;
        m_textAfterImage.text = textAfterImage;
        yield return new WaitForSeconds(timeToStayToScreen);
        container.SetActive(false);
    }

    public void Update() {
        if (InputManager.onController)
            m_imageKey.sprite = m_spriteKeyController;
        else
            m_imageKey.sprite = m_spriteKeyKeyBoard;
    }
}
