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
    public TextMeshProUGUI m_textTimer;
    public GameObject container;
    public GameObject containerText;
    public IEnumerator LaunchTutorial(float timeToStayToScreen, string textBeforeImage, Sprite spriteKeyKeyBoard, Sprite spriteKeyController, string textAfterImage, bool timerWithText) {
        container.SetActive(true);
        StartCoroutine(LaunchTimer());
        if (!timerWithText){
            yield return new WaitForSeconds(2.25f);
        }
        containerText.SetActive(true);
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

    public IEnumerator LaunchTimer() {
        for(int i = 3; i > 0; i -= 1){
            m_textTimer.text = i.ToString();
            yield return new WaitForSeconds(0.75f);
        }
        m_textTimer.text = "";
    }
}
