using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;

public class Subtitle : Singleton<Subtitle>
{
    public GameObject subtitleTextUp;
    public GameObject subtitleTextDown;
    public IEnumerator LaunchSubtitle(string whichDialogue)
    {
        //Debug.Log(Application.dataPath+"/Resources/Dialogues/");
        StreamReader reader = new StreamReader(Application.dataPath+"/Resources/Dialogues/" + whichDialogue + ".txt");
        string line;
        while((line = reader.ReadLine()) != null){
            string[] subtitles = line.Split(new string[] { " | " }, StringSplitOptions.None);
            subtitleTextUp.GetComponent<TextMeshProUGUI>().text = subtitles[0].Split(" \\n ")[0];
            if (subtitles[0].Split(" \\n ").Length > 1)
                subtitleTextDown.GetComponent<TextMeshProUGUI>().text = subtitles[0].Split(" \\n ")[1];
            yield return new WaitForSeconds(float.Parse(subtitles[1]));
            subtitleTextUp.GetComponent<TextMeshProUGUI>().text = "";
            subtitleTextDown.GetComponent<TextMeshProUGUI>().text = "";
        }
        
    }
}
