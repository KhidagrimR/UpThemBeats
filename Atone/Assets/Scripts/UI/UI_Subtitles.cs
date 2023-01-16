using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_Subtitles : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SampleSub;

    bool isShown = true;

    Toggle m_Toggle;

    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(delegate {
            ShowOrHideSubs();
        });
    }

    public void ShowOrHideSubs()
    {
        if (isShown)
        {
            SampleSub.enabled = isShown = false;
        }
        else
        {
            SampleSub.enabled = isShown = true;
        }
    }
}
