using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Checkpoint : MonoBehaviour
{
    public string niveau;
    public string sequence;
    public bool endGame;
    public void OnTriggerEnter(Collider other){
        if(!GameManager.Instance.isReady) {return;}
        
        if (other.CompareTag(PlayerManager.PLAYER_TAG)){
            PlayerManager.Instance.playerController.currentCheckpoint = gameObject.transform.position;
            StartCoroutine(DisplayScore.Instance.DisplayScoreWhenPlay(PlayerManager.scoreSequence));
            SaveScore();
            PlayerManager.scoreSequence = 0;
        }

    }

    public void SaveScore()
    {
        if (!PlayerManager.Instance.scoreBoard.ContainsKey(niveau))
            PlayerManager.Instance.scoreBoard.Add(niveau, new Dictionary<string, float>());
        if (!PlayerManager.Instance.scoreBoard[niveau].ContainsKey(sequence))
            PlayerManager.Instance.scoreBoard[niveau].Add(sequence, PlayerManager.scoreSequence);
            
        if (endGame)
            WriteScoreToFile();
    }

    public void WriteScoreToFile()
    {
        string path = "Assets/ScoreBoard";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string name = GetPlayerName(path);

        StreamWriter sw = new StreamWriter(path + "/" + name + "_Score.txt");

        sw.WriteLine(name);
        sw.WriteLine();
        float scoreTotal = 0;
        foreach (string niveau in PlayerManager.Instance.scoreBoard.Keys)
        {
            sw.WriteLine(niveau + " : ");
            foreach (string sequence in PlayerManager.Instance.scoreBoard[niveau].Keys) {
                sw.WriteLine("    " + sequence + " : " + PlayerManager.Instance.scoreBoard[niveau][sequence].ToString());
                scoreTotal += PlayerManager.Instance.scoreBoard[niveau][sequence];
            }
                
        }
        sw.WriteLine("Score Total : " + scoreTotal);
        sw.Close();
    }

    public string GetPlayerName(string path) {
        string[] files = Directory.GetFiles(path, ".txt");
        PlayerName.Instance.container.SetActive(true);
        bool isSameName = true;
        while (isSameName){
            isSameName = false;
            foreach(string file in files){
                if (PlayerName.Instance.name == file.Split("/")[file.Split("/").Length - 1].Split("_")[0]){
                    isSameName = true;
                    PlayerName.Instance.textError.SetActive(true);
                }  
            }
        }
        return PlayerName.Instance.name;
    }
}
