using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;

public class Subtitle : Singleton<Subtitle>
{

    public IEnumerator LaunchSubtitle(string whichDialogue){
        StreamReader reader = new StreamReader("Assets/Resources/Dialogues/" + whichDialogue + "/" + whichDialogue + ".txt");
        string line;
        while((line = reader.ReadLine()) != null){
            string[] subtitle = line.Split(new string[] { " | " }, StringSplitOptions.None);
            gameObject.GetComponent<TextMeshProUGUI>().text = subtitle[0];
            yield return new WaitForSeconds(float.Parse(subtitle[1]));
        }
        gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }
}
