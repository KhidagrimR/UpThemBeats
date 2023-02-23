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
        StreamReader reader = new StreamReader(Application.streamingAssetsPath+"/Dialogues/" + whichDialogue + ".txt");
        string line;
        while((line = reader.ReadLine()) != null){
            string[] subtitles = line.Split(new string[] { " | " }, StringSplitOptions.None);
            subtitleTextUp.GetComponent<TextMeshProUGUI>().text = "<mark=" + UI_Subtitles.backgroundColor + " padding='50,50,0,0'>" + subtitles[0].Split(" \\n ")[0] + "</mark>";
            if (subtitles[0].Split(" \\n ").Length > 1)
                subtitleTextDown.GetComponent<TextMeshProUGUI>().text = "<mark=" + UI_Subtitles.backgroundColor + " padding='50,50,0,0'>" + subtitles[0].Split(" \\n ")[1] + "</mark>";

            if(subtitles.Length >= 1)
            {
                Debug.Log("Subtitles = "+subtitles.ToString());
                yield return new WaitForSeconds(float.Parse(subtitles[1]));
            }
            subtitleTextUp.GetComponent<TextMeshProUGUI>().text = "";
            subtitleTextDown.GetComponent<TextMeshProUGUI>().text = "";
        }
        
    }
}
