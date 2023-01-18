using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : Singleton<Tutorial>
{
    public IEnumerator LaunchTutorial(float timeToStayToScreen) {
        gameObject.GetComponent<TextMeshProUGUI>().text = "";
        yield return new WaitForSeconds(timeToStayToScreen);
        gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }
}
