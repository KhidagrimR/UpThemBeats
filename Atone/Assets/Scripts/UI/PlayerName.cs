using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PlayerName : Singleton<PlayerName>
{
    public GameObject selectableInputField;
    public GameObject container;
    public GameObject textError;

    public GameObject AtoneTitle;

    

    public void DisplayName(string name) {
        bool isValidName = false;
        if(Directory.Exists(Application.persistentDataPath + "/" + Leaderboard.path)){
            string[] files = Directory.GetFiles(Application.persistentDataPath + "/" + Leaderboard.path, "*.txt");
        
            if (name != ""){
                isValidName = true;
                foreach (string file in files)
                {
                    if (name == file.Split("\\")[1].Split("_")[0])
                    {
                        isValidName = false;
                        textError.SetActive(true);
                    }
                }
            }
        }
        else{
            Directory.CreateDirectory(Application.persistentDataPath + "/" + Leaderboard.path);
            isValidName = true;
        }

        
        print("isValideName : " + isValidName);
        if (isValidName){
            AtoneTitle.SetActive(false);
            Leaderboard.WriteScoreToFile(name);
            Leaderboard.WriteLeaderBoard();
            Leaderboard.Instance.containerFinalScore.SetActive(true);
            container.SetActive(false);
        }
            
    }


}
