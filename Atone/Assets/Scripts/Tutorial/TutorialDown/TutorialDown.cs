using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDown : Singleton<TutorialDown> {
    public TextMeshProUGUI m_textBeforeImage;
    public Image m_imageKey;
    public Image m_imageAlternativeKey;

    private Sprite m_spriteKeyKeyBoardRightHanded;
    private Sprite m_spriteAlternativeKeyKeyBoardRightHanded;

    private Sprite m_spriteKeyKeyBoardLeftHanded;
    private Sprite m_spriteAlternativeKeyKeyBoardLeftHanded;

    private Sprite m_spriteKeyController;
    private Sprite m_spriteAlternativeKeyController;


    public TextMeshProUGUI m_textAfterImage;
    public TextMeshProUGUI m_textTimer;

    public GameObject container;
    public GameObject containerText;
    public GameObject containerAlternativeKey;
    public IEnumerator LaunchTutorial(float timeToStayToScreen, string textBeforeImage,
                                      Sprite spriteKeyKeyBoardRightHanded, Sprite spriteAlternativeKeyKeyBoardRightHanded,
                                      Sprite spriteKeyKeyBoardLeftHanded, Sprite spriteAlternativeKeyKeyBoardLeftHanded,
                                      Sprite spriteKeyController, Sprite spriteAlternativeKeyController, 
                                      string textAfterImage, bool timerWithText, bool alternativeKey, float timeBetweenCount) {
        container.SetActive(true);
        if (alternativeKey)
            containerAlternativeKey.SetActive(true);
        StartCoroutine(LaunchTimer(timeBetweenCount));
        if (!timerWithText)
        {
            yield return new WaitForSeconds(timeBetweenCount*3);
        }
        containerText.SetActive(true);
        m_textBeforeImage.text = textBeforeImage;

        m_spriteKeyKeyBoardRightHanded = spriteKeyKeyBoardRightHanded;
        m_spriteAlternativeKeyKeyBoardRightHanded = spriteAlternativeKeyKeyBoardRightHanded;

        m_spriteKeyKeyBoardLeftHanded = spriteKeyKeyBoardLeftHanded;
        m_spriteAlternativeKeyKeyBoardLeftHanded = spriteAlternativeKeyKeyBoardLeftHanded;

        m_spriteKeyController = spriteKeyController;
        m_spriteAlternativeKeyController = spriteAlternativeKeyController;

        if (InputManager.onController)
        {
            m_imageKey.sprite = m_spriteKeyController;
            m_imageAlternativeKey.sprite = m_spriteAlternativeKeyController;
        }
        else
        {
            if (InputManager.isRightHanded)
            {
                m_imageKey.sprite = m_spriteKeyKeyBoardRightHanded;
                m_imageAlternativeKey.sprite = m_spriteAlternativeKeyKeyBoardRightHanded;
            }
            else
            {
                m_imageKey.sprite = m_spriteKeyKeyBoardLeftHanded;
                m_imageAlternativeKey.sprite = m_spriteAlternativeKeyKeyBoardLeftHanded;
            }
        }

        m_imageKey.sprite = m_spriteKeyKeyBoardRightHanded;
        m_textAfterImage.text = textAfterImage;
        yield return new WaitForSeconds(timeToStayToScreen);

        containerAlternativeKey.SetActive(false);
        containerText.SetActive(false);
        container.SetActive(false);
    }

    public void Update() {
        if (InputManager.onController)
            m_imageKey.sprite = m_spriteKeyController;
        else
            m_imageKey.sprite = m_spriteKeyKeyBoardRightHanded;
    }

    public IEnumerator LaunchTimer(float timeBetweenCount) {
        for (int i = 3; i > 0; i -= 1)
        {
            m_textTimer.text = i.ToString();
            yield return new WaitForSeconds(timeBetweenCount);
        }
        m_textTimer.text = "";
    }
}
