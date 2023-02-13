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
            PlayerController.checkpoint = gameObject.transform.position;
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

        //GetNamePlayer()
        string name = "Pedro";

        StreamWriter sw = new StreamWriter(path + "/" + name + "Score.txt");

        sw.WriteLine(name);
        sw.WriteLine();
        foreach (string niveau in PlayerManager.Instance.scoreBoard.Keys)
        {
            sw.WriteLine(niveau + " : ");
            foreach (string sequence in PlayerManager.Instance.scoreBoard[niveau].Keys)
                sw.WriteLine("    " + sequence + " : " + PlayerManager.Instance.scoreBoard[niveau][sequence].ToString());
        }
        sw.Close();
    }
}
