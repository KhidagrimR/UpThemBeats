using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PlayerName : Singleton<PlayerName>
{
    public GameObject container;
    public GameObject textError;
    

    public void DisplayName(string name) {
        if (Directory.Exists(Checkpoint.path))
            print("directory exist !!");
        string[] files = Directory.GetFiles(Checkpoint.path, "*.txt");
        print("files length : " + files.Length);
        bool isSameName = false;
        foreach (string file in files){
            print("file : " + file);
            if (name == file.Split("\\")[1].Split("_")[0]){
                isSameName = true;
                textError.SetActive(true);
            }
        }
        if (!isSameName){
            Checkpoint.WriteScoreToFile(name);
            Checkpoint.WriteLeaderBoard();
            container.SetActive(false);
        }
            
    }


}
