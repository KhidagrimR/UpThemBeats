using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;

public class Leaderboard : Singleton<Leaderboard>
{
    public List<ScorePlayerDisplay> scorePlayerDisplays = new List<ScorePlayerDisplay>();


    public GameObject containerFinalScore;

    public TextMeshProUGUI finalScoreEditor;
    public static TextMeshProUGUI  finalScore;

    public GameObject containerLeaderBoard;

    public static string path = "ScoreBoard/PlayerScore";
    public static string pathLeaderboard = "ScoreBoard";

    public void Start() {
        finalScore = finalScoreEditor;
    }

    public void DisplayLeaderboard() {
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/" + pathLeaderboard + "/Leaderboard.txt");

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

    public static void WriteScoreToFile(string name) {

        if (!Directory.Exists(Application.persistentDataPath + "/" + path))
            Directory.CreateDirectory(Application.persistentDataPath + "/" + path);

        print("player name : " + name);

        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + path + "/" + name + "_Score.txt");

        sw.WriteLine(name);
        sw.WriteLine();
        float scoreTotal = 0;
        foreach (string niveau in PlayerManager.scoreBoard.Keys)
        {
            sw.WriteLine(niveau + " : ");
            foreach (string sequence in PlayerManager.scoreBoard[niveau].Keys)
            {
                sw.WriteLine("    " + sequence + " : " + PlayerManager.scoreBoard[niveau][sequence].ToString());
                scoreTotal += PlayerManager.scoreBoard[niveau][sequence];
            }

        }
        sw.WriteLine("Score Total : " + scoreTotal);
        sw.Close();
        print("score :" + scoreTotal);
        finalScore.text = scoreTotal.ToString();
    }

    public static void WriteLeaderBoard() {
        if (!Directory.Exists(Application.persistentDataPath + "/" + pathLeaderboard))
            Directory.CreateDirectory(Application.persistentDataPath + "/" + pathLeaderboard);

        string[] files = Directory.GetFiles(Application.persistentDataPath + "/" + path, "*.txt");
        Dictionary<string, float> leaderboard = new Dictionary<string, float>();

        foreach (string file in files)
        {
            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            while (line != null)
            {
                if (line.Contains("Score Total"))
                    leaderboard.Add(file.Split("\\")[1].Split("_")[0], float.Parse(line.Split(" : ")[1]));
                line = sr.ReadLine();
            }
            sr.Close();
        }

        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + pathLeaderboard + "/Leaderboard.txt");
        List<KeyValuePair<string, float>> sortedLeaderboard = leaderboard.ToList();
        sortedLeaderboard.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
        if (sortedLeaderboard.Count > 30)
        {
            File.Delete(path + "/" + sortedLeaderboard[sortedLeaderboard.Count - 1].Key + "_Score.txt");
            sortedLeaderboard.RemoveAt(sortedLeaderboard.Count - 1);
        }

        foreach (KeyValuePair<string, float> player in sortedLeaderboard)
            sw.WriteLine(player.Key + " - " + player.Value);
        sw.Close();

    }
}
