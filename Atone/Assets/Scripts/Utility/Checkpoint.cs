using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Checkpoint : MonoBehaviour
{
    public string niveau;
    public string sequence;
    public bool endGame;

    public static string path = "Assets/ScoreBoard/PlayerScore";
    public static string pathLeaderboard = "Assets/ScoreBoard";
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
            
        if (endGame){
            PlayerName.Instance.container.SetActive(true);
        }
            
    }

    public static void WriteScoreToFile(string name)
    {
        
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);


        print("player name : " + name);

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
        Leaderboard.Instance.finalScore.text = scoreTotal.ToString();
    }

    public static void WriteLeaderBoard() {
        if (!Directory.Exists(path))
            print("Repertoire inexistant");

        string[] files = Directory.GetFiles(path, "*.txt");
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

        StreamWriter sw = new StreamWriter(pathLeaderboard + "/Leaderboard.txt");
        List<KeyValuePair<string, float>> sortedLeaderboard = leaderboard.ToList();
        sortedLeaderboard.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
        if (sortedLeaderboard.Count > 30) {
            File.Delete(path + "/" + sortedLeaderboard[sortedLeaderboard.Count - 1].Key + "_Score.txt");
            sortedLeaderboard.RemoveAt(sortedLeaderboard.Count - 1);
        }
             
        foreach (KeyValuePair<string, float> player in sortedLeaderboard)
            sw.WriteLine(player.Key + " - " + player.Value);
        sw.Close();

    }


}
