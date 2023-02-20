using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class Leaderboard : Singleton<Leaderboard>
{
    public List<ScorePlayerDisplay> scorePlayerDisplays = new List<ScorePlayerDisplay>();


    public GameObject containerFinalScore;
    public TextMeshProUGUI finalScore;

    public GameObject containerLeaderBoard;


    public void DisplayLeaderboard() {
        StreamReader sr = new StreamReader(Checkpoint.pathLeaderboard + "/Leaderboard.txt");

        for(int i = 0; i < scorePlayerDisplays.Count; i+=1){
            string line = sr.ReadLine();
            if (line == null)
                break;
            string[] scorePlayer = line.Split(" - ");
            scorePlayerDisplays[i].playerName.text = scorePlayer[0];
            scorePlayerDisplays[i].scoreNumberPlayer.text = scorePlayer[1];
        }

        sr.Close();
    }
}
