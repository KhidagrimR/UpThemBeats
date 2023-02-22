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

    

    public void DisplayName(string name) {
        string[] files = Directory.GetFiles(Checkpoint.path, "*.txt");
        bool isValidName = false;
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
        print("isValideName : " + isValidName);
        if (isValidName){
            
            Leaderboard.WriteScoreToFile(name);
            Leaderboard.WriteLeaderBoard();
            Leaderboard.Instance.containerFinalScore.SetActive(true);
            container.SetActive(false);
        }
            
    }


}
