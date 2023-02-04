using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScore : Singleton<DisplayScore>
{
    public GameObject container;
    public TextMeshProUGUI scoreText;

    public IEnumerator DisplayScoreWhenPlay(float score) {
        container.SetActive(true);
        scoreText.text = score.ToString();
        yield return new WaitForSeconds(2);
        container.SetActive(false);
    }
}
